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
            return Ok(groups);
        }
    }
}