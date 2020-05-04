using Canopee.Common.Pipelines.Events;

namespace Canopee.StandardLibrary.Inputs.Network
{
    /// <summary>
    /// Represent a Network card 
    /// </summary>
    public class NetworkInfo : CollectedEvent
    {
        /// <summary>
        /// Default constructor. Needed for serialization/deserialization
        /// </summary>
        public NetworkInfo()
        {
            
        }

        /// <summary>
        /// Constructor that set the agent id for the current <see cref="ICollectedEvent"/>
        /// </summary>
        /// <param name="agentId"></param>
        public NetworkInfo(string agentId):base(agentId)
        {
        }
        
        /// <summary>
        /// The network card name
        /// </summary>
        public string NetworkInterfaceName { get; set; }
        
        /// <summary>
        /// The ip v4 of the network card
        /// </summary>
        public string IpV4 { get; set; }

        /// <summary>
        /// The MAC address of the network card
        /// </summary>
        public string MacAddress { get; set; }

        /// <summary>
        /// Indicate all field of this <see cref="NetworkInfo"/> are filled
        /// </summary>
        /// <returns></returns>
        public bool IsComplete()
        {
            return (!string.IsNullOrWhiteSpace(NetworkInterfaceName) &&
                    !string.IsNullOrWhiteSpace(IpV4) &&
                    !string.IsNullOrWhiteSpace(MacAddress));
        }
    }
}