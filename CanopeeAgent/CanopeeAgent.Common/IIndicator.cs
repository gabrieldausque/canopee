using System;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace CanopeeAgent.Common
{
    public interface IIndicator
    {
        void Collect();

        IInput Input { get; set; }
        ITransform Transform { get; set; }
        IOutput Output { get; set; }
        
    }
}