using System.Collections.Generic;
using Canopee.Common.Configuration.AspNet.Dtos;
using Microsoft.Extensions.Configuration;

namespace Canopee.Common.Configuration
{
    /// <summary>
    /// The contract for an object to obtain groups or configurations
    /// </summary>
    public interface ICanopeeConfigurationReader
    {
        /// <summary>
        /// Initialize the reader, using configuration. 
        /// </summary>
        /// <param name="serviceConfiguration">the configuration service configuration</param>
        /// <param name="loggingConfiguration">the logger configuration, needed to initialize the logger of the class</param>
        void Initialize(IConfiguration serviceConfiguration, IConfiguration loggingConfiguration);
        
        /// <summary>
        /// Get all groups for an agent id
        /// </summary>
        /// <param name="agentId">the agent id to obtain the group for</param>
        /// <returns>a collection of <see cref="AgentGroupDto"/></returns>
        ICollection<AgentGroupDto> GetGroups(string agentId);
        
        /// <summary>
        /// Get a configuration for specified agent id and group.
        /// </summary>
        /// <param name="agentId">the agent id</param>
        /// <param name="group">the group</param>
        /// <returns>a <see cref="JsonObject"/> that contains all configurations</returns>
        JsonObject GetConfiguration(string agentId="Default", string group="Default");
    }
}