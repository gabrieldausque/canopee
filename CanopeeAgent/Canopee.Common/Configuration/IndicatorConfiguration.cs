using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Canopee.Common.Configuration
{
    public class IndicatorConfiguration
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Dictionary<string,string> Input { get; set; }
        public Dictionary<string,string> Transform { get; set; }
        public IConfiguration Output { get; set; }
    }
}