using System.Collections.Generic;
using Canopee.Common.Configuration.AspNet.Dtos;
using CanopeeServer.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace CanopeeServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : Controller
    {
        private ICanopeeConfigurationRepository _repository;

        public ConfigurationController(ICanopeeConfigurationRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult Create(CanopeeConfigurationDto newCanopeeConfiguration)
        {
            return CreatedAtAction("Create", _repository.CreateOrUpdate(newCanopeeConfiguration));
        }

        [HttpGet]
        [ResponseCache(NoStore = true)]
        public IActionResult Get([FromQuery] string agentId = "", [FromQuery] string group = "")
        {
            var configurations = _repository.GetConfigurations(agentId, group);
            var dtos = new List<CanopeeConfigurationDto>(); 
            foreach (var config in configurations)
            {
                dtos.Add(new CanopeeConfigurationDto()
                {
                    Configuration = config.Configuration,
                    Group = config.Group,
                    AgentId = config.AgentId
                });
            }
            return Ok(dtos);
        }

        [HttpPatch]
        public IActionResult Update([FromBody] CanopeeConfigurationDto configToUpdate)
        {
            return AcceptedAtAction("Update",_repository.CreateOrUpdate(configToUpdate));
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery]string agentId, string group)
        {
            _repository.Delete(agentId, group);
            return Ok();
        }
    }
}