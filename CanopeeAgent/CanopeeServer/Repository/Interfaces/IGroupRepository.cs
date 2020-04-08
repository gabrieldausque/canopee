using System.Collections.Generic;
using Canopee.Common;
using CanopeeServer.Datas.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CanopeeServer.Repository.Interfaces
{
    public interface IGroupRepository
    {
        ICollection<AgentGroup> GetGroupsForAgent(string agentId);
        AgentGroup CreateOrUpdateAgentGroup(string agentId, string group, long priority);
    }
}