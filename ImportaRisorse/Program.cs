using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ImportaRisorse
{
    static class Program
    {
        public static FileConfigurazione Parametri = new FileConfigurazione();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                if (args[0] == "-manodopera")
                {
                    Zero5.Util.Log.ProgramName = "ImportaManodopera";
                    if (!Zero5.Threading.SingleInstance.ImAloneWithinSystem("-manodopera"))
                    {
                        Zero5.Util.Log.WriteLog("...double istance, closing.");
                        return;
                    }

                    ImportaManodopera manodopera = new ImportaManodopera();
                    try
                    {
                        Zero5.Data.Link.TCPDataLink.ServerIP = Parametri.IpServer;
                        Zero5.Util.Log.WriteLog("***********    START    ***********");
                        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                        sw.Start();
                        manodopera.Esegui();
                        sw.Stop();
                        Zero5.Util.Log.WriteLog("Fine Importazione Ordini. Elapsed: " + sw.Elapsed.ToString(@"dd\.hh\:mm\:ss"));
                        Zero5.Util.Log.WriteLog("***********    END    ***********");
                    }
                    catch (Exception ex)
                    {
                        Zero5.Util.Log.WriteLog("Errore non gestito: " + ex.Message);
                    }
                }

                if (args[0] == "-macchine")
                {
                    Zero5.Util.Log.ProgramName = "ImportaMacchine";
                    if (!Zero5.Threading.SingleInstance.ImAloneWithinSystem("-macchine"))
                    {
                        Zero5.Util.Log.WriteLog("...double istance, closing.");
                        return;
                    }

                    ImportaMacchine macchine = new ImportaMacchine();
                    try
                    {
                        Zero5.Data.Link.TCPDataLink.ServerIP = Parametri.IpServer;
                        Zero5.Util.Log.WriteLog("***********    START    ***********");
                        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                        sw.Start();
                        macchine.Esegui();
                        sw.Stop();
                        Zero5.Util.Log.WriteLog("Fine Importazione Ordini. Elapsed: " + sw.Elapsed.ToString(@"dd\.hh\:mm\:ss"));
                        Zero5.Util.Log.WriteLog("***********    END    ***********");
                    }
                    catch (Exception ex)
                    {
                        Zero5.Util.Log.WriteLog("Errore non gestito: " + ex.Message);
                    }
                }
            }
        }
    }
}
