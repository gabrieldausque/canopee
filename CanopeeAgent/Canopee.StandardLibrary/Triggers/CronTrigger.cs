using System;
using System.Collections.Generic;
using System.Composition;
using Canopee.Common.Events;
using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Impl;
using ITrigger = Canopee.Common.ITrigger;

namespace Canopee.StandardLibrary.Triggers
{
    [Export("Cron", typeof(ITrigger))]
    public class CronTrigger : ITrigger, IDisposable
    {
        private IScheduler _scheduler;
        public event EventHandler<TriggerEventArgs> EventTriggered;
        public string ParentName { get; set; }

        public CronTrigger()
        {
            var schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler().Result;
            RaiseEventJob.EventTriggered += (sender, args) =>
            {
                RaiseEvent(sender, args);
            };
        }
        
        public void Initialize(IConfigurationSection triggerParameters)
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
            _scheduler?.Start();
        }

        public void Stop()
        {
            _scheduler?.Shutdown();
        }

        public void RaiseEvent(object sender, TriggerEventArgs triggerArgs)
        {
            EventTriggered?.Invoke(this, triggerArgs);
        }

        protected bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                this.Stop();
                _scheduler = null;
                EventTriggered = null;
                _disposed = true;
            }
        }
    }
}