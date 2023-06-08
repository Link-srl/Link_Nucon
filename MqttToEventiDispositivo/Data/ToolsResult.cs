using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttToEventiDispositivo.Data
{
    class ToolsResult
    {

        [JsonProperty("cat")]
        public string cat { get; set; }

        [JsonProperty("msg")]
        public string msg { get; set; }

        [JsonProperty("typ")]
        public string typ { get; set; }

        [JsonProperty("sn")]
        public string sn { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }

        [JsonProperty("life")]
        public string life { get; set; }

        [JsonProperty("limit")]
        public string limit { get; set; }

        [JsonProperty("pos")]
        public string pos { get; set; }

        [JsonProperty("unit")]
        public string unit { get; set; }

        [JsonProperty("fx")]
        public string fx { get; set; }

        [JsonProperty("tk")]
        public string tk { get; set; }
    }
}
