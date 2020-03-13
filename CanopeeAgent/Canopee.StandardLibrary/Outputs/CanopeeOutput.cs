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
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;

namespace Canopee.StandardLibrary.Outputs
{
    [Export("Canopee", typeof(IOutput))]
    public class CanopeeOutput : IOutput
    {
        
        private static readonly string UriPath = "api/events";
        private string _url;
        private string _pipelineId;
        private bool _noSSLCheck;
        
        public void SendToOutput(ICollectedEvent collectedEvent)
        {
            collectedEvent.SetFieldValue("EventType", collectedEvent.GetType().FullName);
            UriBuilder builder = new UriBuilder(_url);
            builder.Path = UriPath;
            builder.Query = $"pipelineId={_pipelineId}";
            string serializedEvent = JsonSerializer.Serialize(collectedEvent, collectedEvent.GetType());
            var test = JsonSerializer.Deserialize<CollectedEvent>(serializedEvent);
            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    var response = client.PostAsync(builder.Uri, new StringContent(serializedEvent,  Encoding.UTF8, "application/json")).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException(response.ToString());
                    }
                }
                catch (Exception e)
                {
                    
                    Console.WriteLine(e.ToString());
                    //TODO : log error
                }
            }
        }

        public void Initialize(IConfiguration configurationOutput)
        {
            _url = configurationOutput["Url"];
            _pipelineId = configurationOutput["PipelineId"];
            bool.TryParse(configurationOutput["NoSSLCheck"], out _noSSLCheck);
        }
        
        private HttpClient GetHttpClient()
        {
            if (_noSSLCheck)
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