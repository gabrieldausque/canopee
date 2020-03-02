using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Canopee.Common.Events;

namespace Canopee.Common
{
    public interface IInput
    {
        ICollection<ICollectedEvent> Collect(TriggerEventArgs fromTriggerEventArgs);

        void Initialize(IConfiguration configurationInput, string agentId);

        public string AgentId { get; set; }
    }
}
