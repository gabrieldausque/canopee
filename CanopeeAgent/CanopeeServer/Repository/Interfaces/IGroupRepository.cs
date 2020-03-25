using System.Collections.Generic;
using Canopee.Common;

namespace CanopeeServer.Repository.Interfaces
{
    public interface IGroupRepository
    {
        ICollection<JsonObject> GetGroupsForAgent(string agentId);
    }
}