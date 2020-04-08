using System.Collections.Generic;
using Canopee.Common.Configuration.AspNet.Dtos;
using Microsoft.Extensions.Configuration;

namespace Canopee.Common.Configuration
{
    public interface ICanopeeConfigurationReader
    {
        void Initialize(IConfiguration serviceConfiguration, IConfiguration loggingConfiguration);
        
        ICollection<AgentGroupDto> GetGroups(string agentId);
        
        JsonObject GetConfiguration(string agentId="Default", string group="Default");
    }
}