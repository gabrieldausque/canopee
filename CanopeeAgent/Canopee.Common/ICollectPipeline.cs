using System;
using System.Collections.Generic;
using System.Diagnostics;
using Canopee.Common.Configuration;
using Canopee.Common.Events;
using Microsoft.Extensions.Configuration;

namespace Canopee.Common
{
    public interface ICollectPipeline : IDisposable
    {
        void Initialize(IConfigurationSection configuration);
        void Collect(TriggerEventArgs fromTriggerArgs);
        void Run();
        void Stop();
        IInput Input { get; set; }
        ICollection<ITransform> Transforms { get; set; }
        IOutput Output { get; set; }
        ITrigger Trigger { get; set; }
    }
}