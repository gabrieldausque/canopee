using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CanopeeAgent.Common;
using CanopeeAgent.Common.Events;
using Newtonsoft.Json;

namespace CanopeeAgent.StandardIndicators.Indicators.Hardware
{
    [JsonObject("DiskInfos")]
    public class DiskInfos : BaseCollectedEvent
    {
        [Key]
        public string Name { get; set; }
        public float Size { get; set; }
        
        public string SizeUnit { get; set; }
        public float SpaceAvailable { get; set; }
        
        public string SpaceAvailableUnit { get; set; }

        public DiskInfos(string agentId) : base(agentId)
        {
        }
    }
}