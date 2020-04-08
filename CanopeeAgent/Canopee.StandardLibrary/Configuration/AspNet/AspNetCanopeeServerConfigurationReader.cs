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
    [Export("Default", typeof(ICanopeeConfigurationReader))]
    public class AspNetCanopeeServerConfigurationReader : ICanopeeConfigurationReader
    {
        private HttpClient _httpClient;
        private static ICanopeeLogger Logger = null;
        private string _url;

        [ImportingConstructor]
        public AspNetCanopeeServerConfigurationReader()
        {
            _httpClient = new HttpClient();
        }

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
                        AgentId = cleanObject.GetProperty<string>("agentId"),
                        Group = cleanObject.GetProperty<string>("group"),
                        Priority = cleanObject.GetProperty<long>("priority")
                    });
                }
            }
            catch (Exception ex)
            {
                Logger?.LogError($"Error while getting groups : {ex}");
            }
            return agentGroups;
        }

        public JsonObject GetConfiguration(string agentId = "Default", string @group = "Default")
        {
            CanopeeConfigurationDto configuration = null;
            try
            {
                
                var uriBuilder = new UriBuilder($"{_url}/api/Configuration")
                {
                    Query=$"agentId={agentId}&group={group}"
                };
                var response = _httpClient.GetAsync(uriBuilder.Uri).Result;
                var resultAsString = response.Content.ReadAsStringAsync().Result;
                var jsonObjects = JsonSerializer.Deserialize<List<JsonObject>>(resultAsString);
                return JsonObject.CleanDocument(jsonObjects.FirstOrDefault()).GetProperty<JsonObject>("configuration");
            }
            catch (Exception ex)
            {
                Logger?.LogError($"Error while getting configuration for agentId:{agentId} group:{group} : {ex}");
            }
            return null;
        }
    }
}