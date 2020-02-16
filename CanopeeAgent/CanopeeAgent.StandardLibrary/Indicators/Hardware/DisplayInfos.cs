using CanopeeAgent.Common.Events;

namespace CanopeeAgent.StandardIndicators.Indicators.Hardware
{
    public class DisplayInfos : BaseCollectedEvent
    {
        public DisplayInfos(string agentId) : base(agentId)
        {
        }

        public string Resolution { get; set; }
    }
}