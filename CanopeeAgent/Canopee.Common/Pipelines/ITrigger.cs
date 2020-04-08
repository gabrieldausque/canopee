using System;
using Canopee.Common.Pipelines.Events;
using Microsoft.Extensions.Configuration;

namespace Canopee.Common.Pipelines
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