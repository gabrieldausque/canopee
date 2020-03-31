using System;
using System.Collections.Generic;
using System.Linq;
using Canopee.Common;
using CanopeeServer.Datas.Entities;
using Microsoft.Extensions.Options;
using Nest;

namespace CanopeeServer.Datas
{
    public class CanopeeServerDbContext
    {
        private ElasticClient _client;
        private static readonly string CanopeeAgentGroupsIndexName = "canopee-agentgroups";

        public CanopeeServerDbContext(IOptions<CanopeeServerDbSettings> dbSettings)
        {
            var elasticSettings = new ConnectionSettings(new Uri(dbSettings.Value.Url)).EnableDebugMode();
            _client = new ElasticClient(elasticSettings);
        }

        public ICollection<AgentGroup> Groups()
        {
            List<AgentGroup> groups = new List<AgentGroup>();
            var response = _client.Search<AgentGroup>(sd => sd
                .Index(CanopeeAgentGroupsIndexName)
                .Query(q => q.MatchAll())
            );
            return response.Documents.ToList();
        }

        public AgentGroup AddGroup(AgentGroup newAgentGroup)
        {
            if (!Exists(newAgentGroup.AgentId, newAgentGroup.Group))
            {
                _client.Index(new IndexRequest<AgentGroup>(newAgentGroup, CanopeeAgentGroupsIndexName));
                return newAgentGroup;
            }
            else
            {
                return null;
            }
        }

        private bool Exists(string agentId, string @group)
        {
            var response = _client.Search<AgentGroup>(sd => sd
                .Index(CanopeeAgentGroupsIndexName)
                .Query(q => q
                    .Term(g => g.AgentId, agentId ) && q
                    .Term(g => g.Group, group)
                )
            );
            return response.IsValid && response.Documents.Count >= 1;
        }
    }
}