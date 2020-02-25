using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CanopeeAgent.Common
{
    public interface IInput
    {
        ICollection<ICollectedEvent> Collect();

        void Initialize(IConfiguration configurationInput, string agentId);

        public string AgentId { get; set; }
    }
}
