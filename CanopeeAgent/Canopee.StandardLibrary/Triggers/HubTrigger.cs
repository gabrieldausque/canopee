using System;
using System.Composition;
using Canopee.Common;
using Canopee.Common.Events;
using Microsoft.Extensions.Configuration;

namespace Canopee.StandardLibrary.Triggers
{
    [Export("HubTrigger", typeof(ITrigger))]
    [Shared]
    public class HubTrigger : ITrigger
    {
        public void Dispose()
        {
            
        }

        public event EventHandler<TriggerEventArgs> EventTriggered;
        public string ParentName { get; set; }
        public void Initialize(IConfigurationSection triggerParameters)
        {
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        public void RaiseEvent(object sender, TriggerEventArgs triggerArgs)
        {
        }
    }
}