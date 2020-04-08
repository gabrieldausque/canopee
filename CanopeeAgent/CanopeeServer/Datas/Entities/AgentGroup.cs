using System;

namespace CanopeeServer.Datas.Entities
{
    public class AgentGroup
    {
        public AgentGroup()
        {
            EventDate = DateTime.Now;
            EventId = Guid.NewGuid().ToString();
        }
        
        public string AgentId { get; set; }

        public string Group { get; set; }
        
        public long Priority { get; set; }

        public DateTime EventDate { get; set; }

        public string EventId { get; set; }
        
    }
}