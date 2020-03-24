using Canopee.Common;
using Canopee.Core.Logging;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Pipelines
{
    public abstract class BaseTransform : ITransform
    {
        protected ICanopeeLogger Logger;

        public BaseTransform()
        {
            var configuration = Configuration.ConfigurationService.Instance.GetLoggingConfiguration();
            Logger = CanopeeLoggerFactory.Instance().GetLogger(configuration, this.GetType()); 
        }
        public abstract ICollectedEvent Transform(ICollectedEvent input);

        public abstract void Initialize(IConfigurationSection transformConfiguration);
    }
}