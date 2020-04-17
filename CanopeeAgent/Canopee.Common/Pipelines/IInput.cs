using System.Collections.Generic;
using Canopee.Common.Pipelines.Events;
using Microsoft.Extensions.Configuration;

namespace Canopee.Common.Pipelines
{
    /// <summary>
    /// The interface of the object that is in charge of collecting one or more event in a <see cref="ICollectPipeline"/> collect process
    /// </summary>
    public interface IInput
    {
        /// <summary>
        /// Collect one or more <see cref="ICollectedEvent"/> from a source
        /// </summary>
        /// <param name="fromTriggerEventArgs">The arg specific to the <see cref="ITrigger"/> that has raised the collect process</param>
        /// <returns></returns>
        ICollection<ICollectedEvent> Collect(TriggerEventArgs fromTriggerEventArgs);

        /// <summary>
        /// Initialize the <see cref="IInput"/> object with its specific configuration
        /// </summary>
        /// <param name="configurationInput">the "Input" configuration</param>
        /// <param name="agentId">the agentId that will be populated in all <see cref="ICollectedEvent"/></param>
        void Initialize(IConfiguration configurationInput, string agentId);

        /// <summary>
        /// The agent Id that will be populated in all <see cref="ICollectedEvent"/>
        /// </summary>
        public string AgentId { get; set; }
    }
}
