using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
namespace Importa
{
    class ImportaOrdiniPersonalizzato
    {
        public void Esegui()
        {
            idFasiImportate = new List<int>();

            Zero5.Server.Programmazione srvProg = new Zero5.Server.Programmazione();
            Zero5.Server.Produzione srvProduzione = new Zero5.Server.Produzione();

            Program.Parametri.DatabaseGestionale = "";
            string select = "SELECT * FROM TP_OdpFase ORDER BY Prioritaodp, DataConsegnaOdp ";

            Zero5.Data.Layer.TP_ODPFASE ordiniGestionale = new Zero5.Data.Layer.TP_ODPFASE(Program.Parametri.DatabaseGestionale, select);

            Zero5.Util.Log.WriteLog("ImportaOrdini", "Trovati " + ordiniGestionale.RowCount.ToString() + " ordini.");
            if (ordiniGestionale.RowCount == 0) return;

            Zero5.Data.Layer.OrdiniProduzione ordini = new Zero5.Data.Layer.OrdiniProduzione();
            ordini.LoadNone();

            Zero5.Data.Layer.FasiProduzione fasi = new Zero5.Data.Layer.FasiProduzione();
            fasi.LoadNone();

            Zero5.Data.Layer.Risorse macchine = new Zero5.Data.Layer.Risorse();
            macchine.LoadByTipo(Zero5.Data.Layer.Risorse.eTipo.Macchina);

            Zero5.Data.Layer.Articoli articoli = new Zero5.Data.Layer.Articoli();
            articoli.LoadAll();

            Zero5.Data.Layer.ClientiFornitori clienti = new Zero5.Data.Layer.ClientiFornitori();
            clienti.Load(clienti.Fields.IsCliente == true);

            Zero5.Data.Layer.DestinazioniClientiFornitori destinazioni = new Zero5.Data.Layer.DestinazioniClientiFornitori();
            destinazioni.LoadAll();

            List<int> lstIDOrdiniImportati = new List<int>();

            while (!ordiniGestionale.EOF)
            {
                try
                {
                    System.Threading.Thread.Sleep(1);

                    articoli.MoveToNextFieldValue(articoli.Fields.CodiceArticolo, ordiniGestionale.Codarticolo, true);
                    if (articoli.EOF)
                    {
                        articoli.AddNewAndNewID();
                        articoli.CodiceArticolo = ordiniGestionale.Codarticolo.Trim();
                        articoli.Descrizione = ordiniGestionale.Descriarticolo.Trim();
                        articoli.Save();
                    }

                    ordini.LoadByCodice(ordiniGestionale.Nrodp);
                    if (ordini.EOF)
                    {
                        ordini.AddNew();
                        ordini.IDOrdineProduzione = ordini.PrimaryKeyGetNewValue();
                        ordini.Codice = ordiniGestionale.Nrodp;
                        ordini.Articolo = articoli.CodiceArticolo;
                        ordini.ArticoloDescrizione = articoli.Descrizione;
                        ordini.DataOrdine = DateTime.Now.Date;
                        ordini.ConsegnaPrevista = ordiniGestionale.Dataconsegnaodp;

                        Zero5.Util.Log.WriteLog("ImportaOrdini", "Creato ordine " + ordini.Codice + ".");
                    }
                    ordini.QuantitaRichiesta = ordiniGestionale.Qtaodp;
                    ordini.Save();
                   
                    if (!lstIDOrdiniImportati.Contains(ordini.IDOrdineProduzione))
                        lstIDOrdiniImportati.Add(ordini.IDOrdineProduzione);
                    
                    fasi.LoadByIDOrdineProduzioneAndNumeroFase(ordini.IDOrdineProduzione, Convert.ToInt32(ordiniGestionale.Nrfase));
                    if (fasi.EOF)
                    {
                        fasi.AddNew();
                        fasi.IDFaseProduzione = fasi.PrimaryKeyGetNewValue();
                        fasi.IDOrdineProduzione = ordini.IDOrdineProduzione;
                        fasi.Stato = Zero5.Data.Layer.FasiProduzione.enumFasiProduzioneStati.Inserita;
                        fasi.NumeroFase = Convert.ToInt32(ordiniGestionale.Nrfase);
                        fasi.Descrizione = ordiniGestionale.Descfase;
                    }
                    fasi.InizioEntro = ordiniGestionale.Datainiziofase.ToLocalTime();
                    fasi.FineEntro = ordiniGestionale.Datafinefase.ToLocalTime();
                    fasi.Priorita = ordiniGestionale.Prioritaodp;
                    fasi.PartProgram = ordiniGestionale.Idprogramma;

                    macchine.MoveToNextFieldValue(macchine.Fields.CodiceEsterno, ordiniGestionale.Cdlprevisto.Trim(), true);
                    fasi.IDRisorsaMacchinaPrevista = macchine.IDRisorsa;

                    fasi.QtaPrevista = ordiniGestionale.Qtaodp;
                    fasi.NoteTecniche = ordiniGestionale.Deposito;
                    if (ordiniGestionale.Templavorazione > 0)
                    {
                        //fasi.TPrevMachUniCiclo = ordiniGestionale.Templavorazione / ordiniGestionale.Qtaodp;
                        //fasi.TPrevMachUniCicloLavorazione = ordiniGestionale.Templavorazione / ordiniGestionale.Qtaodp;
                        //fasi.TPrevMachTotCiclo = ordiniGestionale.Templavorazione;
                        //fasi.TPrevMachTotCicloLavorazione = ordiniGestionale.Templavorazione;

                        fasi.TPrevMachUniCiclo = ordiniGestionale.Templavorazione / 60 / ordiniGestionale.Qtaodp;
                        fasi.TPrevMachUniCicloLavorazione = ordiniGestionale.Templavorazione / 60 / ordiniGestionale.Qtaodp;
                        fasi.TPrevMachTotCiclo = ordiniGestionale.Templavorazione / 60;
                        fasi.TPrevMachTotCicloLavorazione = ordiniGestionale.Templavorazione / 60;

                        //Zero5.Data.Layer.FasiProgrammate prog = new Zero5.Data.Layer.FasiProgrammate();
                        //prog.Load(prog.Fields.IDFaseProduzione == fasi.IDFaseProduzione);
                        //prog.TPrevMachUniCiclo = ordiniGestionale.TempoProd * 60 / ordiniGestionale.Qta;
                        //prog.TPrevMachUniCicloLavorazione = ordiniGestionale.TempoProd * 60 / ordiniGestionale.Qta;
                        //prog.TPrevMachTotCiclo = ordiniGestionale.TempoProd * 60;
                        //prog.TPrevMachTotCicloLavorazione = ordiniGestionale.TempoProd * 60;
                        //prog.Save();
                    }
                    else
                    {
                        fasi.TPrevMachUniCiclo = 20;
                        fasi.TPrevMachUniCicloLavorazione = 20;
                        fasi.TPrevMachTotCiclo = 20 * ordiniGestionale.Qtadaprod;
                        fasi.TPrevMachTotCicloLavorazione = 20 * ordiniGestionale.Qtadaprod;
                    }

                    if (fasi.Stato == Zero5.Data.Layer.FasiProduzione.enumFasiProduzioneStati.Inserita)
                    {
                        macchine.MoveToNextFieldValue(macchine.Fields.IDRisorsa, fasi.IDRisorsaMacchinaPrevista, true);
                        bool abilitaProgrammazione = macchine.AbilitaProgrammazione == Zero5.Data.Layer.Risorse.eAbilitaProgrammazione.Si ||
                                                     macchine.AbilitaProgrammazione == Zero5.Data.Layer.Risorse.eAbilitaProgrammazione.RigaSingola;
                        if (!srvProg.EsisteFaseProgrammataNonLavorataSuMacchina(fasi.IDFaseProduzione, fasi.IDRisorsaMacchinaPrevista) && abilitaProgrammazione)
                                srvProg.AggiungiFaseProgrammataSuMacchinaInCoda(fasi.IDFaseProduzione, fasi.IDRisorsaMacchinaPrevista, fasi.QtaPrevista);
                    }

                    idFasiImportate.Add(fasi.IDFaseProduzione);

                    fasi.Save();

                    if (ordiniGestionale.Statofase == "TERMINATA")
                    {
                        try
                        {
                            if (fasi.Stato != Zero5.Data.Layer.FasiProduzione.enumFasiProduzioneStati.Finita)
                            {
                                if (fasi.Stato != Zero5.Data.Layer.FasiProduzione.enumFasiProduzioneStati.Aperta)
                                {
                                    srvProduzione.EseguiChiusuraFaseProduzione(fasi.IDFaseProduzione);
                                    Zero5.Util.Log.WriteLog("ImportaOrdini", "Chiusura automatica della fase " + fasi.IDFaseProduzione);
                                }
                                else
                                {
                                    string note = "Ordine: " + ordini.Codice + " Fase: " + fasi.Descrizione + " - Chiusa lato Mago, ma attualmente è aperta in Phase";
                                    Program.GeneraAllarme("WarningChiusuraFase", note);
                                    Zero5.Util.Log.WriteLog("ImportaOrdini", note);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Zero5.Util.Log.WriteLog("ImportaOrdini", "Errore chiusura automatica fase " + fasi.IDFaseProduzione + " - " + ex.Message);
                        }
                    }

                    if (ordiniGestionale.Statoordine == "TERMINATO" && ordini.Stato != Zero5.Data.Layer.OrdiniProduzione.enumOrdiniProduzioneStati.Chiuso)
                    {
                        try
                        {
                            srvProduzione.OrdiniProduzione_ChiudiOrdine(ordini.IDOrdineProduzione, false);

                            Zero5.Util.Log.WriteLog("ImportaOrdini", "Chiusura automatica dell'ordine " + ordini.IDOrdineProduzione);

                            Zero5.Data.Layer.OrdiniProduzione ordineProduzioneCheck = new Zero5.Data.Layer.OrdiniProduzione();
                            ordineProduzioneCheck.LoadByPrimaryKey(ordini.IDOrdineProduzione);

                            if (ordineProduzioneCheck.Stato != Zero5.Data.Layer.OrdiniProduzione.enumOrdiniProduzioneStati.Chiuso)
                            {
                                string note = "Ordine: " + ordini.Codice + " - Chiuso lato Mago, ma è ancora aperto in Phase";
                                Program.GeneraAllarme("WarningChiusuraOrdine", note);
                                Zero5.Util.Log.WriteLog("ImportaOrdini", note);
                            }
                        }
                        catch (Exception ex)
                        {
                            Zero5.Util.Log.WriteLog("ImportaOrdini", "Errore chiusura automatica ordine " + ordini.IDOrdineProduzione + " - " + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Zero5.Util.Log.WriteLog("ImportaOrdini", "Errore non gestito ordine " + ordiniGestionale.Nrodp.Trim() + ": " + ex.Message);
                }

                if (ordiniGestionale.RowIndex % 500 == 0)
                    Zero5.Util.Log.WriteLog("ImportaOrdini", "Ordini Rimanenti: " + (ordiniGestionale.RowCount - ordiniGestionale.RowIndex).ToString());

                ordiniGestionale.MoveNext();
            }

            Zero5.Data.Layer.FasiProduzione fpPhase = new Zero5.Data.Layer.FasiProduzione();
            fpPhase.Load(fpPhase.Fields.Stato == Zero5.Data.Layer.FasiProduzione.enumFasiProduzioneStati.Inserita);

            while (!fpPhase.EOF)
            {
                if (!idFasiImportate.Contains(fpPhase.IDFaseProduzione))
                {
                    Zero5.Util.Log.WriteLog("ImportaOrdini", "Chiusura automatica Fase: " + fpPhase.CodiceBolla);
                    srvProduzione.FaseProduzione_Elimina(fpPhase.IDFaseProduzione);
                }

                fpPhase.MoveNext();
            }

            fpPhase.Save();
        }

        List<int> idFasiImportate = new List<int>();
    }
}