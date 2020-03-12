using Canopee.Common.Events;

namespace Canopee.StandardLibrary.Inputs.Network
{
    public class NetworkInfo : CollectedEvent
    {
        public NetworkInfo()
        {
            
        }

        public NetworkInfo(string agentId):base(agentId)
        {
        }
        
        public string NetworkInterfaceName { get; set; }
        public string IpV4 { get; set; }

        public string MacAddress { get; set; }

        public bool IsComplete()
        {
            return (!string.IsNullOrWhiteSpace(NetworkInterfaceName) &&
                    !string.IsNullOrWhiteSpace(IpV4) &&
                    !string.IsNullOrWhiteSpace(MacAddress));
        }
    }
}