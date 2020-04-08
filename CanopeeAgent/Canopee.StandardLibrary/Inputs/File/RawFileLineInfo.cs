using Canopee.Common;
using System.Composition;
using Canopee.Common.Pipelines.Events;

namespace Canopee.StandardLibrary.Inputs.File
{
   
    public class RawFileLineInfo : CollectedEvent
    {
        public RawFileLineInfo()
        {
            
        }
        public RawFileLineInfo(string agentId) : base(agentId)
        {

        }
    }
}