
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Canopee.Common;
using Canopee.Common.Pipelines.Events;
using Canopee.StandardLibrary.Inputs.Hardware;

namespace Canopee.StandardLibrary.Inputs.Hardware
{
    /// <summary>
    /// Represents all hardware infos of the current workstation/server
    /// </summary>
    public class HardwareInfos : CollectedEvent
    {
        /// <summary>
        /// Default constructor used for serialization/deserialization
        /// </summary>
        public HardwareInfos()
        {
            Disks = new List<DiskInfos>();
            Displays = new List<DisplayInfos>();
            GraphicalCards = new List<GraphicalCardInfos>();
            USBPeripherals = new List<UsbPeripheralInfos>();
        }
        
        /// <summary>
        /// Constructor that set the agent id of this <see cref="ICollectedEvent"/>. Initialize also all collections.
        /// </summary>
        /// <param name="agentId"></param>
        public HardwareInfos(string agentId) :base(agentId)
        {
            Disks = new List<DiskInfos>();
            Displays = new List<DisplayInfos>();
            GraphicalCards = new List<GraphicalCardInfos>();
            USBPeripherals = new List<UsbPeripheralInfos>();
        }
       
        /// <summary>
        /// The cpus architecture. the format depends of the current OS
        /// </summary>
        public string CpuArchitecture { get; set; }
        
        /// <summary>
        /// Number of logical cpus available
        /// </summary>
        public int CpusAvailable { get; set; }
        
        /// <summary>
        /// The cpu model
        /// </summary>
        public string CpuModel { get; set; }
        
        /// <summary>
        /// The size of the memory in human readable unit
        /// </summary>
        public float MemorySize { get; set; }
        
        /// <summary>
        /// The human readable unit
        /// </summary>
        public string MemoryUnit { get; set; }

        /// <summary>
        /// The disks of the current workstation/server, represented by one or more <see cref="DiskInfos"/>
        /// </summary>
        public ICollection<DiskInfos> Disks { get; set; }

        /// <summary>
        /// The displays of the current workstation/server, represented by one or more <see cref="DisplayInfos"/>
        /// </summary>
        public ICollection<DisplayInfos> Displays { get; set; }

        /// <summary>
        /// The graphical cards of the current workstation/server, represented by one or more <see cref="GraphicalCardInfos"/>
        /// </summary>
        public ICollection<GraphicalCardInfos> GraphicalCards { get; set; }
     
        /// <summary>
        /// The usb peripherals of the current workstation/server, represented by one or more <see cref="UsbPeripheralInfos"/>
        /// </summary>
        public ICollection<UsbPeripheralInfos> USBPeripherals { get; set; }

        /// <summary>
        /// Add a <see cref="DiskInfos"/> in the current <see cref="Disks"/> collection
        /// </summary>
        /// <param name="diskInfo"></param>
        public void AddDiskInfos(DiskInfos diskInfo)
        {
            Disks.Add(diskInfo);
        }

        /// <summary>
        /// Add a <see cref="DisplayInfos"/> in the current <see cref="Displays"/> collection
        /// </summary>
        /// <param name="displayInfos"></param>
        public void AddDisplayInfos(DisplayInfos displayInfos)
        {
            Displays.Add(displayInfos);
        }

        /// <summary>
        /// Add a <see cref="GraphicalCardInfos"/> in the current <see cref="GraphicalCards"/> collection
        /// </summary>
        /// <param name="graphicalCardInfos"></param>
        public void AddGraphicalCardInfos(GraphicalCardInfos graphicalCardInfos)
        {
            GraphicalCards.Add(graphicalCardInfos);
        }

        /// <summary>
        /// Add a <see cref="UsbPeripheralInfos"/> in the current <see cref="USBPeripherals"/> collection
        /// </summary>
        /// <param name="usbInfo"></param>
        public void AddUsbPeripherals(UsbPeripheralInfos usbInfo)
        {
            USBPeripherals.Add(usbInfo);
        }
    }
}