using Canopee.Common;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Pipelines
{
    public class TransformFactory : FactoryFromDirectoryBase
    {
        private static readonly object LockInstance = new object();
        private static TransformFactory _instance;

        public static TransformFactory Instance(string directoryCatalog=@"./Pipelines")
        {
            lock (LockInstance)
            {
                if (_instance == null)
                {
                    _instance = new TransformFactory(directoryCatalog);
                }
            }
            return _instance;
        }

        public TransformFactory(string directoryCatalog = @"./Pipelines") : base(directoryCatalog)
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