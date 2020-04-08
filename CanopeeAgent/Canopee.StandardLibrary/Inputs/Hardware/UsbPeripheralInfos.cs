using Canopee.Common.Pipelines.Events;

namespace Canopee.StandardLibrary.Inputs.Hardware
{
    public class UsbPeripheralInfos : CollectedEvent
    {
        public UsbPeripheralInfos()
        {
            
        }

        public UsbPeripheralInfos(string agentId):base(agentId)
        {
            
        }

        public string BusId { get; set; }
        public string DeviceNumber { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
    }
}