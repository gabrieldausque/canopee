using System;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace CanopeeAgent.Common
{
    public interface IIndicator
    {
        void Initialize(IConfiguration configuration);
        void Collect();
        ITransform Transform { get; set; }
        IOutput Output { get; set; }
        
    }
}