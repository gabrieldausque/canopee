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
            var elasticSettings = new ConnectionSettings(new Uri(dbSettings.Value.Url))
                .EnableDebugMode()
                .DefaultFieldNameInferrer(f => f);
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
            if (Exists(newAgentGroup.AgentId, newAgentGroup.Group))
            {
                DeleteGroup(newAgentGroup);    
            }
            
            var response = _client.Index(new IndexRequest<AgentGroup>(newAgentGroup, CanopeeAgentGroupsIndexName));
            if (!response.IsValid)
            {
                throw new ApplicationException(response.DebugInformation);
            }
            return newAgentGroup;
        }

        public void DeleteGroup(AgentGroup agentGroupToDelete)
        {
            var deleteResponse = _client.DeleteByQuery<AgentGroup>(s => s
                .Index(CanopeeAgentGroupsIndexName)
                .Query(q => q
                    .Bool(b => b
                        .Should(s => s
                            .Match(mq => mq
                                .Field("AgentId")
                                .Query(agentGroupToDelete.AgentId)))
                        .Should(s => s
                            .Match(mq => mq
                                .Field("Group")
                                .Query(agentGroupToDelete.Group)))
                    )
                )
            );
        }
        
        private bool Exists(string agentId, string group)
        {
            var response = _client.Search<AgentGroup>(sd => sd
                .Index(CanopeeAgentGroupsIndexName)
                .Query(q => q
                    .Bool(b => b
                        .Should(s => s
                             .Match(mq => mq
                                 .Field("AgentId")
                                 .Query(agentId)))
                        .Should(s => s
                            .Match(mq => mq
                                .Field("Group")
                                .Query(group)))
                    )
                )
            );
            return response.IsValid && response.Documents.Count >= 1;
        }
    }
}