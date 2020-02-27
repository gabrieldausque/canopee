using System;
using System.Collections.Generic;
using Canopee.Common;
using Canopee.Common.Configuration;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Indicators
{
    public class Collector : IDisposable
    {
        private Dictionary<string, IIndicator> _indicators;
        private IndicatorFactory _indicatorsFactory;

        public Collector()
        {
            _indicators = new Dictionary<string, IIndicator>();
            _indicatorsFactory = new IndicatorFactory();

            var config = ConfigurationService.Instance
                .Configuration.GetSection("Indicators").GetChildren();

            foreach (var indicatorConfig in config)
            {
                var indicator = _indicatorsFactory.GetIndicator(indicatorConfig);
                _indicators.Add(indicatorConfig["Name"], indicator);
            }
        }

        public void Run()
        {
            foreach (var indicator in _indicators)
            {
                indicator.Value.Run();
            }
        }

        public void Dispose()
        {
            foreach (var indicator in _indicators)
            {
                indicator.Value.Dispose();
            }
            _indicators.Clear();
        }
    }
}