namespace Canopee.Common.Pipelines.Events
{
    public class WebTriggerArg : TriggerEventArgs
    {
        public WebTriggerArg(string pipelineId, CollectedEvent collectedEvent)
        {
            PipelineId = pipelineId;
            Raw = collectedEvent;
        }
    }
}