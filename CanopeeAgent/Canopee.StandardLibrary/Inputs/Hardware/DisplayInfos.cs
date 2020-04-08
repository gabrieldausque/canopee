using Canopee.Common.Pipelines.Events;

namespace Canopee.StandardLibrary.Inputs.Hardware
{
    public class DisplayInfos : CollectedEvent
    {
        public DisplayInfos()
        {
            
        }
        public DisplayInfos(string agentId) : base(agentId)
        {
        }

        public string Resolution { get; set; }
        public string Name { get; set; }
    }
}