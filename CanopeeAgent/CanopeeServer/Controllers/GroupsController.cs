using System.Collections.Generic;
using System.Text.Json;
using Canopee.Common;
using CanopeeServer.Datas.Dtos;
using CanopeeServer.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CanopeeServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : Controller
    {
        private IGroupRepository _repository;

        public GroupsController(IGroupRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ResponseCache(NoStore = true)]
        public IActionResult GetGroups([FromQuery] string agentId)
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

        [HttpPost]
        public IActionResult AddGroupForAgent([FromBody] AgentGroupDto agentGroupDto)
        {
            return CreatedAtAction("AddGroupForAgent",_repository.AddGroupForAgent(agentGroupDto.AgentId, agentGroupDto.Group, agentGroupDto.Priority));
        }
    }
}