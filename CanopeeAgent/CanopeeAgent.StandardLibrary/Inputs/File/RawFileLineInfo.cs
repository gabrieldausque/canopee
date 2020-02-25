using CanopeeAgent.Common;
using CanopeeAgent.Common.Events;
using System.Composition;

namespace CanopeeAgent.StandardIndicators.Inputs.File
{
   
    public class RawFileLineInfo : BaseCollectedEvent
    {
        public string Raw { get; set; }
        public RawFileLineInfo(string agentId) : base(agentId)
        {

        }
    }
}