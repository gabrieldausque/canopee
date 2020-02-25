using CanopeeAgent.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CanopeeAgent.Core.Indicators
{
    public class InputFactory : FactoryFromDirectoryBase
    {
        private static readonly object LockInstance = new object();
        private static InputFactory _instance;

        public static InputFactory Instance
        {
            get
            {
                lock (LockInstance)
                {
                    if (_instance == null)
                    {
                        _instance = new InputFactory();
                    }
                }
                return _instance;
            }
        }

        public InputFactory(string directoryCatalog = @"./Indicators") : base(directoryCatalog)
        {
        }

        public IInput GetInput(IConfiguration configurationInput, string agentId)
        {
            var type = string.IsNullOrWhiteSpace(configurationInput["InputType"]) ? "Default" : configurationInput["InputType"];
            bool.TryParse(configurationInput["OSSpecific"], out var isOsSpecific);
            if(isOsSpecific && type != "Default")
            {
                type = $"{type}{GetCurrentPlatform()}";
            }
            var input = Container.GetExport<IInput>(type);
            input?.Initialize(configurationInput, agentId);
            return input;
        }
    }
}
