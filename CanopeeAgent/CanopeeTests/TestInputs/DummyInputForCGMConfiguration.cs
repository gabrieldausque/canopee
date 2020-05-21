using System.Collections.Generic;
using System.Composition;
using System.Linq;
using Canopee.Common.Pipelines.Events;
using Canopee.Core.Pipelines;
using Microsoft.Extensions.Configuration;
using Nest;
using IInput = Canopee.Common.Pipelines.IInput;

namespace CanopeeTests.TestInputs
{
    [Export("CGMConfigTest", typeof(IInput))]
    public class DummyInputForCGMConfiguration : BaseInput
    {
        private IConfigurationSection _myConfiguration;
        public override void Initialize(IConfigurationSection configurationInput, IConfigurationSection loggingConfiguration, string agentId)
        {
            base.Initialize(configurationInput, loggingConfiguration, agentId);
            _myConfiguration = configurationInput;
        }

        public override ICollection<ICollectedEvent> Collect(TriggerEventArgs fromTriggerEventArgs)
        {
            var propertiesFromConfig = new CollectedEvent();
            propertiesFromConfig.SetFieldValue("MappingConfiguration", _myConfiguration.GetSection("Mapping"));
            propertiesFromConfig.SetFieldValue("MappingProperties", _myConfiguration.GetSection("Mapping:Properties"));
            propertiesFromConfig.SetFieldValue("FirstProperty", _myConfiguration.GetSection("Mapping:Properties").GetChildren().FirstOrDefault());
            return new List<ICollectedEvent>() { propertiesFromConfig };
        }
    }
}