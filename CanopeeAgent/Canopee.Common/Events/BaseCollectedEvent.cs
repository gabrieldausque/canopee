using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Canopee.Common.Events
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
        
        [JsonExtensionData]
        public Dictionary<string, object> ExtractedFields { get; }
        
        public void AddExtractedField(string key, object value)
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

        public object GetFieldValue(string propertyName)
        {
            var prop = GetType().GetProperty(propertyName);
            if (prop != null)
            {
                return prop.GetValue(this);
            }

            if (ExtractedFields.ContainsKey(propertyName))
            {
                return ExtractedFields[propertyName];
            }

            return null;
        }

        public void SetFieldValue(string propertyName, object value)
        {
            var prop = GetType().GetProperty(propertyName);
            if (prop != null)
            {
                prop.SetValue(this, value);
            }
            else
            {
                if (ExtractedFields.ContainsKey(propertyName))
                {
                    ExtractedFields[propertyName] = value;
                }
                else
                {
                    ExtractedFields.Add(propertyName, value);
                }
            }
        }
    }
}