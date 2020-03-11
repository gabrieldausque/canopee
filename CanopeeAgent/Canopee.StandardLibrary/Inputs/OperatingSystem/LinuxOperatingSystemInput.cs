using System.Collections.Generic;
using System.Composition;
using Canopee.Common;
using Canopee.Common.Events;
using Canopee.Core.Pipelines;
using Canopee.StandardLibrary.Inputs.Batch;

namespace Canopee.StandardLibrary.Inputs.OperatingSystem
{
    [Export("OperatingSystemLINUX", typeof(IInput))]
    public class LinuxOperatingSystemInput : BatchInput
    {
        public override ICollection<ICollectedEvent> Collect(TriggerEventArgs fromTriggerEventArgs)
        {
            var collectedEvents = new List<ICollectedEvent>();
            var OSinfo = new OperatingSystemInfo(AgentId);
            OSinfo.OperatingSystem = GetBatchOutput("\"uname -o\"")[0];
            OSinfo.Version = GetBatchOutput("\"uname -v\"")[0];
            OSinfo.Processor = GetBatchOutput("\"uname -p\"")[0];
            OSinfo.Hostname = GetBatchOutput("\"hostname\"")[0];
            collectedEvents.Add(OSinfo);
            return collectedEvents;
        }
    }
}