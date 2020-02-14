using Newtonsoft.Json;

namespace CanopeeAgent.StandardIndicators.Indicators.Hardware
{
    [JsonObject("DiskInfos")]
    public class DiskInfos
    {
        public string Name { get; set; }
        public int Size { get; set; }
        
        public string SizeUnit { get; set; }
        public int SpaceAvailable { get; set; }
        
        public string SpaceAvailableUnit { get; set; }
    }
}