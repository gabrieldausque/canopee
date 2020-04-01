using System.Collections.Generic;
using System.Linq;
using Canopee.Common;
using CanopeeServer.Datas;
using CanopeeServer.Datas.Entities;
using CanopeeServer.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CanopeeServer.Repository
{
    public class GroupRepository : IGroupRepository
    {
        private CanopeeServerDbContext _db;

        public GroupRepository(CanopeeServerDbContext dbContext)
        {
            _db = dbContext;
        }

        public ICollection<AgentGroup> GetGroupsForAgent(string agentId)
        {
            var fromDb = _db.Groups().Where(g => g.AgentId == agentId).ToList();
            var uniques = new List<AgentGroup>();
            var keys = new List<string>();
            foreach (var group in fromDb)
            {
                var key = $"{group.AgentId}:{group.Group}";
                if (!keys.Contains(key))
                {
                    keys.Add(key);
                    uniques.Add(group);
                }
            }
            uniques.Sort((left, right) =>
            {
                var currentPriority = left.Priority; 
                var otherPriority = right.Priority;
                if (currentPriority > otherPriority)
                    return -1;
                
                if (currentPriority < otherPriority)
                    return 1;
                
                return 0;
            });
            return uniques;
        }

        public AgentGroup CreateOrUpdateAgentGroup(string agentId, string group,int priority)
        {
            return _db.AddGroup(new AgentGroup()
            {
                AgentId = agentId,
                Group = group,
                Priority = priority
            });
        }
    }
}