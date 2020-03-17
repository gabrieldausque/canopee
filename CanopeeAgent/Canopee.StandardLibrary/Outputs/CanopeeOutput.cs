using System;
using System.Composition;
using System.IO;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using Canopee.Common;
using Canopee.Common.Events;
using Canopee.Common.Hosting.Web;
using Canopee.Core.Pipelines;
using Microsoft.Extensions.Configuration;

namespace Canopee.StandardLibrary.Outputs
{
    [Export("Canopee", typeof(IOutput))]
    public class CanopeeOutput : BaseOutput
    {
        
        private static readonly string UriPath = "api/events";
        private string _url;
        private string _pipelineId;
        private bool _noSslCheck = false;
        
        public override void SendToOutput(ICollectedEvent collectedEvent)
        {
            try
            {
                Logger.LogDebug($"Sending datas to {_url}");
                collectedEvent.SetFieldValue("EventType", collectedEvent.GetType().FullName);
                UriBuilder builder = new UriBuilder(_url);
                builder.Path = UriPath;
                builder.Query = $"pipelineId={_pipelineId}";
                string serializedEvent = JsonSerializer.Serialize(collectedEvent, collectedEvent.GetType());
                var test = JsonSerializer.Deserialize<CollectedEvent>(serializedEvent);
                using (HttpClient client = GetHttpClient())
                {
                    var response = client.PostAsync(builder.Uri, new StringContent(serializedEvent,  Encoding.UTF8, "application/json")).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException(response.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error while sending to Canopee Service : {ex}");
                throw;
            }
        }

        public override void Initialize(IConfiguration configurationOutput)
        {
            _url = configurationOutput["Url"];
            _pipelineId = configurationOutput["PipelineId"];
            bool.TryParse(configurationOutput["NoSSLCheck"], out _noSslCheck);
        }
        
        private HttpClient GetHttpClient()
        {
            if (_noSslCheck)
            {
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true;
                return new HttpClient(handler); 
            }
            return new HttpClient();
        }
    }
}