using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Canopee.Common.Events
{
    public class CollectedEvent : ICollectedEvent
    {
        public CollectedEvent()
        {
            EventId = Guid.NewGuid().ToString();
            EventDate = DateTime.Now;
            ExtractedFields = new Dictionary<string, object>();
        }
        public CollectedEvent(string agentId):this()
        {
            AgentId = agentId;
        }

        public string EventId { get; set; }
        public DateTime EventDate { get; set; }
        public string AgentId { get; set; }

        [JsonIgnore]
         public string Raw { get; set; }
        
        [JsonExtensionData]
        public Dictionary<string, object> ExtractedFields { get; set; }
        
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

        public T ConvertTo<T>() where T:ICollectedEvent,new()
        {
            T converted = new T();
            foreach (var propertyInfo in GetType().GetProperties())
            {
                if (propertyInfo.Name != "ExtractedFields")
                {
                    converted.SetFieldValue(propertyInfo.Name, propertyInfo.GetValue(this));    
                }
            }

            foreach (var extractedFields in ExtractedFields)
            {
                converted.SetFieldValue(extractedFields.Key, extractedFields.Value);
            }
            return converted;
        }

        public string GetEventType()
        {
            var extractedEventType = GetFieldValue("EventType");
            if (extractedEventType != null)
                return extractedEventType.ToString();
            return this.GetType().FullName;
        }
    }
}