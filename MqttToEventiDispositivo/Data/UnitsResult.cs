using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttToEventiDispositivo.Data
{
    class UnitsResult
    {
        [JsonProperty("cat")]
        public string cat { get; set; }

        [JsonProperty("u")]
        public string u { get; set; }

        [JsonProperty("s")]
        public string s { get; set; }

        [JsonProperty("x")]
        public string x { get; set; }

        [JsonProperty("p")]
        public bool p { get; set; }

        [JsonProperty("ms")]
        public string ms { get; set; }

        [JsonProperty("mp")]
        public bool mp { get; set; }

        [JsonProperty("mt")]
        public string mt { get; set; }

        [JsonProperty("mi")]
        public string mi { get; set; }

        [JsonProperty("mu")]
        public string mu { get; set; }

        [JsonProperty("md")]
        public string md { get; set; }

        [JsonProperty("ww")]
        public string ww { get; set; }

        [JsonProperty("wb")]
        public string wb { get; set; }

        [JsonProperty("rt")]
        public string rt { get; set; }

        [JsonProperty("ov")]
        public string ov { get; set; }

        [JsonProperty("ld")]
        public double ld { get; set; }
    }
}
