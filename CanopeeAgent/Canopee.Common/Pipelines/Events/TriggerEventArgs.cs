using System;

namespace Canopee.Common.Pipelines.Events
{
    public class TriggerEventArgs : EventArgs
    {
        public TriggerEventArgs()
        {
        }
        
        public TriggerEventArgs(string name, string Id):this()
        {
            PipelineName = name;
            PipelineId = Id;
        }
        public string PipelineName { get; set; }
        public string PipelineId { get; set; }
        
        public object Raw { get; set; }
    }
}