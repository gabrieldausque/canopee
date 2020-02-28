using System.Collections.Generic;
using Canopee.Common;
using Canopee.Common.Events;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Pipelines
{
    public abstract class BaseInput : IInput
    {
        public string AgentId { get; set; }

        public abstract ICollection<ICollectedEvent> Collect(TriggerEventArgs fromTriggerEventArgs);

        public virtual void Initialize(IConfiguration configurationInput, string agentId)
        {
            AgentId = agentId;
        }
    }
}
