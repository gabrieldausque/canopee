using System.Collections.Generic;
using Canopee.Common;
using CanopeeServer.Datas.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CanopeeServer.Repository.Interfaces
{
    public interface IGroupRepository
    {
        ICollection<AgentGroup> GetGroupsForAgent(string agentId);
        AgentGroup AddGroupForAgent(string agentId, string group, int priority);
    }
}