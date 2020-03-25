using System;
using System.Collections.Generic;
using System.Linq;
using Canopee.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nest;

namespace CanopeeServer
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
        public IActionResult GetGroups([FromQuery] string agentId)
        {
            var groups = _repository.GetGroupsForAgent(agentId);
            return Ok(groups);
        }
    }

    public class GroupRepository : IGroupRepository
    {
        private CanopeeServerDbContext _db;

        public GroupRepository(CanopeeServerDbContext dbContext)
        {
            _db = dbContext;
        }

        public ICollection<JsonObject> GetGroupsForAgent(string agentId)
        {
            return _db.Groups().Where(g => g.GetProperty<string>("AgentId") == agentId).ToList();
        }
    }

    public class CanopeeServerDbContext
    {
        private ElasticClient _client;

        public CanopeeServerDbContext(IOptions<CanopeeServerDbSettings> dbSettings)
        {
            var elasticSettings = new ConnectionSettings(new Uri(dbSettings.Value.Url)).EnableDebugMode();
            _client = new ElasticClient(elasticSettings);
        }

        public ICollection<JsonObject> Groups()
        {
            List<JsonObject> groups = new List<JsonObject>();
            var response = _client.Search<dynamic>(sd => sd
                .Index("canopee-agentgroups")
                .Sort(s => s.Descending("AgentId"))
                .Query(q => q.MatchAll())
            );
            foreach(var document in response.Documents)
            {
                JsonObject group = new JsonObject();
                foreach(var prop in document.Keys)
                {
                    group.SetProperty(prop, document[prop]);
                }
                groups.Add(group);
            }
            return groups;
        }
    }

    public class CanopeeServerDbSettings
    {
        public string Url { get; set; }
    }

    public interface IGroupRepository
    {
        ICollection<JsonObject> GetGroupsForAgent(string agentId);
    }
}