using System.Collections.Generic;
using Canopee.Common;
using Canopee.Common.Events;
using Canopee.Core.Configuration;
using Canopee.Core.Logging;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Pipelines
{
    public abstract class BaseInput : IInput
    {
        public BaseInput()
        {
            var configuration = ConfigurationService.Instance.GetLoggingConfiguration();
            Logger = CanopeeLoggerFactory.Instance().GetLogger(configuration, this.GetType()); 
        }
        
        protected ICanopeeLogger Logger;
        public string AgentId { get; set; }

        public abstract ICollection<ICollectedEvent> Collect(TriggerEventArgs fromTriggerEventArgs);

        public virtual void Initialize(IConfiguration configurationInput, string agentId)
        {
            AgentId = agentId;
        }
    }
}
