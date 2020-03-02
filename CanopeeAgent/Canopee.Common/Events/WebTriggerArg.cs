using Canopee.Common.Events;

namespace Canopee.Common.Hosting.Web
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