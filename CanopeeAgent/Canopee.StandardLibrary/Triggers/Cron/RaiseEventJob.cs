using System;
using System.Threading.Tasks;
using Canopee.Common.Events;
using Quartz;

namespace Canopee.StandardLibrary.Triggers.Cron
{
    internal class RaiseEventJob : IJob
    {
        public static event EventHandler<TriggerEventArgs> EventTriggered;
        
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                var args = new CronTriggerEventArgs()
                {
                    FireTimeInUtc = context.ScheduledFireTimeUtc.ToString()
                };
                EventTriggered?.Invoke(this, args);
            });
        }
    }
}