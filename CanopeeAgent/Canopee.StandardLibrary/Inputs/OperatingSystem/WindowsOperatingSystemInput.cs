using Canopee.Common;
using Canopee.StandardLibrary.Inputs.Batch;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Text;
using System.Text.RegularExpressions;
using Canopee.Common.Pipelines;
using Canopee.Common.Pipelines.Events;

namespace Canopee.StandardLibrary.Inputs.OperatingSystem
{
    /// <summary>
    /// collect one <see cref="OperatingSystemInfo"/> for Windows OS.
    ///
    /// Configuration will be :
    /// 
    /// <example>
    /// <code>
    ///     {
    ///         ...
    ///         "Canopee": {
    ///             ...
    ///                 "Pipelines": [
    ///                  ...   
    ///                   {
    ///                     "Name": "OS",
    ///                     ...
    ///                     "Input": {
    ///                        "InputType": "OperatingSystem",
    ///                        "OSSpecific": true
    ///                     },
    ///                  ...
    ///                 }
    ///                 ...   
    ///                 ]
    ///             ...
    ///         }
    ///     }
    /// </code>
    /// </example>
    ///
    /// the InputType is OperatingSystem
    /// The OSSpecific argument must be set to true.
    /// </summary>
    [Export("OperatingSystemWINDOWS", typeof(IInput))]
    public class WindowsOperatingSystemInput : BatchInput
    {
        /// <summary>
        /// Default constructor. Set the command line to wmic.
        /// </summary>
        public WindowsOperatingSystemInput()
        {
            CommandLine = "\"wmic OS get Version,Name,OSArchitecture /value\"";
        }

        /// <summary>
        /// Collect one <see cref="OperatingSystemInfo"/> using wmic command.
        /// </summary>
        /// <param name="fromTriggerEventArgs">the <see cref="TriggerEventArgs"/> send by the <see cref="ITrigger"/></param>
        /// <returns></returns>
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
