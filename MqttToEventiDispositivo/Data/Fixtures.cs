using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttToEventiDispositivo.Data
{
    class Fixtures
    {
        [JsonProperty("cat")]
        public string cat { get; set; }

        [JsonProperty("msg")]
        public string msg { get; set; }

        [JsonProperty("typ")]
        public string typ { get; set; }

        [JsonProperty("sn")]
        public string sn { get; set; }

        [JsonProperty("unit")]
        public string unit { get; set; }

        [JsonProperty("pos")]
        public string pos { get; set; }

        [JsonProperty("tk")]
        public string tk { get; set; }

        [JsonProperty("ppf")]
        public int ppf { get; set; }

        [JsonProperty("exec")]
        public int exec { get; set; }

        [JsonProperty("placings")]
        public int placings { get; set; }

        [JsonProperty("rework")]
        public bool rework { get; set; }

        [JsonProperty("pcodes")]
        public string[] pcodes { get; set; }

        [JsonProperty("parts")]
        public string[] parts { get; set; }

        [JsonProperty("fxt")]
        public string fxt { get; set; }

        [JsonProperty("user")]
        public string user { get; set; }

        [JsonProperty("wa")]
        public string wa { get; set; }
    }
}
