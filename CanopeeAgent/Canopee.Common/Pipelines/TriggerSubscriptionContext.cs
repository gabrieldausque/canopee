namespace Canopee.Common.Pipelines
{
    /// <summary>
    /// The context of a subscription done on a <see cref="ITrigger"/> 
    /// </summary>
    public class TriggerSubscriptionContext
    {
        /// <summary>
        /// The name of the pipeline that make a subscription
        /// </summary>
        public string PipelineName { get; set; }
        
        /// <summary>
        /// The id of the pipeline that make the subscription
        /// </summary>
        public string PipelineId { get; set; }
    }
}