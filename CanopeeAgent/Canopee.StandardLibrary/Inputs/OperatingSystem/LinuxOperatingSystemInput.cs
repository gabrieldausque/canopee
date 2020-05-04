using System.Collections.Generic;
using System.Composition;
using Canopee.Common;
using Canopee.Common.Pipelines;
using Canopee.Common.Pipelines.Events;
using Canopee.Core.Pipelines;
using Canopee.StandardLibrary.Inputs.Batch;

namespace Canopee.StandardLibrary.Inputs.OperatingSystem
{
    /// <summary>
    /// collect one <see cref="OperatingSystemInfo"/> for LINUX OS.
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
    [Export("OperatingSystemLINUX", typeof(IInput))]
    public class LinuxOperatingSystemInput : BatchInput
    {
        /// <summary>
        /// Collect one <see cref="OperatingSystemInfo"/> using uname and hostname command.
        /// </summary>
        /// <param name="fromTriggerEventArgs">the <see cref="TriggerEventArgs"/> send by the <see cref="ITrigger"/></param>
        /// <returns></returns>
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