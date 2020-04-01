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
            EventDate = JsonSerializer.Serialize(DateTime.Now);
        }
        
        public JsonObject Configuration { get; set; }

        public string AgentId { get; set; }

        public string EventId { get; set; }

        public string EventDate { get; set; }
        
        public string Group { get; set; }
        public int Priority { get; set; }
    }
}