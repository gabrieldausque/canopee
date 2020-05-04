using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Canopee.Common;
using System.Text.Json;
using Canopee.Common.Pipelines.Events;

namespace Canopee.StandardLibrary.Inputs.Hardware
{
    /// <summary>
    /// Represents all information for a physical disk 
    /// </summary>
    public class DiskInfos : CollectedEvent
    {
        /// <summary>
        /// Name of the disk
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The size of the disk in human readable unit
        /// </summary>
        public float Size { get; set; }
        
        /// <summary>
        /// The human readable unit 
        /// </summary>
        public string SizeUnit { get; set; }
        
        /// <summary>
        /// The space available in human readable unit
        /// </summary>
        public float SpaceAvailable { get; set; }
        
        /// <summary>
        /// the space available human readable unit
        /// </summary>
        public string SpaceAvailableUnit { get; set; }
        
        /// <summary>
        /// The real size in byte
        /// </summary>
        public float SizeInByte { get; set; }
        
        /// <summary>
        /// The space available in byte
        /// </summary>
        public float SpaceAvailableInByte { get; set; }

        /// <summary>
        /// Default constructor needed for serialization/deserialization
        /// </summary>
        public DiskInfos()
        {
        }

        /// <summary>
        /// The constructor that set the agent id of this <see cref="ICollectedEvent"/>
        /// </summary>
        /// <param name="agentId">the agent id</param>
        public DiskInfos(string agentId) : base(agentId)
        {
        }
    }
}