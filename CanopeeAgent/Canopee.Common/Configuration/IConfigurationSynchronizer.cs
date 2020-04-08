using System;
using Canopee.Common.Configuration.Events;
using Microsoft.Extensions.Configuration;

namespace Canopee.Common.Configuration
{
    public interface IConfigurationSynchronizer
    {
        JsonObject GetConfigFromSource();

        void Start();

        void Stop();

        event EventHandler<NewConfigurationEventArg> OnNewConfiguration;
        void Initialize(IConfiguration serviceConfiguration, IConfiguration configurationServiceConfiguration);
    }
}