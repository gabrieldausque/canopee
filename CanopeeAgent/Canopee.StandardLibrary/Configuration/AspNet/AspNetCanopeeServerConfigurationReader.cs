using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using Canopee.Common;
using Canopee.Common.Configuration;
using Canopee.Common.Configuration.AspNet.Dtos;
using Canopee.Common.Logging;
using Canopee.Core.Configuration;
using Canopee.Core.Logging;
using Microsoft.Extensions.Configuration;

namespace Canopee.StandardLibrary.Configuration.AspNet
{
    /// <summary>
    /// The client of the CanopeeServer instance that will give access to :
    /// - the list of group for a specified agent id
    /// - the configuration for a specified agent id and group
    /// </summary>
    [Export("Default", typeof(ICanopeeConfigurationReader))]
    public class AspNetCanopeeServerConfigurationReader : ICanopeeConfigurationReader
    {
        /// <summary>
        /// The internal http 
        /// </summary>
        private HttpClient _httpClient;
        
        /// <summary>
        /// The internal logger
        /// </summary>
        private static ICanopeeLogger Logger = null;
        
        /// <summary>
        /// The url of the Canopee Server to get information from
        /// </summary>
        private string _url;

        /// <summary>
        /// The default constructor
        /// </summary>
        [ImportingConstructor]
        public AspNetCanopeeServerConfigurationReader()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Initialize the current AspNetCanopeeServer Reader to load configuration from
        /// </summary>
        /// <param name="serviceConfiguration">the configuration section object </param>
        /// <param name="loggingConfiguration">the logger configuration</param>
        public void Initialize(IConfiguration serviceConfiguration, IConfiguration loggingConfiguration)
        {
            Logger = CanopeeLoggerFactory.Instance().GetLogger(loggingConfiguration, this.GetType());
            _url = serviceConfiguration["url"];
            if (bool.Parse(serviceConfiguration["NoSSLCheck"]))
            {
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true;
                _httpClient = new HttpClient(handler);
            }
        }

        /// <summary>
        /// Get one or more <see cref="AgentGroupDto"/> for the corresponding AgentId 
        /// </summary>
        /// <param name="agentId">the agent id to get list of groups</param>
        /// <returns>The list of group associated to the agent id</returns>
        public ICollection<AgentGroupDto> GetGroups(string agentId)
        {
            var agentGroups = new List<AgentGroupDto>(); 
            try
            {
                var uriBuilder =
                    new UriBuilder(
                        $"{_url}/api/AgentGroup")
                    {
                        Query = $"agentId={agentId}"
                    };
                var response = _httpClient.GetAsync(uriBuilder.Uri).Result;
                var resultAsString = response.Content.ReadAsStringAsync().Result;
                var jsonObjects = JsonSerializer.Deserialize<List<JsonObject>>(resultAsString);
                foreach (var jsonObj in jsonObjects)
                {
                    var cleanObject = JsonObject.CleanDocument(jsonObj);
                    agentGroups.Add(new AgentGroupDto()
                    {
                        AgentId = cleanObject.GetProperty<string>("agentId", true),
                        Group = cleanObject.GetProperty<string>("group", true),
                        Priority = cleanObject.GetProperty<long>("priority", true)
                    });
                }
            }
            catch (Exception ex)
            {
                Logger?.LogError($"Error while getting groups : {ex}");
            }
            return agentGroups;
        }

        /// <summary>
        /// Get a <see cref="JsonObject"/> that represent a Canopee Configuration for the specified AgentId and Group 
        /// </summary>
        /// <param name="agentId">
        /// The agent Id
        /// Optional. Default: "Default" 
        /// </param>
        /// <param name="group">
        /// The group
        /// Optional. Default: "Default"
        /// </param>
        /// <returns></returns>
        public JsonObject GetConfiguration(string agentId = "Default", string @group = "Default")
        {
            try
            {
                var uriBuilder = new UriBuilder($"{_url}/api/Configuration")
                {
                    Query=$"agentId={agentId}&group={group}"
                };
                var response = _httpClient.GetAsync(uriBuilder.Uri).Result;
                var resultAsString = response.Content.ReadAsStringAsync().Result;
                var jsonObjects = JsonSerializer.Deserialize<List<JsonObject>>(resultAsString);
                var configAsJsonObject = jsonObjects.FirstOrDefault();
                if(configAsJsonObject != null)
                    return JsonObject.CleanDocument(configAsJsonObject).GetProperty<JsonObject>("configuration", true);
            }
            catch (Exception ex)
            {
                Logger?.LogWarning($"Error while getting configuration : {ex}");
            }
            Logger?.LogWarning($"No configuration for agentId:{agentId} and group:{group}");
            return null;
        }
    }
}