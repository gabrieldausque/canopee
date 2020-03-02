using System;
using System.Composition;
using System.Globalization;
using System.Threading;
using Canopee.Common;
using Canopee.Common.Events;
using Canopee.Core.Pipelines;
using Canopee.StandardLibrary.Triggers.Cron;
using Microsoft.Extensions.Configuration;

namespace Canopee.StandardLibrary.Triggers
{
    [Export("Once", typeof(ITrigger))]
    public class OnceTrigger : BaseTrigger
    {
        private Timer _timer;
        private long _dueTimeInMs;
        
        public override void Initialize(IConfigurationSection triggerParameters)
        {
            long.TryParse(triggerParameters["DueTimeInMs"], out _dueTimeInMs);
        }

        public override void Start()
        {
            _timer = new Timer(
                state => { RaiseEvent(this, new TriggerEventArgs(OwnerName, OwnerId)); },
                null,
                _dueTimeInMs,
                Timeout.Infinite
            );
        }

        public override void Stop()
        {
            //Nothing to do, timer is started once
        }
        
        protected override void InternalDispose()
        {
            _timer = null;
        }
    }
}