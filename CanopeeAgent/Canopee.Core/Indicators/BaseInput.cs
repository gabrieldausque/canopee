using Canopee.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Canopee.Core.Indicators
{
    public abstract class BaseInput : IInput
    {
        public string AgentId { get; set; }

        public abstract ICollection<ICollectedEvent> Collect();

        public virtual void Initialize(IConfiguration configurationInput, string agentId)
        {
            AgentId = agentId;
        }
    }
}
