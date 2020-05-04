using Canopee.Common.Pipelines.Events;

namespace Canopee.StandardLibrary.Inputs.Hardware
{
    /// <summary>
    /// This <see cref="ICollectedEvent"/> represents a Display 
    /// </summary>
    public class DisplayInfos : CollectedEvent
    {
        /// <summary>
        /// Default constructor. Needed for serialization/deserialization
        /// </summary>
        public DisplayInfos()
        {
            
        }
        
        /// <summary>
        /// Constructor that set the agent id of this <see cref="ICollectedEvent"/>
        /// </summary>
        /// <param name="agentId">the agent id</param>
        public DisplayInfos(string agentId) : base(agentId)
        {
        }

        /// <summary>
        /// The resolution used for this display (in the form width x height)
        /// </summary>
        public string Resolution { get; set; }
        
        /// <summary>
        /// The name of the display
        /// </summary>
        public string Name { get; set; }
    }
}