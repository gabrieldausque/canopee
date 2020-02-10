﻿ using System.Composition;
  using CanopeeAgent.Common;
  using Microsoft.Extensions.Configuration;

  namespace CanopeeAgentStl
{
    [Export("Hardware",typeof(IIndicator))]
    public class HardwareIndicator : IIndicator
    {
        public void Collect()
        {
            throw new System.NotImplementedException();
        }

        public IInput Input { get; set; }
        public ITransform Transform { get; set; }
        public IOutput Output { get; set; }
    }
}