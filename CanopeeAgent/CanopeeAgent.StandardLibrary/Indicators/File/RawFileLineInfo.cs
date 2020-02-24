using CanopeeAgent.Common.Events;

namespace CanopeeAgent.StandardIndicators.Indicators.File
{
    public class RawFileLineInfo : BaseCollectedEvent
    {
        public string Raw { get; set; }
        public RawFileLineInfo(string agentId) : base(agentId)
        {

        }
    }
}