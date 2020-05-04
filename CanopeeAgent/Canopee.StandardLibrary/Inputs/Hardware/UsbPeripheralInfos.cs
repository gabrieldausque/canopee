using Canopee.Common.Pipelines.Events;

namespace Canopee.StandardLibrary.Inputs.Hardware
{
    /// <summary>
    /// Represent a Usb peripheral
    /// </summary>
    public class UsbPeripheralInfos : CollectedEvent
    {
        /// <summary>
        /// Default constructor. Needed for serialization/deserialization
        /// </summary>
        public UsbPeripheralInfos()
        {
            
        }

        /// <summary>
        /// Constructor that set the agent id of the current <see cref="ICollectedEvent"/>
        /// </summary>
        /// <param name="agentId"></param>
        public UsbPeripheralInfos(string agentId):base(agentId)
        {
            
        }

        /// <summary>
        /// The Bus id
        /// </summary>
        public string BusId { get; set; }
        
        /// <summary>
        /// the device number
        /// </summary>
        public string DeviceNumber { get; set; }
        
        /// <summary>
        /// The device id
        /// </summary>
        public string DeviceId { get; set; }
        
        /// <summary>
        /// The device name
        /// </summary>
        public string DeviceName { get; set; }
    }
}