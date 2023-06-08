using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EsportaXML
{
    class Esporta
    {
        public void EsportaTransazioni()
        {
            Zero5.Data.Layer.Transazioni transazioniDaEsportare = new Zero5.Data.Layer.Transazioni();

            Zero5.Data.Layer.CausaliAttivita causaliLavorazioneMacchinaDaEsportare = new Zero5.Data.Layer.CausaliAttivita();
            //Zero5.Data.Layer.CausaliAttivita causaliSetupMacchinaDaEsportare = new Zero5.Data.Layer.CausaliAttivita();

            Zero5.Data.Layer.CausaliAttivita causaliLavorazioneUomoDaEsportare = new Zero5.Data.Layer.CausaliAttivita();
            Zero5.Data.Layer.CausaliAttivita causaliSetupUomoDaEsportare = new Zero5.Data.Layer.CausaliAttivita();

            Zero5.Data.Layer.Risorse macchine = new Zero5.Data.Layer.Risorse();
            macchine.LoadByTipo(Zero5.Data.Layer.Risorse.eTipo.Macchina);
            Zero5.Data.Layer.Risorse uominiIND = new Zero5.Data.Layer.Risorse();
            uominiIND.Load(uominiIND.Fields.Descrizione.FilterLikeTo("%-INDIRETTO-%"));
            Zero5.Data.Layer.Risorse uominiDIR = new Zero5.Data.Layer.Risorse();
            uominiDIR.Load(uominiDIR.Fields.Descrizione.FilterLikeTo("%-DIRETTO-%"));

            Zero5.Data.Layer.Risorse uomini = new Zero5.Data.Layer.Risorse();
            uomini.Load(uomini.Fields.Tipo == Zero5.Data.Layer.Risorse.eTipo.Uomo);

            Zero5.Data.Layer.TipoAttivita attIndirette = new Zero5.Data.Layer.TipoAttivita();
            attIndirette.Load(
                new Zero5.Data.Filter.FilterItem[] {
                    attIndirette.Fields.Codice == "IND",
                    attIndirette.Fields.PerUomo == Zero5.Data.Layer.TipoAttivita.eZeroFalseOneTrue.Yes_True
                });

            Zero5.Data.Layer.vTipiAttivitaCausaliAttivita causaliAttivitaIndirette = new Zero5.Data.Layer.vTipiAttivitaCausaliAttivita();
            causaliAttivitaIndirette.Load(causaliAttivitaIndirette.Fields.Attivita_Codice == "IND", causaliAttivitaIndirette.Fields.Attivita_PerUomo == Zero5.Data.Layer.TipoAttivita.eZeroFalseOneTrue.Yes_True);

            List<int> lstCausaliAttivitaIndirette = causaliAttivitaIndirette.GetIntListFromField(causaliAttivitaIndirette.Fields.Causale_IDCausaleAttivita);


            {
                //if (tipoTransazioniDaEsportare == "tempiMacchina")
                {
                    Zero5.Data.Filter.Filter filtro = new Zero5.Data.Filter.Filter();

                    {
                        filtro.Add(causaliLavorazioneMacchinaDaEsportare.Fields.TempiMacchinaSuFase == Zero5.Data.Layer.CausaliAttivita.enumTempiMacchinaSuFase.CicloLavorazione);
                        filtro.AddOR();
                        filtro.Add(causaliLavorazioneMacchinaDaEsportare.Fields.TempiMacchinaSuFase == Zero5.Data.Layer.CausaliAttivita.enumTempiMacchinaSuFase.CicloFermoOperativo);

                        causaliLavorazioneMacchinaDaEsportare.Load(filtro);
                    }

                    filtro.Clear();

                    {
                        filtro.Add(causaliLavorazioneUomoDaEsportare.Fields.TempiUomoSuFase == Zero5.Data.Layer.CausaliAttivita.enumTempiUomoSuFase.CicloLavorazione);

                        causaliLavorazioneUomoDaEsportare.Load(filtro);
                    }

                    filtro.Clear();

                    {
                        filtro.Add(causaliSetupUomoDaEsportare.Fields.TempiUomoSuFase == Zero5.Data.Layer.CausaliAttivita.enumTempiUomoSuFase.Setup);

                        causaliSetupUomoDaEsportare.Load(filtro);
                    }

                    List<int> lstCausali = new List<int>();
                    lstCausali.Concat(causaliLavorazioneMacchinaDaEsportare.GetIntListFromPrimaryKey());
                    lstCausali.Concat(causaliLavorazioneUomoDaEsportare.GetIntListFromPrimaryKey());
                    lstCausali.Concat(causaliSetupUomoDaEsportare.GetIntListFromPrimaryKey());

                    DateTime dtStart = DateTime.Now;
                    DateTime dtEnd = DateTime.Now;

                    //if (!causaliSetupMacchinaDaEsportare.EOF)
                    {
                        Zero5.Data.Layer.vOrdiniProduzioneFasiProduzioneTransazioni vOrdiniFasiTransazioni = new Zero5.Data.Layer.vOrdiniProduzioneFasiProduzioneTransazioni();

                        {
                            Zero5.Data.Filter.Filter filter = new Zero5.Data.Filter.Filter();
                            filter.Add(vOrdiniFasiTransazioni.Fields.Transazione_Esportato == 0);
                            //filter.Add(vOrdiniFasiTransazioni.Fields.Transazione_Inizio > DateTime.Now.AddDays(-7));
                            filter.Add(vOrdiniFasiTransazioni.Fields.Transazione_Causale.FilterIn(lstCausali));
                            filter.Add(vOrdiniFasiTransazioni.Fields.Transazione_Fine <= DateTime.Now.AddMinutes(-1));
                            filter.Add(vOrdiniFasiTransazioni.Fields.Transazione_Minuti > 0);
                            filter.AddOrderBy(vOrdiniFasiTransazioni.Fields.Transazione_IDFaseProduzione, Zero5.Data.Filter.eSortOrder.ASC);
                            filter.AddOrderBy(vOrdiniFasiTransazioni.Fields.Transazione_IDRisorsaMacchina, Zero5.Data.Filter.eSortOrder.ASC);
                            filter.AddOrderBy(vOrdiniFasiTransazioni.Fields.Transazione_IDRisorsaUomo, Zero5.Data.Filter.eSortOrder.ASC);
                            filter.AddOrderBy(vOrdiniFasiTransazioni.Fields.Transazione_Fine, Zero5.Data.Filter.eSortOrder.DESC);
                            vOrdiniFasiTransazioni.Load(filter);
                        }
                        Zero5.Util.Log.WriteLog("Caricate " + vOrdiniFasiTransazioni.RowCount + " transazioni");

                        Dictionary<int, double> dicMinutiLavoroMacchinabyFase = new Dictionary<int, double>();
                        Dictionary<int, double> dicMinutiSetupDIRETTIbyFase = new Dictionary<int, double>();

                        Dictionary<int, Dictionary<int, double>> dicMinutiLavoroUomobyFase = new Dictionary<int, Dictionary<int, double>>();
                        Dictionary<int, Dictionary<int, double>> dicMinutiSetupUomobyFase = new Dictionary<int, Dictionary<int, double>>();

                        Dictionary<int, List<int>> dicTransazioniByIDFase = new Dictionary<int, List<int>>();

                        {
                            vOrdiniFasiTransazioni.MoveFirst();
                            while (!vOrdiniFasiTransazioni.EOF)
                            {
                                if (vOrdiniFasiTransazioni.Transazione_Inizio < dtStart)
                                    dtStart = vOrdiniFasiTransazioni.Transazione_Inizio;
                                if (vOrdiniFasiTransazioni.Transazione_Fine > dtEnd)
                                    dtEnd = vOrdiniFasiTransazioni.Transazione_Fine;

                                if (!dicMinutiLavoroMacchinabyFase.ContainsKey(vOrdiniFasiTransazioni.Fase_IDFaseProduzione))
                                    dicMinutiLavoroMacchinabyFase.Add(vOrdiniFasiTransazioni.Fase_IDFaseProduzione, 0);
                                if (!dicMinutiSetupDIRETTIbyFase.ContainsKey(vOrdiniFasiTransazioni.Fase_IDFaseProduzione))
                                    dicMinutiSetupDIRETTIbyFase.Add(vOrdiniFasiTransazioni.Fase_IDFaseProduzione, 0);
                                if (!dicMinutiLavoroUomobyFase.ContainsKey(vOrdiniFasiTransazioni.Fase_IDFaseProduzione))
                                    dicMinutiLavoroUomobyFase.Add(vOrdiniFasiTransazioni.Fase_IDFaseProduzione, new Dictionary<int, double>());
                                if (!dicMinutiSetupUomobyFase.ContainsKey(vOrdiniFasiTransazioni.Fase_IDFaseProduzione))
                                    dicMinutiSetupUomobyFase.Add(vOrdiniFasiTransazioni.Fase_IDFaseProduzione, new Dictionary<int, double>());


                                if (vOrdiniFasiTransazioni.Transazione_IDRisorsaMacchina != 0)
                                {
                                    if (causaliLavorazioneMacchinaDaEsportare.GetIntListFromPrimaryKey().Contains(vOrdiniFasiTransazioni.Transazione_Causale))
                                        dicMinutiLavoroMacchinabyFase[vOrdiniFasiTransazioni.Fase_IDFaseProduzione] += vOrdiniFasiTransazioni.Transazione_Minuti;
                                }

                                if (vOrdiniFasiTransazioni.Transazione_IDRisorsaUomo != 0)
                                {
                                    if (uominiIND.GetIntListFromPrimaryKey().Contains(vOrdiniFasiTransazioni.Transazione_IDRisorsaUomo) ||
                                        lstCausaliAttivitaIndirette.Contains(vOrdiniFasiTransazioni.Transazione_Causale))
                                    {
                                        if (causaliLavorazioneUomoDaEsportare.GetIntListFromPrimaryKey().Contains(vOrdiniFasiTransazioni.Transazione_Causale))
                                        {
                                            if (!dicMinutiLavoroUomobyFase[vOrdiniFasiTransazioni.Fase_IDFaseProduzione].ContainsKey(vOrdiniFasiTransazioni.Transazione_IDRisorsaUomo))
                                                dicMinutiLavoroUomobyFase[vOrdiniFasiTransazioni.Fase_IDFaseProduzione].Add(vOrdiniFasiTransazioni.Transazione_IDRisorsaUomo, 0);

                                            dicMinutiLavoroUomobyFase[vOrdiniFasiTransazioni.Fase_IDFaseProduzione][vOrdiniFasiTransazioni.Transazione_IDRisorsaUomo] += vOrdiniFasiTransazioni.Transazione_Minuti;
                                        }

                                        if (causaliSetupUomoDaEsportare.GetIntListFromPrimaryKey().Contains(vOrdiniFasiTransazioni.Transazione_Causale))
                                        {
                                            if (!dicMinutiSetupUomobyFase[vOrdiniFasiTransazioni.Fase_IDFaseProduzione].ContainsKey(vOrdiniFasiTransazioni.Transazione_IDRisorsaUomo))
                                                dicMinutiSetupUomobyFase[vOrdiniFasiTransazioni.Fase_IDFaseProduzione].Add(vOrdiniFasiTransazioni.Transazione_IDRisorsaUomo, 0);

                                            dicMinutiSetupUomobyFase[vOrdiniFasiTransazioni.Fase_IDFaseProduzione][vOrdiniFasiTransazioni.Transazione_IDRisorsaUomo] += vOrdiniFasiTransazioni.Transazione_Minuti;
                                        }
                                    }
                                    else if (uominiDIR.GetIntListFromPrimaryKey().Contains(vOrdiniFasiTransazioni.Transazione_IDRisorsaUomo))
                                    {

                                        if (causaliSetupUomoDaEsportare.GetIntListFromPrimaryKey().Contains(vOrdiniFasiTransazioni.Transazione_Causale))
                                            dicMinutiSetupDIRETTIbyFase[vOrdiniFasiTransazioni.Fase_IDFaseProduzione] += vOrdiniFasiTransazioni.Transazione_Minuti;
                                    }
                                }

                                if (!dicTransazioniByIDFase.ContainsKey(vOrdiniFasiTransazioni.Fase_IDFaseProduzione))
                                    dicTransazioniByIDFase.Add(vOrdiniFasiTransazioni.Fase_IDFaseProduzione, new List<int>());

                                dicTransazioniByIDFase[vOrdiniFasiTransazioni.Fase_IDFaseProduzione].Add(vOrdiniFasiTransazioni.Transazione_IDTransazione);

                                vOrdiniFasiTransazioni.MoveNext();
                            }

                            vOrdiniFasiTransazioni.MoveFirst();
                        }

                        Zero5.Data.Layer.Transazioni transazioniSave = new Zero5.Data.Layer.Transazioni();

                        using (new Zero5.Process.Impersonator("administrator", "nucon", "Polipo%A"))
                        {
                            using (XmlWriter writer = XmlWriter.Create(Program.Parametri.PercorsoFileXML + "EsportatorePhase.xml"))
                            {
                                writer.WriteStartElement("ElementsList");

                                foreach (KeyValuePair<int, List<int>> kvp in dicTransazioniByIDFase)
                                {
                                    vOrdiniFasiTransazioni.MoveToNextFieldValue(vOrdiniFasiTransazioni.Fields.Fase_IDFaseProduzione, kvp.Key, true);

                                    Zero5.Data.Layer.Transazioni transazioniPezzi = new Zero5.Data.Layer.Transazioni();
                                    filtro = new Zero5.Data.Filter.Filter();
                                    filtro.Add(transazioniPezzi.Fields.IDFaseProduzione == kvp.Key);
                                    filtro.Add(transazioniPezzi.Fields.Minuti == 0);
                                    filtro.Add(transazioniPezzi.Fields.PezziBuoni > 0);
                                    filtro.Add(transazioniPezzi.Fields.Esportato == 0);
                                    transazioniPezzi.Load(filtro);

                                    int pezziBuoniDaEsportare = 0;
                                    while (!transazioniPezzi.EOF)
                                    {
                                        pezziBuoniDaEsportare += (int)transazioniPezzi.PezziBuoni;
                                        transazioniPezzi.MoveNext();
                                    }

                                    DateTime data = DateTime.Now;

                                    writer.WriteStartElement("Element");
                                    writer.WriteStartElement("MandatoryFields");
                                    writer.WriteElementString("MONo", vOrdiniFasiTransazioni.Ordine_Codice.ToString());
                                    writer.WriteElementString("RtgStep", vOrdiniFasiTransazioni.Fase_NumeroFase.ToString());
                                    writer.WriteElementString("AltRtgStep", "0");
                                    writer.WriteElementString("Alternate", "");
                                    writer.WriteEndElement();
                                    writer.WriteStartElement("OptionalFields");
                                    writer.WriteElementString("ActualWC", "");
                                    //writer.WriteElementString("ActualWC", vOrdiniFasiTransazioni.Fase_Operazione.ToString());
                                    writer.WriteElementString("StartingDate", dtStart.ToString(@"dd/MM/yyyy HH\:mm\:ss"));
                                    writer.WriteElementString("EndingDate", dtEnd.ToString(@"dd/MM/yyyy HH\:mm\:ss"));
                                    writer.WriteElementString("ProducedQty", pezziBuoniDaEsportare.ToString());
                                    writer.WriteElementString("ProcessingQty", pezziBuoniDaEsportare.ToString());
                                    writer.WriteElementString("Storage", vOrdiniFasiTransazioni.Fase_NoteTecniche.ToString());
                                    //writer.WriteElementString("Storage", "SEDE");
                                    //writer.WriteElementString("Scrap", "rottame");
                                    //writer.WriteElementString("ScrapQty", vOrdiniFasiTransazioni.Fase_QtaScartoTotale.ToString());
                                    //writer.WriteElementString("ScrapStorage", "SCARTI");
                                    //writer.WriteElementString("SecondRate", "A-2scelta");
                                    //writer.WriteElementString("SecondRateQty", "0");
                                    // writer.WriteElementString("SecondRateStorage", "FOCE");
                                    writer.WriteElementString("ActualSetupTime", GetTimeSpanRoundUp(TimeSpan.FromMinutes(dicMinutiSetupDIRETTIbyFase[kvp.Key])));
                                    writer.WriteElementString("ActualProcessingTime", GetTimeSpanRoundUp(TimeSpan.FromMinutes(dicMinutiLavoroMacchinabyFase[kvp.Key])));
                                    //writer.WriteElementString("HourlyCost", "0");
                                    //writer.WriteElementString("UnitCost", "0");
                                    //writer.WriteElementString("AdditionalCost", "0");

                                    //if (vOrdiniFasiTransazioni.Fase_Stato == Zero5.Data.Layer.FasiProduzione.enumFasiProduzioneStati.Finita)
                                    //writer.WriteElementString("SetConfirmed", "True");
                                    //else
                                    //writer.WriteElementString("SetConfirmed", "False");

                                    writer.WriteEndElement();
                                    writer.WriteStartElement("LabourList");
                                    foreach (KeyValuePair<int, double> kvu in dicMinutiLavoroUomobyFase[kvp.Key])
                                    {
                                        uomini.MoveToPrimaryKey(kvu.Key);

                                        writer.WriteStartElement("Labour");
                                        writer.WriteElementString("IsWorker", "True");
                                        writer.WriteElementString("ResourceType", "");
                                        writer.WriteElementString("LabourType", "28508160");
                                        writer.WriteElementString("Resource", "");
                                        writer.WriteElementString("Worker", uomini.Codice.ToString());
                                        writer.WriteElementString("WorkingTime", GetTimeSpanRoundUp(TimeSpan.FromMinutes(kvu.Value)));
                                        writer.WriteElementString("LabourDate", DateTime.Now.ToString(@"dd/MM/yyyy"));
                                        writer.WriteElementString("NoOfResources", "1");
                                        writer.WriteEndElement();
                                    }

                                    foreach (KeyValuePair<int, double> kvs in dicMinutiSetupUomobyFase[kvp.Key])
                                    {
                                        uomini.MoveToPrimaryKey(kvs.Key);

                                        writer.WriteStartElement("Labour");
                                        writer.WriteElementString("IsWorker", "True");
                                        writer.WriteElementString("ResourceType", "");
                                        writer.WriteElementString("LabourType", "28508161");
                                        writer.WriteElementString("Resource", "");
                                        writer.WriteElementString("Worker", uomini.Codice.ToString());
                                        writer.WriteElementString("WorkingTime", GetTimeSpanRoundUp(TimeSpan.FromMinutes(kvs.Value)));
                                        writer.WriteElementString("LabourDate", DateTime.Now.ToString(@"dd/MM/yyyy"));
                                        writer.WriteElementString("NoOfResources", "1");
                                        writer.WriteEndElement();
                                    }

                                    writer.WriteEndElement();
                                    writer.WriteEndElement();

                                    List<List<int>> listalunga = Zero5.Util.Common.SplitList(kvp.Value, 100);
                                    
                                    foreach (List<int> listacorta in listalunga)
                                    {
                                        if (listacorta.Count > 0)
                                        {
                                            transazioniSave.Load(transazioniSave.Fields.IDTransazione.FilterIn(listacorta));
                                            while (!transazioniSave.EOF)
                                            {
                                                transazioniSave.Esportato = 1;
                                                transazioniSave.MoveNext();
                                            }
                                            transazioniSave.Save();
                                        }
                                    }


                                    transazioniPezzi.MoveFirst();
                                    while (!transazioniPezzi.EOF)
                                    {
                                        transazioniPezzi.Esportato = 1;
                                        transazioniPezzi.MoveNext();
                                    }
                                    transazioniPezzi.Save();
                                }

                                writer.WriteEndElement();
                                writer.Flush();
                            }
                        }
                    }
                }
            }
        }

        private string GetTimeSpanRoundUp(TimeSpan ts)
        {
            int minuti = ts.Minutes;
            int ore = (int)ts.TotalHours;

            if (ts.Seconds > 0)
                minuti = ts.Minutes + 1;

            return string.Format("{0}:{1}:{2}", ore.ToString().PadLeft(2,'0'), minuti.ToString().PadLeft(2,'0'), "00");
        }

    }
}
