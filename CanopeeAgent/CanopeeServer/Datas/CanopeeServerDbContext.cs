using System;
using System.Collections.Generic;
using Canopee.Common;
using Microsoft.Extensions.Options;
using Nest;

namespace CanopeeServer.Datas
{
    public class CanopeeServerDbContext
    {
        private ElasticClient _client;

        public CanopeeServerDbContext(IOptions<CanopeeServerDbSettings> dbSettings)
        {
            var elasticSettings = new ConnectionSettings(new Uri(dbSettings.Value.Url)).EnableDebugMode();
            _client = new ElasticClient(elasticSettings);
        }

        public ICollection<JsonObject> Groups()
        {
            List<JsonObject> groups = new List<JsonObject>();
            var response = _client.Search<dynamic>(sd => sd
                .Index("canopee-agentgroups")
                .Query(q => q.MatchAll())
            );
            //TODO : manage unsucessfull request
            foreach(var document in response.Documents)
            {
                JsonObject group = new JsonObject();
                foreach(var prop in document.Keys)
                {
                    if (prop == "Priority")
                    {
                        group.SetProperty(prop,int.Parse(document[prop]));
                    }
                    else
                    {
                        group.SetProperty(prop, document[prop]);    
                    }
                }
                groups.Add(group);
            }
            return groups;
        }
    }
}