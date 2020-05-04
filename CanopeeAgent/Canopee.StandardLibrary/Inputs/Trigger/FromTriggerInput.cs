using System;
using System.Collections.Generic;
using System.Composition;
using System.Reflection;
using System.Text.Json;
using Canopee.Common;
using Canopee.Common.Pipelines;
using Canopee.Common.Pipelines.Events;
using Canopee.Core.Pipelines;

namespace Canopee.StandardLibrary.Inputs.Trigger
{
    /// <summary>
    /// This <see cref="IInput"/> is used to take the <see cref="ICollectedEvent"/> from the <see cref="TriggerEventArgs.Raw"/> send by the <see cref="ITrigger"/> that raise the executing <see cref="CollectPipeline"/>
    ///
    ///    /// Configuration will be 
    /// 
    /// <example>
    /// <code>
    ///     {
    ///         ...
    ///         "Canopee": {
    ///             ...
    ///                 "Pipelines": [
    ///                  ...   
    ///                   {
    ///                     "Name": "OS",
    ///                     ...
    ///                     "Input": {
    ///                        "InputType": "FromTrigger"
    ///                     },
    ///                  ...
    ///                 }
    ///                 ...   
    ///                 ]
    ///             ...
    ///         }
    ///     }
    /// </code>
    /// </example>
    ///
    /// the InputType is FromTrigger
    /// 
    /// </summary>
    [Export("FromTrigger", typeof(IInput))]
    public class FromTriggerInput : BaseInput
    {
        /// <summary>
        /// Extract the <see cref="ICollectedEvent"/> from the <see cref="TriggerEventArgs.Raw"/> received
        /// </summary>
        /// <param name="fromTriggerEventArgs">The <see cref="TriggerEventArgs"/> that contains the collected event</param>
        /// <returns></returns>
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