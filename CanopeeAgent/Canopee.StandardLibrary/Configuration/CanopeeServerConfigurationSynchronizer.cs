using System;
using System.Composition;
using Canopee.Common;
using Canopee.Common.Events;

namespace Canopee.StandardLibrary.Configuration
{
    [Export("CanopeeServer",typeof(IConfigurationSynchronizer))]
    public class CanopeeServerConfigurationSynchronizer : IConfigurationSynchronizer
    {
        public JsonObject GetConfigFromSource()
        {
            //TODO : first get the list of group for agent Id
            //TODO : get the default conf, 
            //TODO : foreach agentgroup get the configuration
            //TODO : get the agent conf
            //TODO : foreach configuration property, override based on default < agentgroup by priority < agent
            //TODO : foreach pipelines, overrides pipeline based on name, following priority order default < agentgroup by priority < agent
            //TODO : return the object
            return null;
        }

        public void Start()
        {
            //TODO : launch a timer using the configuration, load configuration file in JSonObject and compare to the one obtained from the server
            // If a diff, then save the configuration, and raise event (for process to stop and restart)
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<NewConfigurationEventArg> OnNewConfiguration;
    }
}