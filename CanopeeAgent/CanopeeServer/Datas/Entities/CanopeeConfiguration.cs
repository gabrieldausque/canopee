using System;
using Canopee.Common;
using System.Text.Json;

namespace CanopeeServer.Datas.Entities
{
    public class CanopeeConfiguration
    {
        public CanopeeConfiguration()
        {
            EventId = Guid.NewGuid().ToString();
            EventDate = DateTime.Now;
        }
        
        public JsonObject Configuration { get; set; }

        public string AgentId { get; set; }

        public string EventId { get; set; }

        public DateTime EventDate { get; set; }
        
        public string Group { get; set; }
        public int Priority { get; set; }

        public override string ToString()
        {
            var serialized = new JsonObject();
            serialized.SetProperty("Configuration",this.Configuration);
            serialized.SetProperty("AgentId", this.AgentId);
            serialized.SetProperty("EventId",this.EventId);
            serialized.SetProperty("EventDate",this.EventDate);
            serialized.SetProperty("Group",this.Group);
            serialized.SetProperty("Priority",this.Priority);
            return serialized.ToString();
        }
    }
}