using Canopee.Common.Events;

namespace Canopee.Common.Hosting.Web
{
    public class WebTriggerArg : TriggerEventArgs
    {
        public WebTriggerArg(string pipelineName, string collectedEventAsJson)
        {
            PipelineName = pipelineName;
            RawEvent = collectedEventAsJson;
        }

        public string RawEvent { get; set; }

        public string PipelineName { get; set; }
    }
}