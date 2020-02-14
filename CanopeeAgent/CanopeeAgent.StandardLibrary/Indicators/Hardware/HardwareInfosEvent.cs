using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CanopeeAgent.Common;
using CanopeeAgent.Common.Events;
using Newtonsoft.Json;

namespace CanopeeAgent.StandardIndicators.Indicators.Hardware
{
    [JsonObject("HardWareInfos")]
    public class HardwareInfosEvent : BaseCollectedEvent
    {
        public HardwareInfosEvent(string agentId)
        {
            EventDate = DateTime.Now;
            AgentId = agentId;
            Disks = new List<DiskInfos>();
            Displays = new List<DisplayInfos>();
        }
        
        public HardwareInfosEvent()
        {
            Disks = new List<DiskInfos>();
        }
        public string CpuArchitecture { get; set; }
        public int CpusAvailable { get; set; }
        public string CpuModel { get; set; }
        public int MemorySize { get; set; }
        
        public string MemoryUnit { get; set; }

        public ICollection<DiskInfos> Disks { get; set; }
        public ICollection<DisplayInfos> Displays { get; set; }

        public void AddDiskInfos(DiskInfos diskInfo)
        {
            Disks.Add(diskInfo);
        }

        public void AddDisplayInfos(DisplayInfos displayInfos)
        {
            Displays.Add(displayInfos);
        }
    }
}