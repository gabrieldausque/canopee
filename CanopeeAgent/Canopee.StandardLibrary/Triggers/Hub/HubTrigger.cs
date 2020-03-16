using System;
using System.Collections.Generic;
using System.Composition;
using Canopee.Common;
using Canopee.Common.Events;
using Canopee.Core.Pipelines;
using Microsoft.Extensions.Configuration;

namespace Canopee.StandardLibrary.Triggers.Hub
{
    [Export("HubTrigger", typeof(ITrigger))]
    [Export("Default", typeof(ITrigger))]
    [Shared]
    public class HubTrigger : BaseTrigger
    {
        
        private readonly Dictionary<string,EventHandler<TriggerEventArgs>> _eventTriggeredByPipelineId;
        private bool _isStarted;
        public HubTrigger()
        {
            _eventTriggeredByPipelineId = new Dictionary<string, EventHandler<TriggerEventArgs>>();
        }

        public string OwnerName { get; set; }
        
        public string OwnerId { get; set; }

        public override void Initialize(IConfigurationSection triggerParameters)
        {
        }

        public override void Start()
        {
            _isStarted = true;
        }
        
        public override void Stop()
        {
            _isStarted = false;
        }

        public override void RaiseEvent(object sender, TriggerEventArgs triggerArgs)
        {
            if (_isStarted)
            {
                if (_eventTriggeredByPipelineId.TryGetValue(triggerArgs.PipelineId, out var toTrigger))
                {
                    toTrigger.Invoke(sender, triggerArgs);
                }        
            }
        }

        public override void SubscribeToTrigger(EventHandler<TriggerEventArgs> eventHandler, TriggerSubscriptionContext context)
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

        protected override void InternalDispose()
        {
            _eventTriggeredByPipelineId.Clear();
        }
    }
}