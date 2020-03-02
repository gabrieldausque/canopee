using System.Collections.Generic;
using System.Composition;
using System.Text.Json;
using Canopee.Common;
using Canopee.Common.Events;
using Canopee.Core.Pipelines;
using Newtonsoft.Json.Linq;

namespace Canopee.StandardLibrary.Inputs.Trigger
{
    [Export("FromTrigger", typeof(IInput))]
    public class FromTriggerInput : BaseInput
    {
        public override ICollection<ICollectedEvent> Collect(TriggerEventArgs fromTriggerEventArgs)
        {
            var collectedEvents = new List<ICollectedEvent>();
            CollectedEvent collectedEvent = fromTriggerEventArgs.Raw as CollectedEvent;
            return collectedEvents;
        }
    }
}