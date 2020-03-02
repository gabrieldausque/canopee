using Canopee.Common;
using Canopee.Common.Events;
using System.Composition;

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