using System;
using System.Collections.Generic;

namespace CanopeeAgent.Common.Events
{
    public class BaseCollectedEvent : ICollectedEvent
    {
        public BaseCollectedEvent()
        {
            ExtractedFields = new Dictionary<string, object>();
        }
        
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