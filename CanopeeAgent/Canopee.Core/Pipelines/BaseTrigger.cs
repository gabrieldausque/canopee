using System;
using Canopee.Common;
using Canopee.Common.Logging;
using Canopee.Common.Pipelines;
using Canopee.Common.Pipelines.Events;
using Canopee.Core.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Canopee.Core.Pipelines
{
    /// <summary>
    /// The base implementation of <see cref="ITrigger"/>
    /// </summary>
    public abstract class BaseTrigger : ITrigger
    {
        /// <summary>
        /// The internal <see cref="ICanopeeLogger"/>
        /// </summary>
        protected ICanopeeLogger Logger;
        
        /// <summary>
        /// Event raise when the trigger check that a collect needs to be done
        /// </summary>
        protected event EventHandler<TriggerEventArgs> EventTriggered;
        
        /// <summary>
        /// The name of the owner object (a <see cref="CollectPipeline"/>)
        /// </summary>
        public string OwnerName { get; set; }
        
        /// <summary>
        /// The id of the owner object ( a <see cref="CollectPipeline"/>)
        /// </summary>
        public string OwnerId { get; set; }

        /// <summary>
        /// Initialize the trigger with the Trigger configuration and its logger
        /// </summary>
        /// <param name="triggerConfiguration">the trigger configuration</param>
        /// <param name="loggingConfiguration">the logger configuration</param>
        public virtual void Initialize(IConfigurationSection triggerConfiguration,
            IConfigurationSection loggingConfiguration)
        {
            Logger = CanopeeLoggerFactory.Instance().GetLogger(loggingConfiguration, this.GetType());
        }
        
        /// <summary>
        /// Start the watch of the trigger
        /// </summary>
        public abstract void Start();
        
        /// <summary>
        /// Stop the watch of the trigger
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Raise the <see cref="BaseTrigger.EventTriggered"/>
        /// </summary>
        /// <param name="sender">the object that raise the event</param>
        /// <param name="triggerArgs">args specific to implementation of the <see cref="BaseTrigger"/></param>
        public virtual void RaiseEvent(object sender, TriggerEventArgs triggerArgs)
        {
            triggerArgs.PipelineId = OwnerId;
            triggerArgs.PipelineName = OwnerName;
            EventTriggered?.Invoke(this, triggerArgs);
        }
        
        /// <summary>
        /// Subscribe to the Event 
        /// </summary>
        /// <param name="eventHandler">The <see cref="EventHandler{TEventArgs}"/> to execute when trigger is raised</param>
        /// <param name="context">The context object of the subscription. Will contain at least owner id and name</param>
        public virtual void SubscribeToTrigger(EventHandler<TriggerEventArgs> eventHandler, TriggerSubscriptionContext context)
        {
            OwnerId = context.PipelineId;
            OwnerName = context.PipelineName;
            EventTriggered += eventHandler;
        }

        /// <summary>
        /// Internal disposed flag
        /// </summary>
        private bool _disposed = false;
        
        /// <summary>
        /// Will dispose all needed internal object of the current implementation
        /// </summary>
        protected abstract void InternalDispose();

        /// <summary>
        /// Dispose the current <see cref="ITrigger"/>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the current <see cref="ITrigger"/>
        /// </summary>
        /// <param name="disposing"></param>
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