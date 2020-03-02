using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Canopee.Common;
using Canopee.Common.Events;
using System.Text.Json;

namespace Canopee.StandardLibrary.Inputs.Hardware
{
    public class DiskInfos : CollectedEvent
    {
        public DiskInfos()
        {
            
        }
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