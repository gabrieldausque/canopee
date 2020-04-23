using Canopee.Common;
using Canopee.Common.Pipelines;
using Microsoft.Extensions.Configuration;

namespace Canopee.Core.Pipelines
{
    public class InputFactory : FactoryFromDirectoryBase
    {
        private static readonly object LockInstance = new object();
        private static InputFactory _instance;

        public static InputFactory Instance(string directoryCatalog = @"./Pipelines")
        {
            lock (LockInstance)
            {
                if (_instance == null)
                {
                    _instance = new InputFactory(directoryCatalog);
                }
            }
            return _instance;
        }

        public InputFactory(string directoryCatalog = @"./Pipelines") : base(directoryCatalog)
        {
        }

        public IInput GetInput(IConfigurationSection configurationInput,IConfigurationSection loggingConfiguration, string agentId)
        {
            var type = string.IsNullOrWhiteSpace(configurationInput["InputType"]) ? "Default" : configurationInput["InputType"];
            bool.TryParse(configurationInput["OSSpecific"], out var isOsSpecific);
            if(isOsSpecific && type != "Default")
            {
                type = $"{type}{GetCurrentPlatform().ToString()}";
            }
            var input = Container.GetExport<IInput>(type);
            input?.Initialize(configurationInput,loggingConfiguration, agentId);
            return input;
        }
    }
}
