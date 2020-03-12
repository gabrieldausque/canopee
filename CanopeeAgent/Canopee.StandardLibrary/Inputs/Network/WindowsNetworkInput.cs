using Canopee.Common;
using Canopee.Common.Events;
using Canopee.Core.Pipelines;
using Canopee.StandardLibrary.Inputs.Batch;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Text;
using System.Text.RegularExpressions;

namespace Canopee.StandardLibrary.Inputs.Network
{
    [Export("NetworkWINDOWS", typeof(IInput))]
    public class WindowsNetworkInput : BatchInput
    {
        public WindowsNetworkInput()
        {
            CommandLine = "\"ipconfig /all\"";
        }

        public override ICollection<ICollectedEvent> Collect(TriggerEventArgs fromTriggerEventArgs)
        {
            var collectedEvents = new List<ICollectedEvent>();
            NetworkInfo networkInfo = null;
            var regexpNetworkCard = new Regex("^(?<networkName>[a-zA-Z]+[ a-zA-Z0-9 ]+):");
            var regexpIPv4 = new Regex("[ ]+IPv4.+:[ ]+(?<ip4Address>[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}).+");
            var regexpMacAddress = new Regex("[ ]+.*:[ ]+(?<macAddress>[0-9A-Za-z]{2}-[0-9A-Za-z]{2}-[0-9A-Za-z]{2}-[0-9A-Za-z]{2}-[0-9A-Za-z]{2}-[0-9A-Za-z]{2})");
            foreach(var line in GetBatchOutput(CommandLine))
            {
                if (regexpNetworkCard.IsMatch(line))
                {
                    networkInfo = new NetworkInfo(AgentId)
                    {
                        NetworkInterfaceName = regexpNetworkCard.Match(line).Groups["networkName"].Value
                    };
                } 
                else if (networkInfo != null && regexpIPv4.IsMatch(line)) {
                    networkInfo.IpV4 = regexpIPv4.Match(line).Groups["ip4Address"].Value;
                } 
                else if (networkInfo != null && regexpMacAddress.IsMatch(line))
                {
                    networkInfo.MacAddress = regexpMacAddress.Match(line).Groups["macAddress"].Value;
                }

                if(networkInfo != null && networkInfo.IsComplete())
                {
                    collectedEvents.Add(networkInfo);
                    networkInfo = null;
                }
            }
            return collectedEvents;
        }
    }
}
