using System;
using System.Composition;
using Canopee.Common;
using Canopee.Common.Events;
using Canopee.Core.Pipelines;
using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Impl;
using ITrigger = Canopee.Common.ITrigger;

namespace Canopee.StandardLibrary.Triggers.Cron
{
    [Export("Cron", typeof(ITrigger))]
    public class CronTrigger : BaseTrigger
    {
        private IScheduler _scheduler;
        public CronTrigger()
        {
            var schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler().Result;
        }
        
        public override void Initialize(IConfigurationSection triggerParameters)
        {
            var raiseEventTaskId = Guid.NewGuid().ToString();
            RaiseEventJob.SubscribeTo(raiseEventTaskId, RaiseEvent);
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

        public override void Start()
        {
            _scheduler?.Start();
        }

        public override void Stop()
        {
            _scheduler?.Shutdown();
        }

        protected override void InternalDispose()
        {
            _scheduler = null;
        }
    }
}