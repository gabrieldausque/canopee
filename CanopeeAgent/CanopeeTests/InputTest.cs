using System;
using System.Linq;
using Canopee.Common.Logging;
using Canopee.Core.Configuration;
using Canopee.Core.Logging;
using Canopee.Core.Pipelines;
using CanopeeTests.TestInputs;
using Microsoft.Extensions.Configuration;
using Nest;
using NUnit.Framework;

namespace CanopeeTests
{
    public class Tests
    {
        private ICanopeeLogger _logger = null;
        
        [SetUp]
        public void Setup()
        {
            CanopeeLoggerFactory.SetGlobalInstance(new CanopeeLoggerFactory("./"));
            _logger = CanopeeLoggerFactory.Instance()
                .GetLogger(ConfigurationService.Instance.GetLoggingConfiguration(), typeof(Tests));
            InputFactory.SetGlobalInstance(new InputFactory("./"));
            TransformFactory.SetGlobalInstance(new TransformFactory("./"));
            OutputFactory.SetGlobalInstance(new OutputFactory("./"));
        }

        [Test]
        public void Should_create_correctly_Dummy_Input_With_Configuration()
        {
            IConfigurationSection cgmPipelineTest = ConfigurationService.Instance.GetPipelinesConfiguration().GetChildren().FirstOrDefault((p) => p["Name"] == "TestCGMConfig");
            Assert.IsNotNull(cgmPipelineTest);
            var testInput = InputFactory.Instance().GetInput(cgmPipelineTest.GetSection("Input"),
                ConfigurationService.Instance.GetLoggingConfiguration(), ConfigurationService.Instance.AgentId);
            Assert.IsInstanceOf(typeof(DummyInputForCGMConfiguration), testInput);
            var collected = testInput.Collect(null);
            IConfigurationSection firstProperty = collected.First().GetFieldValue("FirstProperty") as IConfigurationSection;
            Assert.IsNotNull(firstProperty);
            Assert.AreEqual("Product", firstProperty["Name"]);
            var fields = firstProperty.GetSection("Properties");
            Assert.IsNotNull(fields);
            var index = 0;
            foreach (var field in fields.GetChildren())
            {
                if (index == 0)
                {
                    Assert.AreEqual("Code", field["XMLTag"]);
                    Assert.AreEqual("Value", field["Value"]);
                    Assert.AreEqual("install_products.code", field["FieldName"]);
                } else if (index == 1)
                {
                    Assert.AreEqual("VersionExe", field["XMLTag"]);
                    Assert.AreEqual("Value", field["Value"]);
                    Assert.AreEqual("install_products.version", field["FieldName"]);
                }

                index++;
            }
        }
    }
}