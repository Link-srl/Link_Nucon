using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttToEventiDispositivo.Data
{
    class WorksResult
    {
        [JsonProperty("unit")]
        public string unit { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }

        [JsonProperty("msg")]
        public string msg { get; set; }

        [JsonProperty("rework")]
        public bool rework { get; set; }

        [JsonProperty("fixture")]
        public string fixture { get; set; }

        [JsonProperty("ppf")]
        public string ppf { get; set; }

        [JsonProperty("tk")]
        public string tk { get; set; }

        [JsonProperty("ordNO")]
        public string ordNO { get; set; }

        [JsonProperty("partReference")]
        public string partReference { get; set; }

        [JsonProperty("phase")]
        public string phase { get; set; }

        [JsonProperty("program")]
        public string program { get; set; }

        [JsonProperty("pCodes")]
        public string[] pCodes { get; set; }

    }
}
