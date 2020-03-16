using Canopee.Common;
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
            var configuration = ConfigurationService.Instance.GetCanopeeConfiguration().GetSection("Logging");
            Logger = CanopeeLoggerFactory.Instance().GetLogger(configuration, this.GetType()); 
        }

        public abstract void SendToOutput(ICollectedEvent collectedEvent);

        public virtual void Initialize(IConfiguration configurationOutput)
        {
            
        }
    }
}