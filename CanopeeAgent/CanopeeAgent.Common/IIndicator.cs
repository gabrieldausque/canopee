using System;
using System.Diagnostics;
using CanopeeAgent.Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace CanopeeAgent.Common
{
    public interface IIndicator : IDisposable
    {
        void Initialize(IndicatorConfiguration configuration);
        void Collect();
        void Run();
        void Stop();
        ITransform Transform { get; set; }
        IOutput Output { get; set; }
        ITrigger Trigger { get; set; }
    }
}