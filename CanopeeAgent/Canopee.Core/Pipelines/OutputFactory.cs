using Canopee.Common;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Pipelines
{
    public class OutputFactory : FactoryFromDirectoryBase
    {
        private static readonly object LockInstance = new object();
        private static OutputFactory _instance;

        public static OutputFactory Instance(string directoryCatalog=@"./Pipelines")  {
            lock (LockInstance)
            {
                if (_instance == null)
                {
                    _instance = new OutputFactory(directoryCatalog);
                }
            }
            return _instance;
        }

        public OutputFactory(string directoryCatalog = @"./Pipelines") : base(directoryCatalog)
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