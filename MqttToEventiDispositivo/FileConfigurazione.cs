using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero5;
using Zero5.Util;
using Zero5.Data;
using Zero5.IO;

namespace MqttToEventiDispositivo
{
    class FileConfigurazione : FileParametri
    {
        public FileConfigurazione(string pathToCFGFile)
            : base(Util.LocalPathFile(pathToCFGFile))
        {
            this.IpServerBroker = this.IpServerBroker;
            this.TopicHeartBeat = this.TopicHeartBeat;
            this.TopicTools = this.TopicTools;
            this.TopicWorks = this.TopicWorks;
            this.TopicUnits = this.TopicUnits;
            this.TopicFixtures = this.TopicFixtures;
        }

        public string IpServerBroker
        {
            get
            {
                return GetParametro("IpServerBroker", "10.2.6.135");
            }
            set
            {
                SetParametro("IpServerBroker", value);
            }
        }

        public string TopicHeartBeat
        {
            get
            {
                return GetParametro("TopicHeartBeat", "JFMX/L1/jl1-00880/heartBeat");
            }
            set
            {
                SetParametro("TopicHeartBeat", value);
            }
        }

        public string TopicTools
        {
            get
            {
                return GetParametro("TopicTools", "JFMX/L1/jl1-00880/tools");
            }
            set
            {
                SetParametro("TopicTools", value);
            }
        }

        public string TopicWorks
        {
            get
            {
                return GetParametro("TopicWorks", "JFMX/L1/jl1-00880/works");
            }
            set
            {
                SetParametro("TopicWorks", value);
            }
        }

        public string TopicUnits
        {
            get
            {
                return GetParametro("TopicUnits", "JFMX/L1/jl1-00880/units");
            }
            set
            {
                SetParametro("TopicUnits", value);
            }
        }

        public string TopicFixtures
        {
            get
            {
                return GetParametro("TopicFixtures", "JFMX/L1/jl1-00880/fixtures");
            }
            set
            {
                SetParametro("TopicFixtures", value);
            }
        }

    }
}
