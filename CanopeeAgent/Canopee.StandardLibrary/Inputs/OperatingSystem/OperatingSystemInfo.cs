using Canopee.Common.Pipelines.Events;

namespace Canopee.StandardLibrary.Inputs.OperatingSystem
{
    public class OperatingSystemInfo : CollectedEvent
    {
        public OperatingSystemInfo()
        {
        }

        public OperatingSystemInfo(string agentId):base(agentId)
        {
        }
        public string OperatingSystem { get; set; }
        public string Version { get; set; }
        public string Processor { get; set; }
        public string Hostname { get; set; }
    }
}