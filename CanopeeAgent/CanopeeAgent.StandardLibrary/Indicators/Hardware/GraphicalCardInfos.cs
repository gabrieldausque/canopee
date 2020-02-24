using System.ComponentModel.DataAnnotations;
using CanopeeAgent.Common.Events;

namespace CanopeeAgent.StandardIndicators.Indicators.Hardware
{
    public class GraphicalCardInfos : BaseCollectedEvent
    {
        [Key]
        public string GraphicalCardType { get; set; }
        public string GraphicalCardModel { get; set; }

        public GraphicalCardInfos(string agentId) : base(agentId)
        {

        }
    }
}