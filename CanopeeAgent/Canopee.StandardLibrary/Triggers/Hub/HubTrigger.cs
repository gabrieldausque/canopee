using System;
using System.Collections.Generic;
using System.Composition;
using Canopee.Common;
using Canopee.Common.Events;
using Microsoft.Extensions.Configuration;

namespace Canopee.StandardLibrary.Triggers.Hub
{
    [Export("HubTrigger", typeof(ITrigger))]
    [Shared]
    public class HubTrigger : ITrigger
    {
        
        private readonly Dictionary<string,EventHandler<TriggerEventArgs>> _eventTriggeredByPipelineId;
        private bool _isStarted;
        public HubTrigger()
        {
            _eventTriggeredByPipelineId = new Dictionary<string, EventHandler<TriggerEventArgs>>();
        }

        public string OwnerName { get; set; }
        
        public string OwnerId { get; set; }

        public void Initialize(IConfigurationSection triggerParameters)
        {
        }

        public void Start()
        {
            _isStarted = true;
        }
        
        public void Stop()
        {
            _isStarted = false;
        }

        public void RaiseEvent(object sender, TriggerEventArgs triggerArgs)
        {
            if (_isStarted)
            {
                if (_eventTriggeredByPipelineId.TryGetValue(triggerArgs.PipelineId, out var toTrigger))
                {
                    toTrigger.Invoke(sender, triggerArgs);
                }        
            }
        }

        public void SubscribeToTrigger(EventHandler<TriggerEventArgs> eventHandler, TriggerSubscriptionContext context)
        {
            if (!_eventTriggeredByPipelineId.ContainsKey(context.PipelineId))
            {
                _eventTriggeredByPipelineId[context.PipelineId] = eventHandler;
            }
            else
            {
                _eventTriggeredByPipelineId.Add(context.PipelineId, eventHandler);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed = false;

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _eventTriggeredByPipelineId.Clear();
                _disposed = true;
            }
        }
    }
}