using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EsportaXML
{
    class FileConfigurazione : Zero5.Util.FileParametri
    {
        public FileConfigurazione() : base(Zero5.IO.Util.LocalPathFile("EsportaXML.cfg"))
        {
            this.IpServer = this.IpServer;
            this.IDUltimoComandoEsportato = this.IDUltimoComandoEsportato;
            this.LastExport = this.LastExport;
        }

        public string PercorsoFileXML
        {
            get
            {
                return GetParametro("PercorsoFileXML", @"\\\\192.168.10.248\\Mago4_Custom\\ConsuntiviMagoXML\\");
            }
            set
            {
                SetParametro("PercorsoFileXML", value);
            }
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

        public int IDUltimoComandoEsportato
        {
            get
            {
                return int.Parse(GetParametro("IDUltimoComandoEsportato", "0"));
            }
            set
            {
                SetParametro("IDUltimoComandoEsportato", value);
            }
        }

        public DateTime LastExport
        {
            get
            {
                DateTime dt = new DateTime(2020, 11, 01, 00, 00, 00);
                if (!DateTime.TryParse(GetParametro("LastExport", dt.ToString()), out dt))
                    dt = DateTime.Now.AddMonths(-3);
                return dt;
            }
            set
            {
                SetParametro("LastExport", value.ToString());
            }
        }

    }
}

