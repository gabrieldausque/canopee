using System;
using System.Collections.Generic;

namespace Canopee.Common
{
    public interface ICollectedEvent
    {
        DateTime EventDate { get; set; }
        
        String AgentId { get; set; }
        
        Dictionary<string,object> ExtractedFields { get; set; }

        object GetFieldValue(string propertyName);

        void SetFieldValue(string propertyName, object value);

        T ConvertTo<T>() where T:ICollectedEvent,new();
        string GetEventType();
    }
}