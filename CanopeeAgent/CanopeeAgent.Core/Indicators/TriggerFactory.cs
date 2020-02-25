using System.Collections.Generic;
using CanopeeAgent.Common;
using Microsoft.Extensions.Configuration;

namespace CanopeeAgent.Core.Indicators
{
    public class TriggerFactory : FactoryFromDirectoryBase
    {
        private static readonly object LockInstance = new object();
        private static TriggerFactory _instance;

        public static TriggerFactory Instance
        {
            get
            {
                lock (LockInstance)
                {
                    if (_instance == null)
                    {
                        _instance = new TriggerFactory();
                    }
                }
                return _instance;
            }
        }

        public TriggerFactory(string directoryCatalog = @"./Indicators") : base(directoryCatalog)
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