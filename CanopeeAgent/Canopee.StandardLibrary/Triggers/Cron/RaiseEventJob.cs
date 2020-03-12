using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Canopee.Common.Events;
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
        
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                var args = new CronTriggerEventArgs()
                {
                    FireTimeInUtc = context.ScheduledFireTimeUtc.ToString()
                };
                var exists = _subscriptions.TryGetValue(context.JobDetail.Key.Name,out var eventToTrigger);
                eventToTrigger?.Invoke(this, args);
            });
        }

        public static void SubscribeTo(string raiseEventTaskId, EventHandler<TriggerEventArgs> raiseEvent)
        {
            _subscriptions.Add(raiseEventTaskId, raiseEvent);
        }
    }
}