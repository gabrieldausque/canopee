using System.Collections.Generic;
using System.Linq;
using Canopee.Common.Configuration.AspNet.Dtos;
using CanopeeServer.Controllers;
using CanopeeServer.Datas;
using CanopeeServer.Datas.Entities;
using CanopeeServer.Repository.Interfaces;

namespace CanopeeServer.Repository
{
    public class CanopeeConfigurationRepository : ICanopeeConfigurationRepository
    {
        private CanopeeServerDbContext _db;

        public CanopeeConfigurationRepository(CanopeeServerDbContext dbContext)
        {
            _db = dbContext;
        }
        
        public CanopeeConfiguration CreateOrUpdate(CanopeeConfigurationDto newCanopeeConfiguration)
        {
            var canopeeConfiguration = new CanopeeConfiguration()
            {
                Configuration = newCanopeeConfiguration.Configuration,
                Group = newCanopeeConfiguration.Group,
                AgentId = newCanopeeConfiguration.AgentId,
                Priority = newCanopeeConfiguration.Priority
            };
            return _db.AddConfiguration(canopeeConfiguration);
        }

        public ICollection<CanopeeConfiguration> GetConfigurations(string agentId, string group)
        {
            return _db.GetConfiguration(agentId, group);
        }

        public CanopeeConfiguration Delete(string agentId, string @group)
        {
            var configToDelete = GetConfigurations(agentId, group).FirstOrDefault();
            _db.DeleteConfiguration(new CanopeeConfiguration()
            {
                AgentId = agentId,
                Group = group
            });
            return configToDelete;
        }
    }
}