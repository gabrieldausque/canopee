using System.Collections.Generic;
using Canopee.Common;
using Canopee.Common.Logging;
using Canopee.Common.Pipelines;
using Canopee.Common.Pipelines.Events;
using Canopee.Core.Configuration;
using Canopee.Core.Logging;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Pipelines
{
    /// <summary>
    /// Base class for the <see cref="IInput"/> contract
    /// </summary>
    public abstract class BaseInput : IInput
    {
       
        /// <summary>
        /// The internal <see cref="ICanopeeLogger"/>
        /// </summary>
        protected ICanopeeLogger Logger;
        
        /// <summary>
        /// The unique Id of the agent (the host)
        /// </summary>
        public string AgentId { get; set; }

        /// <summary>
        /// Collect one or more <see cref="ICollectedEvent"/>
        /// </summary>
        /// <param name="fromTriggerEventArgs">An arg that can be send by the trigger which has started the collect</param>
        /// <returns>A collection of <see cref="ICollectedEvent"/>, even for one event</returns>
        public abstract ICollection<ICollectedEvent> Collect(TriggerEventArgs fromTriggerEventArgs);

        /// <summary>
        /// Initialize the current <see cref="BaseInput"/> from the configuration. Set the agent from the argument
        /// </summary>
        /// <param name="configurationInput">the Input configuration</param>
        /// <param name="loggingConfiguration">the ICanopeeLogger configuration </param>
        /// <param name="agentId">the agentid send from the caller. Prefer Guid format (uuidv4)</param>
        public virtual void Initialize(IConfigurationSection configurationInput, IConfigurationSection loggingConfiguration, string agentId)
        {
            Logger = CanopeeLoggerFactory.Instance().GetLogger(loggingConfiguration, this.GetType());
            AgentId = agentId;
        }
    }
}
