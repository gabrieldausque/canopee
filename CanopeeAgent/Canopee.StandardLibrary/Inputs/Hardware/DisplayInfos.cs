using Canopee.Common.Events;

namespace Canopee.StandardLibrary.Inputs.Hardware
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