using System;
using System.Collections.Generic;
using Canopee.Common.Events;
using Microsoft.Extensions.Configuration;

namespace Canopee.Common
{
    public interface ITrigger : IDisposable
    {
        string OwnerName { get; set; }
        public string OwnerId { get; set; }
        void Initialize(IConfigurationSection triggerParameters);
        void Start();
        void Stop();
        void RaiseEvent(object sender, TriggerEventArgs triggerArgs);
        void SubscribeToTrigger(EventHandler<TriggerEventArgs> eventHandler, TriggerSubscriptionContext context);
    }
}