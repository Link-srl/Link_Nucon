using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Importa
{
    static class Program
    {
        public static FileConfigurazione Parametri = new FileConfigurazione();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Zero5.Data.Link.TCPDataLink.ServerIP = "127.0.0.1";

            if (!Zero5.Threading.SingleInstance.ImAloneWithinSystem())
            {
                Zero5.Util.Log.WriteLog("ImportaOrdini", "...double istance, closing.");
                return;
            }

            try
            {
                Zero5.Util.Log.WriteLog("ImportaOrdini", "***********    START    ***********");

                DateTime dtInizioTimer = DateTime.Now;
                ImportaOrdiniPersonalizzato importaOrdini = new ImportaOrdiniPersonalizzato();
                Zero5.Util.Log.WriteLog("ImportaOrdini", "Inizio Importazione Ordini Produzione.");
                importaOrdini.Esegui();
                TimeSpan duration = new TimeSpan(DateTime.Now.Ticks - dtInizioTimer.Ticks);
                Zero5.Util.Log.WriteLog("ImportaOrdini", "Fine Importazione Ordini Produzione. Elapsed: " + duration.ToString(@"dd\.hh\:mm\:ss"));

                Zero5.Util.Log.WriteLog("ImportaOrdini", "***********    END    ***********");
            }
            catch (Exception ex)
            {
                Zero5.Util.Log.WriteLog("ImportaOrdini", "Errore Generico: " + ex.Message);
            }
        }

        public static void GeneraAllarme(string codiceAllarme, string noteAllarme)
        {
            if (noteAllarme.Length > 2000)
            {
                noteAllarme = noteAllarme.Substring(0, 1999);
            }

            Zero5.Data.Layer.AllarmiSistema allarmeSistema = new Zero5.Data.Layer.AllarmiSistema();
            Zero5.Data.Filter.Filter filtro = new Zero5.Data.Filter.Filter();
            filtro.Add(allarmeSistema.Fields.DataOraAllarme >= DateTime.Now.Date);
            filtro.Add(allarmeSistema.Fields.TipoAllarmeSistema == Zero5.Data.Layer.AllarmiSistema.eAllarmiSistemaTipo.ScambioDati_ErroreDuranteIlProcesso);
            filtro.Add(allarmeSistema.Fields.CodiceAllarme == codiceAllarme);
            filtro.Add(allarmeSistema.Fields.NoteAllarme == noteAllarme);
            allarmeSistema.Load(filtro);

            if (allarmeSistema.EOF)
            {
                allarmeSistema.AddNewAndNewID();
                allarmeSistema.DataOraAllarme = DateTime.Now;
                allarmeSistema.CodiceAllarme = codiceAllarme;
                allarmeSistema.NoteAllarme = noteAllarme;
                allarmeSistema.StatoAllarme = Zero5.Data.Layer.AllarmiSistema.eAllarmiSistemaStato.Inserito;
                allarmeSistema.TipoAllarmeSistema = Zero5.Data.Layer.AllarmiSistema.eAllarmiSistemaTipo.ScambioDati_ErroreDuranteIlProcesso;
                allarmeSistema.Save();
            }
            else
            {
                allarmeSistema.DataOraAllarme = DateTime.Now;
                allarmeSistema.Save();
            }
        }
    }
}

