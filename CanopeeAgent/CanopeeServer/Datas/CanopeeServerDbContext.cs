using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Canopee.Common;
using CanopeeServer.Datas.Entities;
using Elasticsearch.Net;
using Microsoft.Extensions.Options;
using Nest;

namespace CanopeeServer.Datas
{
    public class CanopeeServerDbContext
    {
        private ElasticClient _client;
        private static readonly string CanopeeAgentGroupsIndexName = "canopee-agentgroups";
        private static readonly string CanopeeConfigurationIndexName = "canopee-configurations";

        public CanopeeServerDbContext(IOptions<CanopeeServerDbSettings> dbSettings)
        {
            var elasticSettings = new ConnectionSettings(new Uri(dbSettings.Value.Url))
                .EnableDebugMode()
                .DefaultFieldNameInferrer(f => f);
            _client = new ElasticClient(elasticSettings);
        }

        public ICollection<AgentGroup> GetGroups(string agentId)
        {
            var response = _client.Search<JsonObject>(sd => sd
                .Index(CanopeeAgentGroupsIndexName)
                .Query(q => q
                    .Bool(b => b
                    .Should(s => s
                        .Match(mq => mq
                            .Field("AgentId")
                            .Query(agentId)))))
            );
            var agentGroups = new List<AgentGroup>();
            foreach (var agentGroupAsJsonObject in response.Documents)
            {
                var cleanObject = JsonObject.CleanDocument(agentGroupAsJsonObject);
                //cleanObject.SetProperty("EventDate", DateTime.Parse(cleanObject.GetProperty<string>("EventDate")));
                agentGroups.Add(new AgentGroup()
                    {
                        Group = cleanObject.GetProperty<string>("Group"),
                        Priority = cleanObject.GetProperty<long>("Priority"),
                        AgentId = cleanObject.GetProperty<string>("AgentId"),
                        EventDate = cleanObject.GetProperty<DateTime>("EventDate"),
                        EventId = cleanObject.GetProperty<string>("EventId")
                    });
            }
            return agentGroups;
        }

        public AgentGroup AddGroup(AgentGroup newAgentGroup)
        {
            if (GroupExists(newAgentGroup.AgentId, newAgentGroup.Group))
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
            if (!deleteResponse.IsValid)
            {
                throw new ApplicationException(deleteResponse.DebugInformation);
            }
        }
        
        private bool GroupExists(string agentId, string group)
        {
            var response = _client.Search<JsonObject>(sd => sd
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

        public ICollection<CanopeeConfiguration> Configurations()
        {
            var response = _client.Search<JsonObject>(sd => sd
                .Index(CanopeeConfigurationIndexName)
                .Query(q => q.MatchAll())
            );
            var configs = new List<CanopeeConfiguration>();
            foreach (var config in response.Documents)
            {
                var cleanObject = JsonObject.CleanDocument(config);
                cleanObject.SetProperty("EventDate", DateTime.Parse(cleanObject.GetProperty<string>("EventDate")));
                configs.Add(new CanopeeConfiguration()
                {
                    AgentId = cleanObject.GetProperty<string>("AgentId"),
                    Configuration = cleanObject.GetProperty<JsonObject>("Configuration"),
                    EventDate = cleanObject.GetProperty<DateTime>("EventDate"),
                    EventId = cleanObject.GetProperty<string>("EventId"),
                    Priority = cleanObject.GetProperty<short>("Priority"),
                    Group = cleanObject.GetProperty<string>("Group")
                });
            }
            return configs;
        }

        public CanopeeConfiguration AddConfiguration(CanopeeConfiguration canopeeConfiguration)
        {
            if (CanopeeConfigurationExists(canopeeConfiguration))
            {
                DeleteConfiguration(canopeeConfiguration);
            }

            var serializedConfiguration = canopeeConfiguration.ToString();
            var response = _client.LowLevel.Index<IndexResponse>(CanopeeConfigurationIndexName, PostData.String(serializedConfiguration));
            if (!response.IsValid)
            {
                throw new ApplicationException(response.DebugInformation);
            }
            return canopeeConfiguration;
        }

        public void DeleteConfiguration(CanopeeConfiguration canopeeConfiguration)
        {
            var deleteResponse = _client.DeleteByQuery<CanopeeConfiguration>(s => s
                .Index(CanopeeConfigurationIndexName)
                .Query(q => q
                    .Bool(b => b
                        .Should(s => s
                            .Match(mq => mq
                                .Field("AgentId")
                                .Query(canopeeConfiguration.AgentId)))
                        .Should(s => s
                            .Match(mq => mq
                                .Field("Group")
                                .Query(canopeeConfiguration.Group)))
                    )
                )
            );
            if (!deleteResponse.IsValid)
            {
                throw new ApplicationException(deleteResponse.DebugInformation);
            }
        }

        private bool CanopeeConfigurationExists(CanopeeConfiguration canopeeConfiguration)
        {
            var response = _client.Search<JsonDocument>(sd => sd
                .Index(CanopeeConfigurationIndexName)
                .Query(q => q
                    .Bool(b => b
                        .Should(s => s
                            .Match(mq => mq
                                .Field("AgentId")
                                .Query(canopeeConfiguration.AgentId)))
                        .Should(s => s
                            .Match(mq => mq
                                .Field("Group")
                                .Query(canopeeConfiguration.Group)))
                    )
                )
            );
            return response.IsValid && response.Documents.Count >= 1;
        }

        public ICollection<CanopeeConfiguration> GetConfiguration(string agentId, string @group)
        {
            var response = _client.Search<JsonObject>(sd => sd
                            .Index(CanopeeConfigurationIndexName)
                            .Query(q => q
                                .Bool(b => b
                                    .Should(s => s
                                        .Match(mq => mq
                                            .Field("AgentId")
                                            .Query(agentId)))
                                    .Should(s => s
                                        .Match(mq => mq
                                            .Field("Group")
                                            .Query(group))))));
            var configs = new List<CanopeeConfiguration>();
            foreach (var config in response.Documents)
            {
                var cleanObject = JsonObject.CleanDocument(config);
                cleanObject.SetProperty("EventDate", DateTime.Parse(cleanObject.GetProperty<string>("EventDate")));
                configs.Add(new CanopeeConfiguration()
                {
                    AgentId = cleanObject.GetProperty<string>("AgentId"),
                    Configuration = cleanObject.GetProperty<JsonObject>("Configuration"),
                    EventDate = cleanObject.GetProperty<DateTime>("EventDate"),
                    EventId = cleanObject.GetProperty<string>("EventId"),
                    Priority = cleanObject.GetProperty<short>("Priority"),
                    Group = cleanObject.GetProperty<string>("Group")
                });
            }
            return configs;
        }
    }
}