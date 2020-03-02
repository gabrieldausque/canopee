using System;
using Canopee.Common;
using Canopee.Common.Events;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Pipelines
{
    public abstract class BaseTrigger : ITrigger
    {
        private bool _disposed = false;
        
        protected event EventHandler<TriggerEventArgs> EventTriggered;
        
        public string OwnerName { get; set; }
        public string OwnerId { get; set; }
        public abstract void Initialize(IConfigurationSection triggerParameters);
        public abstract void Start();
        public abstract void Stop();

        public virtual void RaiseEvent(object sender, TriggerEventArgs triggerArgs)
        {
            triggerArgs.PipelineId = OwnerId;
            triggerArgs.PipelineName = OwnerName;
            EventTriggered?.Invoke(this, triggerArgs);
        }
        
        public void SubscribeToTrigger(EventHandler<TriggerEventArgs> eventHandler, TriggerSubscriptionContext context)
        {
            OwnerId = context.PipelineId;
            OwnerName = context.PipelineName;
            EventTriggered += eventHandler;
        }

        protected abstract void InternalDispose();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                Stop();
                InternalDispose();
                EventTriggered = null;
                _disposed = true;
            }
        }
    }
}