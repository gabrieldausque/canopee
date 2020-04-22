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
        /// The default constructor. Create the <see cref="ICanopeeLogger"/>
        /// </summary>
        public BaseTransform()
        {
            var configuration = Configuration.ConfigurationService.Instance.GetLoggingConfiguration();
            Logger = CanopeeLoggerFactory.Instance().GetLogger(configuration, this.GetType()); 
        }
        
        /// <summary>
        /// Transform a <see cref="ICollectedEvent"/> : add new field, transform existing field, change type of the collected event
        /// </summary>
        /// <param name="input">the <see cref="ICollectedEvent"/> to transform</param>
        /// <returns></returns>
        public abstract ICollectedEvent Transform(ICollectedEvent input);

        /// <summary>
        /// Initialize using the transform configuration
        /// </summary>
        /// <param name="transformConfiguration">the Transform configuration section for the current <see cref="ITransform"/></param>
        public abstract void Initialize(IConfigurationSection transformConfiguration);
    }
}