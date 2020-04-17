namespace Canopee.Common.Pipelines.Events
{
    /// <summary>
    /// A <see cref="TriggerEventArgs"/> specific to a web context
    /// </summary>
    public class WebTriggerArg : TriggerEventArgs
    {
        /// <summary>
        /// Constructor that set up a CollectedEvent that may have been transmitted through a web call
        /// </summary>
        /// <param name="pipelineId">the id of the pipeline to trigger</param>
        /// <param name="collectedEvent">the collected event to be treated by the pipeline</param>
        public WebTriggerArg(string pipelineId, CollectedEvent collectedEvent)
        {
            PipelineId = pipelineId;
            Raw = collectedEvent;
        }
    }
}