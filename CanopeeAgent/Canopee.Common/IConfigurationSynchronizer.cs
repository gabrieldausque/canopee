using System;
using Canopee.Common.Events;
using Microsoft.Extensions.Configuration;

namespace Canopee.Common
{
    public interface IConfigurationSynchronizer
    {
        JsonObject GetConfigFromSource();

        void Start();

        void Stop();

        event EventHandler<NewConfigurationEventArg> OnNewConfiguration;
        void Initialize(IConfiguration configurationServiceConfiguration);
    }
}