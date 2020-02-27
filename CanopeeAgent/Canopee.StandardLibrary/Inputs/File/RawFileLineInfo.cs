using Canopee.Common;
using Canopee.Common.Events;
using System.Composition;

namespace Canopee.StandardLibrary.Inputs.File
{
   
    public class RawFileLineInfo : BaseCollectedEvent
    {
        public string Raw { get; set; }
        public RawFileLineInfo(string agentId) : base(agentId)
        {

        }
    }
}