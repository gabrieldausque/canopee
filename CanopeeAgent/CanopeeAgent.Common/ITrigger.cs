using System;
using System.Collections.Generic;
using CanopeeAgent.Common.Events;
using Microsoft.Extensions.Configuration;

namespace CanopeeAgent.Common
{
    public interface ITrigger
    {
        event EventHandler<TriggerEventArgs> EventTriggered;
        void Initialize(IConfigurationSection triggerParameters);
        void Start();
        void Stop();
    }
}