using Canopee.Common;
using Canopee.Common.Pipelines;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Pipelines
{
    public class TriggerFactory : FactoryFromDirectoryBase
    {
        private static readonly object LockInstance = new object();
        private static TriggerFactory _instance;

        public static TriggerFactory Instance(string directoryCatalog = @"./Pipelines")
        {
                lock (LockInstance)
                {
                    if (_instance == null)
                    {
                        _instance = new TriggerFactory(directoryCatalog);
                    }
                }
                return _instance;
        }

        public TriggerFactory(string directoryCatalog = @"./Pipelines") : base(directoryCatalog)
        {
            
        }

        public ITrigger GetTrigger(IConfigurationSection configurationTrigger)
        {
            var type = string.IsNullOrWhiteSpace(configurationTrigger["TriggerType"]) ? "Default" : configurationTrigger["TriggerType"];
            var trigger = Container.GetExport<ITrigger>(type);
            trigger?.Initialize(configurationTrigger);
            return trigger;
        }
    }
}