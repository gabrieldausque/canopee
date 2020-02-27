using System;
using System.Collections.Generic;
using System.Diagnostics;
using Canopee.Common.Configuration;
using Microsoft.Extensions.Configuration;

namespace Canopee.Common
{
    public interface IIndicator : IDisposable
    {
        void Initialize(IConfigurationSection configuration);
        void Collect();
        void Run();
        void Stop();
        IInput Input { get; set; }
        ICollection<ITransform> Transforms { get; set; }
        IOutput Output { get; set; }
        ITrigger Trigger { get; set; }
    }
}