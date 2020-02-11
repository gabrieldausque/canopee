using System;
using System.Collections.Generic;
using CanopeeAgent.Common.Events;

namespace CanopeeAgent.Common
{
    public interface ITrigger
    {
        event EventHandler<TriggerEventArgs> EventTriggered;
        void Initialize(Dictionary<string,string> triggerParameters);
        void Start();
        void Stop();
    }
}