using CanopeeAgent.Common.Events;

namespace CanopeeAgent.StandardIndicators.Inputs.Hardware
{
    public class DisplayInfos : BaseCollectedEvent
    {
        public DisplayInfos(string agentId) : base(agentId)
        {
        }

        public string Resolution { get; set; }
        public string Name { get; set; }
    }
}