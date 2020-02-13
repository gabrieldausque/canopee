using System;
using System.Collections.Generic;

namespace CanopeeAgent.Common
{
    public interface ICollectedEvent
    {
        DateTime EventDate { get; set; }
        
        String AgentId { get; set; }
        
        Dictionary<string,object> ExtractedFields { get; }

        void AddExtendedField(string key, object value);
    }
}