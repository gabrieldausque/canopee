using System;
using System.Collections.Generic;

namespace CanopeeAgent.Common.Events
{
    public abstract class BaseCollectedEvent : ICollectedEvent
    {
        protected BaseCollectedEvent(string agentId)
        {
            AgentId = agentId;
            EventId = Guid.NewGuid().ToString();
            EventDate = DateTime.Now;
            ExtractedFields = new Dictionary<string, object>();
        }
        public string EventId { get; set; }
        public DateTime EventDate { get; set; }
        public string AgentId { get; set; }
        public Dictionary<string, object> ExtractedFields { get; }
        
        public void AddExtendedField(string key, object value)
        {
            if (!ExtractedFields.ContainsKey(key))
            {
                ExtractedFields.Add(key, value);
            }
            else
            {
                ExtractedFields[key] = value;
            }
        }
    }
}