using System.ComponentModel.DataAnnotations;
using Canopee.Common.Events;

namespace Canopee.StandardLibrary.Inputs.Hardware
{
    public class GraphicalCardInfos : CollectedEvent
    {
        public GraphicalCardInfos()
        {
            
        }
        [Key]
        public string GraphicalCardType { get; set; }
        public string GraphicalCardModel { get; set; }

        public GraphicalCardInfos(string agentId) : base(agentId)
        {

        }
    }
}