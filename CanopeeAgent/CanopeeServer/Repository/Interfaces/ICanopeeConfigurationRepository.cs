using System.Collections.Generic;
using CanopeeServer.Datas.Dtos;
using CanopeeServer.Datas.Entities;

namespace CanopeeServer.Repository.Interfaces
{
    public interface ICanopeeConfigurationRepository
    {
        CanopeeConfiguration CreateOrUpdate(CanopeeConfigurationDto newCanopeeConfiguration);
        ICollection<CanopeeConfiguration> GetConfigurations(string agentId, string @group);
        CanopeeConfiguration Delete(string agentId, string group);
    }
}