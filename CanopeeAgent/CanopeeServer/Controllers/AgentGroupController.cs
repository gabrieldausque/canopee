using System.Collections.Generic;
using System.Text.Json;
using Canopee.Common;
using Canopee.Common.Configuration.AspNet.Dtos;
using CanopeeServer.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CanopeeServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentGroupController : Controller
    {
        private IGroupRepository _repository;

        public AgentGroupController(IGroupRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult Create([FromBody] AgentGroupDto agentGroupDto)
        {
            return CreatedAtAction("Create",_repository.CreateOrUpdateAgentGroup(agentGroupDto.AgentId, agentGroupDto.Group, agentGroupDto.Priority));
        }
        
        [HttpGet]
        [ResponseCache(NoStore = true)]
        public IActionResult Get([FromQuery] string agentId)
        {
            var groups = _repository.GetGroupsForAgent(agentId);
            var groupDtos = new List<AgentGroupDto>();
            foreach (var group in groups)
            {
                groupDtos.Add(new AgentGroupDto()
                {
                    AgentId = group.AgentId,
                    Group = group.Group,
                    Priority = group.Priority
                });
            }
            return Ok(groupDtos);
        }

        [HttpPatch]
        public IActionResult Update([FromBody] AgentGroupDto agentGroupToUpdate)
        {
            return  CreatedAtAction("Update",_repository.CreateOrUpdateAgentGroup(agentGroupToUpdate.AgentId, agentGroupToUpdate.Group, agentGroupToUpdate.Priority));
        }
        
        [HttpDelete]
        public IActionResult Delete([FromQuery] string agentId, [FromQuery] string group)
        {
            _repository.Delete(agentId, group);
            return Ok();
        }
    }
}