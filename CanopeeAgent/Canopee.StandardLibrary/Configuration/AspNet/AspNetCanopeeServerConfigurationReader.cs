using System;
using System.Collections.Generic;
using System.Composition;
using System.Net.Http;
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
        }

        public ICollection<AgentGroupDto> GetGroups(string agentId)
        {
            try
            {
                var url =
                    $"{ConfigurationService.Instance.GetCanopeeConfiguration()["Configuration:url"]}?agentId={agentId}";
                var response = _httpClient.GetAsync(url).Result;
            }
            catch (Exception ex)
            {
                Logger?.LogError($"Error while getting groups : {ex}");
            }
            return new List<AgentGroupDto>();
        }

        public JsonObject GetConfiguration(string agentId, string @group)
        {
            throw new System.NotImplementedException();
        }
    }
}