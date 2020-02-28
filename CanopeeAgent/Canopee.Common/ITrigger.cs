using System;
using System.Collections.Generic;
using Canopee.Common.Events;
using Microsoft.Extensions.Configuration;

namespace Canopee.Common
{
    public interface ITrigger : IDisposable
    {
        event EventHandler<TriggerEventArgs> EventTriggered;
        string ParentName { get; set; }
        void Initialize(IConfigurationSection triggerParameters);
        void Start();
        void Stop();
        void RaiseEvent(object sender, TriggerEventArgs triggerArgs);
    }
}