using Canopee.Common.Pipelines.Events;
using Microsoft.Extensions.Configuration;

namespace Canopee.Common.Pipelines
{
    /// <summary>
    /// The interface of the object that is in charge of pushing <see cref="ICollectedEvent"/> of a <see cref="ICollectPipeline.Collect"/> process to an external output
    /// </summary>
    public interface IOutput
    {
        /// <summary>
        /// Send a collected event to a specific output
        /// </summary>
        /// <param name="collectedEvent">send a collected event to a specific output</param>
        void SendToOutput(ICollectedEvent collectedEvent);
        /// <summary>
        /// Initialize the <see cref="IOutput"/> with its specific "Output" configuration
        /// </summary>
        /// <param name="configurationOutput">configuration output</param>
        void Initialize(IConfiguration configurationOutput);
    }
}