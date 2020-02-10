﻿ using System.Composition;
  using CanopeeAgent.Common;
  using Microsoft.Extensions.Configuration;

  namespace CanopeeAgentStl
{
    [Export("Hardware",typeof(IIndicator))]
    public class HardwareIndicator : IIndicator
    {
        public void Initialize(IConfiguration configuration)
        {
            //TODO : make initialization based on configuration of the agent
        }

        public void Collect()
        {
            throw new System.NotImplementedException();
        }
        
        public ITransform Transform { get; set; }
        
        public IOutput Output { get; set; }
    }
}