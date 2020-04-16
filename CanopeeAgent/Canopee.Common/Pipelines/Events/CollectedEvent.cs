using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Canopee.Common.Pipelines.Events
{
    /// <summary>
    /// An event collected, transformed and output by a <see cref="ICollectPipeline"/>
    /// Implements <see cref="ICollectedEvent"/>
    /// </summary>
    public class CollectedEvent : ICollectedEvent
    {
        /// <summary>
        /// Default constructor of the constructor event
        /// </summary>
        public CollectedEvent()
        {
            EventId = Guid.NewGuid().ToString();
            EventDate = DateTime.Now;
            ExtractedFields = new Dictionary<string, object>();
        }
        
        /// <summary>
        /// Constructor that specified the agentid of the collected event
        /// </summary>
        /// <param name="agentId"></param>
        public CollectedEvent(string agentId):this()
        {
            AgentId = agentId;
        }

        /// <summary>
        /// The id of the collected event
        /// </summary>
        public string EventId { get; set; }
        
        /// <summary>
        ///   Inherit from <see cref="ICollectedEvent.EventDate"/>
        /// </summary>
        /// <inheritdoc cref="ICollectedEvent"/>
        public DateTime EventDate { get; set; }
        /// <summary>
        ///   Inherit from <see cref="ICollectedEvent.AgentId"/>
        /// </summary>
        /// <inheritdoc cref="ICollectedEvent"/>
        public string AgentId { get; set; }

        /// <summary>
        /// Event as raw string.
        /// </summary>
        /// <inheritdoc cref="ICollectedEvent"/>
        [JsonIgnore]
         public string Raw { get; set; }
        
        /// <summary>
        ///   Inherit from <see cref="ICollectedEvent.ExtractedFields"/>
        /// </summary>
        /// <inheritdoc cref="ICollectedEvent"/>
        [JsonExtensionData]
        public Dictionary<string, object> ExtractedFields { get; set; }
        
        /// <summary>
        ///   Inherit from <see cref="ICollectedEvent.GetFieldValue"/>
        /// </summary>
        /// <inheritdoc cref="ICollectedEvent"/>
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

        /// <summary>
        ///   Inherit from <see cref="ICollectedEvent.SetFieldValue"/>
        /// </summary>
        /// <inheritdoc cref="ICollectedEvent"/>
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

        /// <summary>
        ///   Inherit from <see cref="ICollectedEvent.ConvertTo<T>"/>
        /// </summary>
        /// <inheritdoc cref="ICollectedEvent"/>
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

        /// <summary>
        ///   Inherit from <see cref="ICollectedEvent.GetEventType"/>
        /// </summary>
        /// <inheritdoc cref="ICollectedEvent"/>
        public string GetEventType()
        {
            var extractedEventType = GetFieldValue("EventType");
            if (extractedEventType != null)
                return extractedEventType.ToString();
            return this.GetType().FullName;
        }
    }
}