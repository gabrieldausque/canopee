
using System.Collections.Generic;
using Canopee.Common.Events;
using Canopee.StandardLibrary.Inputs.Hardware;

namespace Canopee.StandardLibrary.Inputs.Hardware
{
    public class HardwareInfos : CollectedEvent
    {
        public HardwareInfos()
        {
            
        }
        public HardwareInfos(string agentId) :base(agentId)
        {
            Disks = new List<DiskInfos>();
            Displays = new List<DisplayInfos>();
            GraphicalCards = new List<GraphicalCardInfos>();
        }
       
        public string CpuArchitecture { get; set; }
        public int CpusAvailable { get; set; }
        public string CpuModel { get; set; }
        public float MemorySize { get; set; }
        
        public string MemoryUnit { get; set; }

        public ICollection<DiskInfos> Disks { get; set; }

        public ICollection<DisplayInfos> Displays { get; set; }

        public ICollection<GraphicalCardInfos> GraphicalCards { get; set; }
        
        public void AddDiskInfos(DiskInfos diskInfo)
        {
            Disks.Add(diskInfo);
        }

        public void AddDisplayInfos(DisplayInfos displayInfos)
        {
            Displays.Add(displayInfos);
        }

        public void AddGraphicalCardInfos(GraphicalCardInfos graphicalCardInfos)
        {
            GraphicalCards.Add(graphicalCardInfos);
        }
    }
    
}