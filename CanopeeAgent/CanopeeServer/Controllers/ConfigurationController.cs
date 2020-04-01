using System.Collections.Generic;
using CanopeeServer.Datas.Dtos;
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
        public IActionResult Get([FromQuery] string agentId, [FromQuery] string group)
        {
            var configurations = _repository.GetConfigurations(agentId, group);
            var dtos = new List<CanopeeConfigurationDto>(); 
            foreach (var config in configurations)
            {
                dtos.Add(new CanopeeConfigurationDto()
                {
                    Configuration = config.Configuration,
                    Group = config.Group,
                    AgentId = config.AgentId,
                    Priority = config.Priority
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
        public IActionResult Delete([FromBody] CanopeeConfigurationDto configToDelete)
        {
            _repository.Delete(configToDelete.AgentId, configToDelete.Group);
            return Ok();
        }
    }
}