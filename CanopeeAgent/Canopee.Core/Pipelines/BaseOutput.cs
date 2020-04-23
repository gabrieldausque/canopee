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
    /// The base class for <see cref="IOutput"/>. Initialize the logger
    /// </summary>
    public abstract class BaseOutput : IOutput
    {
        /// <summary>
        /// The internal <see cref="ICanopeeLogger"/>
        /// </summary>
        protected ICanopeeLogger Logger;

        /// <summary>
        /// Send to output the <see cref="ICollectedEvent"/>
        /// </summary>
        /// <param name="collectedEvent">the collected event to send</param>
        public abstract void SendToOutput(ICollectedEvent collectedEvent);

        /// <summary>
        /// Initialize the output. In base class do nothing
        /// </summary>
        /// <param name="configurationOutput"></param>
        /// <param name="loggingConfiguration">the ICanopeeLogger configuration </param>
        public virtual void Initialize(IConfiguration configurationOutput, IConfigurationSection loggingConfiguration)
        {
            Logger = CanopeeLoggerFactory.Instance().GetLogger(loggingConfiguration, this.GetType());     
        }
    }
}