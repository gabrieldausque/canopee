using System;
using System.Collections.Generic;

namespace Canopee.Common.Pipelines.Events
{
    public interface ICollectedEvent
    {
        /// <summary>
        /// The <see cref="DateTime"/> when the event occured/collected
        /// </summary>
        DateTime EventDate { get; set; }
        
        /// <summary>
        /// The agent id on which the event has been collected
        /// </summary>
        String AgentId { get; set; }
        
        /// <summary>
        /// Extended fields of the event that may have been extracted or set by a <see cref="ITransform"/> during pipeline transformation
        /// </summary>
        Dictionary<string,object> ExtractedFields { get; set; }

        /// <summary>
        /// Get a field value (standard property or from extracted fields)
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        object GetFieldValue(string propertyName);

        /// <summary>
        /// Set a field value on standard properties or <see cref="ExtractedFields"/> if no standard properties are found for given name
        /// </summary>
        /// <param name="propertyName">the name of the property to set value for</param>
        /// <param name="value">the value to set</param>
        void SetFieldValue(string propertyName, object value);

        /// <summary>
        /// Convert a <see cref="ICollectedEvent"/> to another <see cref="ICollectedEvent"/>
        /// </summary>
        /// <typeparam name="T">the target <see cref="ICollectedEvent"/> type</typeparam>
        /// <returns>an instance of the target <see cref="ICollectedEvent"/> type</returns>
        T ConvertTo<T>() where T:ICollectedEvent,new();
        /// <summary>
        /// Get the type of the event. May be a custom string, by default the full name of the class
        /// </summary>
        /// <returns></returns>
        string GetEventType();
    }
}