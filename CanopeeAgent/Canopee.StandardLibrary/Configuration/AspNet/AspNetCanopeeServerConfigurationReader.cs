using System;
using System.Collections.Generic;
using System.Composition;
using System.Net.Http;
using System.Text.Json;
using Canopee.Common;
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

        [ImportingConstructor]
        public AspNetCanopeeServerConfigurationReader()
        {
            _httpClient = new HttpClient();
        }

        public void Initialize(IConfiguration serviceConfiguration, IConfiguration loggingConfiguration)
        {
            Logger = CanopeeLoggerFactory.Instance().GetLogger(loggingConfiguration, this.GetType());
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
                var url =
                    $"{ConfigurationService.Instance.GetCanopeeConfiguration()["Configuration:url"]}/api/AgentGroup?agentId={agentId}";
                var response = _httpClient.GetAsync(url).Result;
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

        public JsonObject GetConfiguration(string agentId, string @group)
        {
            throw new System.NotImplementedException();
        }
    }
}