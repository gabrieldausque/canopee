using System;
using System.Collections.Generic;

namespace CanopeeAgent.Common
{
    public class Collector : IDisposable
    {
        private List<IIndicator> _indicators;
        private IndicatorFactory _indicatorsFactory;

        public Collector()
        {
            _indicators = new List<IIndicator>();
            _indicatorsFactory = new IndicatorFactory();
            
            //TODO : read the configuration section for the indicators and get each from the factory
        }

        public void Run()
        {
            //TODO : start each indicators
        }

        public void Dispose()
        {
            //TODO: dispose each indicators
        }
    }
}