using System;
using System.Collections.Generic;
using System.Composition;
using CanopeeAgent.Common.Events;
using Quartz;
using Quartz.Impl;
using ITrigger = CanopeeAgent.Common.ITrigger;

namespace CanopeeAgent.StandardIndicators.Triggers
{
    [Export("Cron", typeof(ITrigger))]
    public class CronTrigger : ITrigger, IDisposable
    {
        private readonly IScheduler _scheduler;
        public event EventHandler<TriggerEventArgs> EventTriggered;

        public CronTrigger()
        {
            var schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler().Result;
            RaiseEventJob.EventTriggered += (sender, args) =>
            {
                EventTriggered?.Invoke(this, args);
            };
        }
        
        public void Initialize(Dictionary<string, string> triggerParameters)
        {
            var raiseEventTaskId = Guid.NewGuid().ToString();
            var jobDetail = JobBuilder.Create<RaiseEventJob>()
                .WithIdentity(raiseEventTaskId)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithCronSchedule(triggerParameters["When"])
                .StartNow()
                .WithIdentity(raiseEventTaskId)
                .Build();

            _scheduler.ScheduleJob(jobDetail, trigger);
        }

        public void Start()
        {
            _scheduler.Start();
        }

        public void Stop()
        {
            _scheduler.Shutdown();
        }

        private bool IsDisposed { get; set; }
        public void Dispose()
        {
            if (!IsDisposed)
            {
                this.Stop();
                IsDisposed = true;
            }
        }
    }
}