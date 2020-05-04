using Canopee.Common;
using System.Composition;
using Canopee.Common.Pipelines.Events;

namespace Canopee.StandardLibrary.Inputs.File
{
   /// <summary>
   /// A <see cref="ICollectedEvent"/> that represents a line of a file
   /// </summary>
    public class RawFileLineInfo : CollectedEvent
    {
        /// <summary>
        /// Default constructor. Needed for serialization/deserialization
        /// </summary>
        public RawFileLineInfo()
        {
            
        }
        
        /// <summary>
        /// Set the agent id on creation
        /// </summary>
        /// <param name="agentId">the agent id of the current <see cref="ICollectedEvent"/></param>
        public RawFileLineInfo(string agentId) : base(agentId)
        {

        }
    }
}