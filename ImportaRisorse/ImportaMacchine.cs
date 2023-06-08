using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace ImportaRisorse
{
    class ImportaMacchine
    {
        public void Esegui()
        {
            Zero5.Data.Layer.TP_ANAGCDL vMesMacchine = new Zero5.Data.Layer.TP_ANAGCDL(Program.Parametri.DatabaseGestionale, "SELECT * FROM TP_AnagCDL");

            Zero5.Util.Log.WriteLog("ImportaMacchine", "Righe Caricate " + vMesMacchine.RowCount.ToString());
            if (vMesMacchine.RowCount == 0) return;

            Zero5.Data.Layer.Risorse macchine = new Zero5.Data.Layer.Risorse();
            macchine.Load(macchine.Fields.Tipo == Zero5.Data.Layer.Risorse.eTipo.Macchina);

            Zero5.Data.Layer.Livelli livelli = new Zero5.Data.Layer.Livelli();
            livelli.Load( livelli.Fields.Tipo == Zero5.Data.Layer.Livelli.eTipo.Macchina);

            while (!vMesMacchine.EOF)
            {
                System.Threading.Thread.Sleep(1);
              
                macchine.MoveToNextFieldValue(macchine.Fields.Codice, vMesMacchine.Codicecdl, true);

                //add/update macchine
                if (macchine.EOF)
                {
                    macchine.AddNewAndNewID();
                    macchine.Tipo = Zero5.Data.Layer.Risorse.eTipo.Macchina;
                    macchine.Codice = vMesMacchine.Codicecdl.Trim();
                    macchine.IDLivello = livelli.IDLivello;
                }
                macchine.CodiceEsterno = macchine.Codice;
                macchine.Nome = vMesMacchine.Descricdl;
                macchine.Save();

                vMesMacchine.MoveNext();
            }
             
        }
    }
}