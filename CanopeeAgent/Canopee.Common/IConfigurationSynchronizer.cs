using System;
using Canopee.Common.Events;

namespace Canopee.Common
{
    public interface IConfigurationSynchronizer
    {
        JsonObject GetConfigFromSource();

        void Start();

        void Stop();

        event EventHandler<NewConfigurationEventArg> OnNewConfiguration;
    }
}