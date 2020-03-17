using System;
using System.Collections.Generic;
using System.Composition;
using System.Reflection;
using System.Text.Json;
using Canopee.Common;
using Canopee.Common.Events;
using Canopee.Core.Pipelines;

namespace Canopee.StandardLibrary.Inputs.Trigger
{
    [Export("FromTrigger", typeof(IInput))]
    public class FromTriggerInput : BaseInput
    {
        public override ICollection<ICollectedEvent> Collect(TriggerEventArgs fromTriggerEventArgs)
        {
            Logger.LogDebug($"Received events from trigger : {fromTriggerEventArgs}");
            try
            {
                var collectedEvents = new List<ICollectedEvent>();
                ICollectedEvent collectedEvent = fromTriggerEventArgs.Raw as ICollectedEvent;
                collectedEvents.Add(collectedEvent);
                return collectedEvents;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error when collecting events from trigger : {ex}");
                throw;
            }
        }
    }
}