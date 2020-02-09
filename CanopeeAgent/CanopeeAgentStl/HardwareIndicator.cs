﻿ using System.Composition;
  using CanopeeAgent.Common;

  namespace CanopeeAgentStl
{
    [Export("Hardware",typeof(IIndicator))]
    public class HardwareIndicator : IIndicator
    {
        public string Collect()
        {
            throw new System.NotImplementedException();
        }
    }
}