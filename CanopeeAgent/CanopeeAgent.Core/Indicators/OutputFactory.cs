using System.Collections.Generic;
using CanopeeAgent.Common;
using Microsoft.Extensions.Configuration;

namespace CanopeeAgent.Core.Indicators
{
    public class OutputFactory : FactoryFromDirectoryBase
    {
        private static readonly object LockInstance = new object();
        private static OutputFactory _instance;

        public static OutputFactory Instance  {
            get
            {
                lock (LockInstance)
                {
                    if (_instance == null)
                    {
                        _instance = new OutputFactory();
                    }
                }
                return _instance;
            }
        }

        public OutputFactory(string directoryCatalog = @"./Indicators") : base(directoryCatalog)
        {
        }
        
        public IOutput GetOutput(IConfiguration configurationOutput)
        {
            var type = string.IsNullOrWhiteSpace(configurationOutput["OutputType"]) ? "Default" : configurationOutput["OutputType"];
            var output = Container.GetExport<IOutput>(type);
            output?.Initialize(configurationOutput);
            return output;
        }
    }
}