using System;
using System.Composition;
using System.IO;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using Canopee.Common;
using Canopee.Common.Pipelines;
using Canopee.Common.Pipelines.Events;
using Canopee.Core.Pipelines;
using Microsoft.Extensions.Configuration;

namespace Canopee.StandardLibrary.Outputs
{
    /// <summary>
    /// Send a collection of <see cref="ICollectedEvent"/> to a CanopeeServer api, the REST exposition of the Canopee pipeline execution engine.
    /// This is useful in following case : the canopeeserver is a passthrough node isolated in a web exposed dmz, the canopee server will enrich   
    /// with new field all events send by agents with internal repositories.
    ///
    /// The configuration will be :
    ///
    /// <example>
    ///
    /// <code>
    ///     {
    ///         ...
    ///         "Canopee": {
    ///             ...
    ///                 "Pipelines": [
    ///                  ...   
    ///                   {
    ///                     "Name": "OS",
    ///                     ...
    ///                     "Outputs" : [{
    ///                         "NoSSLCheck": true,
    ///                         "OutputType": "Canopee",
    ///                         "Url": "https://the-url-of-canopee-server-web-exposed",
    ///                         "PipelineId": "the-pipeline-id"
    ///                    }]
    ///                  ...
    ///                 }
    ///                 ...   
    ///                 ]
    ///             ...
    ///         }
    ///     }
    /// </code>
    /// 
    /// </example>
    ///
    /// the OutputType is "Canopee"   
    /// the Url is the url of the CanopeeServer web exposed
    /// the PipelineId is the id of a pipeline that will treat the <see cref="ICollectedEvent"/> send by the current pipeline
    /// The NoSSLCheck is optional, and used for development purpose. Default value : false.
    /// </summary>
    [Export("Canopee", typeof(IOutput))]
    public class CanopeeOutput : BaseOutput
    {
        /// <summary>
        /// The uri path of the Canopee Web server. 
        /// </summary>
        private static readonly string UriPath = "api/events";
        
        /// <summary>
        /// The url of the Canopee Web server
        /// </summary>
        private string _url;
        
        /// <summary>
        /// The pipeline id that will enrich or modify the <see cref="ICollectedEvent"/>
        /// </summary>
        private string _pipelineId;
        
        /// <summary>
        /// To be used in dev. Suppress the ssl check of the certificate of the agent. TO BE ACTIVATED IN PRODUCTION
        /// </summary>
        private bool _noSslCheck = false;
        
        /// <summary>
        /// Send a <see cref="ICollectedEvent"/> to the specified CanopeeServer
        /// </summary>
        /// <param name="collectedEvent">The collected event to send</param>
        /// <exception cref="HttpRequestException">Raised in case of pb with http communication between client and Canopee Server</exception>
        public override void SendToOutput(ICollectedEvent collectedEvent)
        {
            try
            {
                Logger.LogDebug($"Sending datas to {_url}");
                collectedEvent.SetFieldValue("EventType", collectedEvent.GetType().FullName);
                UriBuilder builder = new UriBuilder(_url);
                builder.Path = UriPath;
                builder.Query = $"pipelineId={_pipelineId}";
                string serializedEvent = JsonSerializer.Serialize(collectedEvent, collectedEvent.GetType(), new JsonSerializerOptions()
                {
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });
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

        /// <summary>
        /// Initialize the <see cref="IOutput"/> with the output configuration and the logging configuration. Create the logger and setup all fields
        /// </summary>
        /// <param name="configurationOutput">the output configuration</param>
        /// <param name="loggingConfiguration">the logging configuration</param>
        public override void Initialize(IConfiguration configurationOutput, IConfigurationSection loggingConfiguration)
        {
            base.Initialize(configurationOutput, loggingConfiguration);
            _url = configurationOutput["Url"];
            _pipelineId = configurationOutput["PipelineId"];
            bool.TryParse(configurationOutput["NoSSLCheck"], out _noSslCheck);
        }
        
        /// <summary>
        /// Create the http client used to communicate with the Canopee server
        /// </summary>
        /// <returns></returns>
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