using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportaRisorse
{
    class FileConfigurazione:Zero5.Util.FileParametri
    {
        public FileConfigurazione():base(Zero5.IO.Util.LocalPathFile("ImportaRisorse.cfg"))
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
                return GetParametro("DatabaseGestionale", "");
            }
            set
            {
                SetParametro("DatabaseGestionale", value);
            }
        }
    }
}
