using System;
using System.Threading.Tasks;
using Canopee.Common.Events;
using Quartz;

namespace Canopee.StandardLibrary.Triggers
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