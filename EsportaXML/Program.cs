using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsportaXML
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
            if (!Zero5.Threading.SingleInstance.ImAloneWithinSystem())
            {
                Zero5.Util.Log.WriteLog("...double istance, closing.");
                return;
            }

            Esporta esp = new Esporta();
            try
            {
                esp.EsportaTransazioni();
            }
            catch (Exception ex)
            {
                Zero5.Util.Log.WriteLog("Errore generico - " + ex.Message);
            }

        }
    }
}
