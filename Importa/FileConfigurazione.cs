using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Importa
{
    class FileConfigurazione:Zero5.Util.FileParametri
    {
        public FileConfigurazione():base(Zero5.IO.Util.LocalPathFile("ImportaOrdini.cfg"))
        {
            this.IpServer = this.IpServer;
            this.DatabaseGestionale = this.DatabaseGestionale;
        }

        public string IpServer
        {
            get
            {
                return GetParametro("IpServer", "127.0.0.1");
            }
            set
            {
                SetParametro("IpServer", value);
            }
        }

        public string DatabaseGestionale
        {
            get 
            {
                return GetParametro("DatabaseGestionale", "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=PhaseScambioDati;Data Source=NUCONMES\\SQLEXPRESS");
            }
            set
            {
                SetParametro("DatabaseGestionale", value);
            }
        }
    }
}
