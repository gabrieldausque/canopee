using System.Collections.Generic;
using Newtonsoft.Json;

namespace CanopeeAgent.Core.Configuration
{
    [JsonObject("Indicator")]
    public class IndicatorConfiguration
    {
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public string Type { get; set; }
        [JsonProperty]
        public Dictionary<string,string> Input { get; set; }
        [JsonProperty]
        public Dictionary<string,string> Transform { get; set; }
        [JsonProperty]
        public Dictionary<string,string> Output { get; set; }
    }
}