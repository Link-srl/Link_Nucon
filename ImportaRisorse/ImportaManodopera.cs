using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace ImportaRisorse
{
    class ImportaManodopera
    {
        public void Esegui()
        {
            Zero5.Data.Layer.TP_ANAGADDETTI vMesDipendenti = new Zero5.Data.Layer.TP_ANAGADDETTI(Program.Parametri.DatabaseGestionale, "SELECT * FROM TP_AnagAddetti");

            Zero5.Util.Log.WriteLog("ImportaManodopera", "Righe Caricate " + vMesDipendenti.RowCount.ToString());
            if (vMesDipendenti.RowCount == 0) return;

            Zero5.Data.Layer.Risorse manodopera = new Zero5.Data.Layer.Risorse();
            manodopera.Load(manodopera.Fields.Tipo == Zero5.Data.Layer.Risorse.eTipo.Uomo);

            Zero5.Data.Layer.Livelli livelli = new Zero5.Data.Layer.Livelli();
            livelli.Load(livelli.Fields.Tipo == Zero5.Data.Layer.Livelli.eTipo.Uomo);

            while (!vMesDipendenti.EOF)
            {
                System.Threading.Thread.Sleep(1);

                manodopera.MoveToNextFieldValue(manodopera.Fields.Codice, vMesDipendenti.Idaddetto.ToString(), true);

                if (manodopera.EOF)
                {
                    manodopera.AddNewAndNewID();
                    manodopera.Codice = vMesDipendenti.Idaddetto.ToString();
                }

                if (vMesDipendenti.NMatricola.Trim().Length > 0)
                    manodopera.Badge = vMesDipendenti.NMatricola;
                else
                    manodopera.Badge = vMesDipendenti.Idaddetto.ToString();

                manodopera.Nome = manodopera.Badge;
                manodopera.Tipo = Zero5.Data.Layer.Risorse.eTipo.Uomo;
                manodopera.IDLivello = livelli.IDLivello;
                manodopera.Descrizione = (vMesDipendenti.Tipo.Trim().ToUpper().Contains("IND")) ? "-INDIRETTO-" : "-DIRETTO-";
                manodopera.Descrizione += vMesDipendenti.Descriaddetto;
                manodopera.Save();

                vMesDipendenti.MoveNext();  
            }
        }
    }
}