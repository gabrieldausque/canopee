using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using CanopeeAgent.Core.Indicators;
using CanopeeAgent.StandardIndicators.Indicators;
using CanopeeAgent.StandardIndicators.Indicators.Hardware;

namespace CanopeeAgent.StandardIndicators.Factories
{
    public class HardwareInfosEventCollectorFactory : FactoryFromDirectoryBase
    {
        private static readonly object LockInstance = new object();
        private static HardwareInfosEventCollectorFactory _instance;
        public static HardwareInfosEventCollectorFactory Instance
        {
            get
            {
                lock (LockInstance)
                {
                    if (_instance == null)
                    {
                        _instance = new HardwareInfosEventCollectorFactory();
                    }
                }

                return _instance;
            }
        }

        public HardwareInfosEventCollectorFactory() : base(@"./Indicators")
        {
            
        }

        public IHardwareInfosEventCollector GetCollectorByPlatform(OSPlatform platform)
        {
            return Container.GetExport<IHardwareInfosEventCollector>(platform.ToString());
        }
    }
}