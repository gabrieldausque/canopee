using System;
using System.Collections.Generic;
using Canopee.Common.Events;
using Microsoft.Extensions.Configuration;

namespace Canopee.Common
{
    public interface ITrigger
    {
        event EventHandler<TriggerEventArgs> EventTriggered;
        void Initialize(IConfigurationSection triggerParameters);
        void Start();
        void Stop();
    }
}