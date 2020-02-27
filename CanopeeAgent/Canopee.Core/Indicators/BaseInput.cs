using Canopee.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Canopee.Common.Events;

namespace Canopee.Core.Indicators
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
