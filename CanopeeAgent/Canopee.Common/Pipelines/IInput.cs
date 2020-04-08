using System.Collections.Generic;
using Canopee.Common.Pipelines.Events;
using Microsoft.Extensions.Configuration;

namespace Canopee.Common.Pipelines
{
    public interface IInput
    {
        ICollection<ICollectedEvent> Collect(TriggerEventArgs fromTriggerEventArgs);

        void Initialize(IConfiguration configurationInput, string agentId);

        public string AgentId { get; set; }
    }
}
