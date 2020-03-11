using Canopee.Common;
using Canopee.Common.Events;
using Canopee.StandardLibrary.Inputs.Batch;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Text;
using System.Text.RegularExpressions;

namespace Canopee.StandardLibrary.Inputs.OperatingSystem
{
    [Export("OperatingSystemWINDOWS", typeof(IInput))]
    public class WindowsOperatingSystemInput : BatchInput
    {
        public WindowsOperatingSystemInput()
        {
            CommandLine = "\"wmic OS get Version,Name,OSArchitecture /value\"";
        }

        public override ICollection<ICollectedEvent> Collect(TriggerEventArgs fromTriggerEventArgs)
        {
            var collectedEvents = new List<ICollectedEvent>();
            var osInfos = new OperatingSystemInfo(AgentId);
            foreach(var line in GetBatchOutput(CommandLine))
            {
                if (line.Contains("Name"))
                {
                    osInfos.OperatingSystem = line.Split('=')[1];
                } else if (line.Contains("OSArchitecture"))
                {
                    osInfos.Processor = line.Split('=')[1];
                } else if (line.Contains("Version"))
                {
                    osInfos.Version = line.Split('=')[1];
                }
            }
            osInfos.Hostname = GetBatchOutput("hostname")[0];
            collectedEvents.Add(osInfos);
            return collectedEvents;
        }
    }
}
