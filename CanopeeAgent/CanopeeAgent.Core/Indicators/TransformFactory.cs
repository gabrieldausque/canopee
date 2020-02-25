using System.Collections.Generic;
using CanopeeAgent.Common;
using CanopeeAgent.Core.Indicators;
using Microsoft.Extensions.Configuration;

namespace CanopeeAgent.StandardIndicators.Indicators.Hardware
{
    public class TransformFactory : FactoryFromDirectoryBase
    {
        private static readonly object LockInstance = new object();
        private static TransformFactory _instance;

        public static TransformFactory Instance
        {
            get
            {
                lock (LockInstance)
                {
                    if (_instance == null)
                    {
                        _instance = new TransformFactory();
                    }
                }
                return _instance;
            }
        }

        public TransformFactory(string directoryCatalog = @"./Indicators") : base(directoryCatalog)
        {
        }

        public ITransform GetTransform(IConfigurationSection configurationTransform)
        {
            var type = string.IsNullOrWhiteSpace(configurationTransform["TransformType"]) ? "Default" : configurationTransform["TransformType"];
            var transformer = Container.GetExport<ITransform>(type);
            transformer?.Initialize(configurationTransform);
            return transformer;
        }
    }
}