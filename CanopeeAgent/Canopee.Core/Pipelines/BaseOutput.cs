using Canopee.Common;
using Canopee.Common.Logging;
using Canopee.Common.Pipelines;
using Canopee.Common.Pipelines.Events;
using Canopee.Core.Configuration;
using Canopee.Core.Logging;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Pipelines
{
    public abstract class BaseOutput : IOutput
    {
        protected ICanopeeLogger Logger;

        public BaseOutput()
        {
            var configuration = ConfigurationService.Instance.GetLoggingConfiguration();
            Logger = CanopeeLoggerFactory.Instance().GetLogger(configuration, this.GetType()); 
        }

        public abstract void SendToOutput(ICollectedEvent collectedEvent);

        public virtual void Initialize(IConfiguration configurationOutput)
        {
            
        }
    }
}