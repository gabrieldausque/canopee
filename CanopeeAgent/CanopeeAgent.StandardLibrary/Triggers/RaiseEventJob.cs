using System;
using System.Threading.Tasks;
using CanopeeAgent.Common.Events;
using Quartz;

namespace CanopeeAgent.StandardIndicators.Triggers
{
    internal class RaiseEventJob : IJob
    {
        public static event EventHandler<TriggerEventArgs> EventTriggered;
        
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                EventTriggered?.Invoke(this, new TriggerEventArgs());
            });
        }
    }
}