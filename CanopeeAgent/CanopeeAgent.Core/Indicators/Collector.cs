using System;
using System.Collections.Generic;
using CanopeeAgent.Common;
using CanopeeAgent.Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace CanopeeAgent.Core.Indicators
{
    public class Collector : IDisposable
    {
        private List<IIndicator> _indicators;
        private IndicatorFactory _indicatorsFactory;

        public Collector()
        {
            _indicators = new List<IIndicator>();
            _indicatorsFactory = new IndicatorFactory();

            var config = ConfigurationService.Instance
                .Configuration.GetSection("Indicators")
                .Get<List<IndicatorConfiguration>>();
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