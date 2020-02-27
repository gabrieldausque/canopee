using System;
using System.Collections.Generic;

namespace CanopeeAgent.Common
{
    public interface ICollectedEvent
    {
        DateTime EventDate { get; set; }
        
        String AgentId { get; set; }
        
        Dictionary<string,object> ExtractedFields { get; }

        void AddExtractedField(string key, object value);

        object GetFieldValue(string propertyName);

        void SetFieldValue(string propertyName, object value);
    }
}