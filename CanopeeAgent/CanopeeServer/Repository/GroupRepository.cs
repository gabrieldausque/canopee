using System.Collections.Generic;
using System.Linq;
using Canopee.Common;
using CanopeeServer.Datas;
using CanopeeServer.Repository.Interfaces;

namespace CanopeeServer.Repository
{
    public class GroupRepository : IGroupRepository
    {
        private CanopeeServerDbContext _db;

        public GroupRepository(CanopeeServerDbContext dbContext)
        {
            _db = dbContext;
        }

        public ICollection<JsonObject> GetGroupsForAgent(string agentId)
        {
            var fromDb = _db.Groups().Where(g => g.GetProperty<string>("AgentId") == agentId).ToList();
            var uniques = new List<JsonObject>();
            var keys = new List<string>();
            foreach (var group in fromDb)
            {
                var key = $"{group.GetProperty<string>("AgentId")}:{group.GetProperty<string>("Group")}";
                if (!keys.Contains(key))
                {
                    keys.Add(key);
                    uniques.Add(group);
                }
            }
            uniques.Sort((left, right) =>
            {
                var currentPriority = left.GetProperty<int>("Priority"); 
                var otherPriority = right.GetProperty<int>("Priority");
                if (currentPriority > otherPriority)
                    return -1;
                else if (currentPriority < otherPriority)
                    return 1;
                return 0;
            });
            return uniques;
        }
    }
}