using System.Collections.Generic;
using CanopeeAgent.Common;
using CanopeeAgent.Core.Indicators;

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

        public ITransform GetTransform(string transformType, Dictionary<string,string> transformConfiguration)
        {
            var transformer = Container.GetExport<ITransform>(transformType);
            transformer?.Initialize(transformConfiguration);
            return transformer;
        }
    }
}