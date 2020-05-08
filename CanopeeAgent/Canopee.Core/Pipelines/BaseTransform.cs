using Canopee.Common;
using Canopee.Common.Logging;
using Canopee.Common.Pipelines;
using Canopee.Common.Pipelines.Events;
using Canopee.Core.Logging;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Pipelines
{
    /// <summary>
    /// The base implementation for <see cref="ITransform"/> contract
    /// </summary>
    public abstract class BaseTransform : ITransform
    {
        /// <summary>
        /// The internal <see cref="ICanopeeLogger"/>
        /// </summary>
        protected ICanopeeLogger Logger;

        /// <summary>
        /// Transform a <see cref="ICollectedEvent"/> : add new field, transform existing field, change type of the collected event
        /// </summary>
        /// <param name="collectedEventToTransform">the <see cref="ICollectedEvent"/> to transform</param>
        /// <returns></returns>
        public abstract ICollectedEvent Transform(ICollectedEvent collectedEventToTransform);

        /// <summary>
        /// Initialize using the transform configuration
        /// </summary>
        /// <param name="transformConfiguration">the Transform configuration section for the current <see cref="ITransform"/></param>
        /// <param name="loggingConfiguration">the ICanopeeLogger configuration </param>
        public virtual void Initialize(IConfigurationSection transformConfiguration,
            IConfigurationSection loggingConfiguration)
        {
            Logger = CanopeeLoggerFactory.Instance().GetLogger(loggingConfiguration, this.GetType());            
        }
    }
}