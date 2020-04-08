using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Canopee.Common;
using Canopee.Common.Logging;
using Canopee.Common.Pipelines.Events;
using Canopee.Core.Logging;
using Canopee.Core.Configuration;
using Quartz;

namespace Canopee.StandardLibrary.Triggers.Cron
{
    internal class RaiseEventJob : IJob
    {
        private static readonly Dictionary<string, EventHandler<TriggerEventArgs>> _subscriptions;
        
        static RaiseEventJob()
        {
            _subscriptions = new Dictionary<string, EventHandler<TriggerEventArgs>>();
        }

        private ICanopeeLogger Logger = null;
        
        public RaiseEventJob()
        {
            var configuration = ConfigurationService.Instance.GetLoggingConfiguration();
            Logger = CanopeeLoggerFactory.Instance().GetLogger(configuration, this.GetType()); 
        }
        
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                var args = new CronTriggerEventArgs()
                {
                    FireTimeInUtc = context.ScheduledFireTimeUtc.ToString()
                };
                var exists = _subscriptions.TryGetValue(context.JobDetail.Key.Name,out var eventToTrigger);
                try
                {
                    eventToTrigger?.Invoke(this, args);
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Error while invoking a job on cron {context.JobDetail} : {ex}");
                }
            });
        }

        public static void SubscribeTo(string raiseEventTaskId, EventHandler<TriggerEventArgs> raiseEvent)
        {
            _subscriptions.Add(raiseEventTaskId, raiseEvent);
        }
    }
}