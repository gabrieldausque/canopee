using Canopee.Common.Pipelines.Events;

namespace Canopee.StandardLibrary.Inputs.OperatingSystem
{
    /// <summary>
    /// Represent the current OS
    /// </summary>
    public class OperatingSystemInfo : CollectedEvent
    {
        /// <summary>
        /// Default constructor. Needed for serialization/deserialization.
        /// </summary>
        public OperatingSystemInfo()
        {
        }

        /// <summary>
        /// Constructor that set the agent id for the current <see cref="ICollectedEvent"/>
        /// </summary>
        /// <param name="agentId">the agent id</param>
        public OperatingSystemInfo(string agentId):base(agentId)
        {
        }
        
        /// <summary>
        /// Name of the operating system
        /// </summary>
        public string OperatingSystem { get; set; }
        
        /// <summary>
        /// Version of the operating system
        /// </summary>
        public string Version { get; set; }
        
        /// <summary>
        /// Used to indicate if the current OS is 32 or 64 bit
        /// </summary>
        public string Processor { get; set; }
        
        /// <summary>
        /// The hostname of the current workstation/server
        /// </summary>
        public string Hostname { get; set; }
    }
}