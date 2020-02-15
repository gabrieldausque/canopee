using System.ComponentModel.DataAnnotations;
using CanopeeAgent.Common.Events;

namespace CanopeeAgent.StandardIndicators.Indicators.Hardware
{
    public class DisplayInfos : BaseCollectedEvent
    {
        [Key]
        public string DisplayType { get; set; }
        public string DisplayModel { get; set; }

        public DisplayInfos(string agentId) : base(agentId)
        {
        }
    }
}