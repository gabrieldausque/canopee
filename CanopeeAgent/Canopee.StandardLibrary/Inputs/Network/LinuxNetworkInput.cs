using System.Collections.Generic;
using System.Composition;
using System.Text.RegularExpressions;
using Canopee.Common;
using Canopee.Common.Pipelines;
using Canopee.Common.Pipelines.Events;
using Canopee.StandardLibrary.Inputs.Batch;

namespace Canopee.StandardLibrary.Inputs.Network
{
    /// <summary>
    /// Collect all <see cref="NetworkInfo"/> of the current workstation/server for LINUX OS
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
    ///                     "Name": "Products",
    ///                     ...
    ///                     "Input": {
    ///                        "InputType": "Network",
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
    /// the InputType is Network
    /// The OSSpecific argument must be set to true.
    /// 
    /// </summary>
    [Export("NetworkLINUX", typeof(IInput))]
    public class LinuxNetworkInput : BatchInput
    {
        /// <summary>
        /// Default constructor. Set <see cref="BatchInput.CommandLine"/> to ifconfig
        /// </summary>
        public LinuxNetworkInput()
        {
            CommandLine = "ifconfig";
        }

        /// <summary>
        /// Get one or more <see cref="NetworkInfo"/> that represents a network card. extract interface name, ipv4 and mac address. 
        /// </summary>
        /// <param name="fromTriggerEventArgs"></param>
        /// <returns></returns>
        public override ICollection<ICollectedEvent> Collect(TriggerEventArgs fromTriggerEventArgs)
        {
            var collectedEvents = new List<ICollectedEvent>();
            NetworkInfo currentInfo = null;
            var regexInterfaceName = new Regex("^(?<interfaceName>[a-zA-Z0-9]+):.*");
            var regexIpV4Address = new Regex("[ ]+inet[ ]+(?<ipV4>[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}).*");
            var regexMACAddress = new Regex("[ ]+ether[ ]+(?<mac>[a-zA-Z:0-9]+)[ ]+");
            foreach (var line in GetBatchOutput(CommandLine))
            {
                if (regexInterfaceName.IsMatch(line))
                {
                    currentInfo = new NetworkInfo(AgentId)
                    {
                        NetworkInterfaceName = regexInterfaceName.Match(line).Groups["interfaceName"].Value
                    };
                }
                else if (currentInfo != null && regexIpV4Address.IsMatch(line))
                {
                    currentInfo.IpV4 = regexIpV4Address.Match(line).Groups["ipV4"].Value;
                } 
                else if (currentInfo != null && regexMACAddress.IsMatch(line))
                {
                    currentInfo.MacAddress = regexMACAddress.Match(line).Groups["mac"].Value;
                }

                if (currentInfo != null  && currentInfo.IsComplete())
                {
                    collectedEvents.Add(currentInfo);
                    currentInfo = null;
                }
            }
            return collectedEvents;
        }
    }
}