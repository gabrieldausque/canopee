using System;

namespace Canopee.Common.Pipelines.Events
{
    /// <summary>
    /// The arg of an event that triggered a pipeline.
    /// </summary>
    public class TriggerEventArgs : EventArgs
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public TriggerEventArgs()
        {
        }
        
        /// <summary>
        /// Create a trigger event arg with setting the name of the pipeline to trigger and its Id
        /// </summary>
        /// <param name="name">The name of the pipeline</param>
        /// <param name="Id">The id of the pipeline</param>
        public TriggerEventArgs(string name, string Id):this()
        {
            PipelineName = name;
            PipelineId = Id;
        }

        /// <summary>
        /// The name of the pipeline to trigger 
        /// </summary>
        public string PipelineName { get; set; }
        
        /// <summary>
        /// The id of the pipeline to trigger
        /// </summary>
        public string PipelineId { get; set; }
        
        /// <summary>
        /// An object specific to the trigger
        /// </summary>
        public object Raw { get; set; }
    }
}