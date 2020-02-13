using Newtonsoft.Json;

namespace CanopeeAgent.StandardIndicators.Indicators.Hardware
{
    [JsonObject("DiskInfos")]
    public class DiskInfos
    {
        public string Name { get; set; }
        public string Size { get; set; }
        public string SpaceAvailable { get; set; }
    }
}