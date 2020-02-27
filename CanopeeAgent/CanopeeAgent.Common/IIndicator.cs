using System;
using System.Collections.Generic;
using System.Diagnostics;
using CanopeeAgent.Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace CanopeeAgent.Common
{
    public interface IIndicator : IDisposable
    {
        void Initialize(IConfigurationSection configuration);
        void Collect();
        void Run();
        void Stop();
        ICollection<ITransform> Transforms { get; set; }
        IOutput Output { get; set; }
        ITrigger Trigger { get; set; }
    }
}