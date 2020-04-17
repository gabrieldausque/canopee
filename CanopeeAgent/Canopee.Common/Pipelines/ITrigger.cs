using System;
using Canopee.Common.Pipelines.Events;
using Microsoft.Extensions.Configuration;

namespace Canopee.Common.Pipelines
{
    /// <summary>
    /// The interface of the object that is in charge of starting the <see cref="ICollectPipeline.Collect"/> process
    /// </summary>
    public interface ITrigger : IDisposable
    {
        /// <summary>
        /// The <see cref="ICollectPipeline.Name"/> that owns this <see cref="ITrigger"/>
        /// </summary>
        string OwnerName { get; set; }
        /// <summary>
        /// The <see cref="ICollectedPipeline.Id" /> that owns this <see cref="ITrigger"/>
        /// </summary>
        public string OwnerId { get; set; }
        /// <summary>
        /// Initialize this <see cref="ITrigger"/> with its "Trigger" configuration
        /// </summary>
        /// <param name="triggerParameters">The "Trigger" configuration </param>
        void Initialize(IConfigurationSection triggerParameters);
        /// <summary>
        /// Start the listening process that will raise the trigger
        /// </summary>
        void Start();
        /// <summary>
        /// Stop the listening process
        /// </summary>
        void Stop();
        /// <summary>
        /// Raise the trigger when needed
        /// </summary>
        /// <param name="sender">The <see cref="ITrigger"/> itself</param>
        /// <param name="triggerArgs">The arg specific to the <see cref="ITrigger"/></param>
        void RaiseEvent(object sender, TriggerEventArgs triggerArgs);
        /// <summary>
        /// Subscribe to the trigger event for a specific handler
        /// </summary>
        /// <param name="eventHandler">The eventHandler that will be executed when needed by the trigger</param>
        /// <param name="context">The context of the subscription, often the name and id of a <see cref="ICollectedPipeline"/></param>
        void SubscribeToTrigger(EventHandler<TriggerEventArgs> eventHandler, TriggerSubscriptionContext context);
    }
}