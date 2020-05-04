using System.ComponentModel.DataAnnotations;
using Canopee.Common.Pipelines.Events;

namespace Canopee.StandardLibrary.Inputs.Hardware
{
    public class GraphicalCardInfos : CollectedEvent
    {
        /// <summary>
        /// The default constructor. Needed for serialization/deserialization
        /// </summary>
        public GraphicalCardInfos()
        {
            
        }

        /// <summary>
        /// The type of the graphical card : 3D card or integrated card
        /// </summary>
        public string GraphicalCardType { get; set; }
        
        /// <summary>
        /// The model of the graphical card
        /// </summary>
        public string GraphicalCardModel { get; set; }

        /// <summary>
        /// The constructor that set the agent id of this <see cref="ICollectedEvent"/>
        /// </summary>
        /// <param name="agentId">the agent id</param>
        public GraphicalCardInfos(string agentId) : base(agentId)
        {

        }
    }
}