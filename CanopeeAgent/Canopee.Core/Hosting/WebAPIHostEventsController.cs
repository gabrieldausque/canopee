using Canopee.Common;
using Canopee.Common.Events;
using Canopee.Core.Pipelines;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Canopee.Core.Hosting
{
    [Route("api/events")]
    [ApiController]
    public class WebAPIHostEventsController : Controller
    {
        private IConfiguration _configuration;
        private ITrigger _trigger;

        public WebAPIHostEventsController(IConfiguration configuration)
        {
            _configuration = configuration.GetSection("CanopeeServer");
            var triggerConfiguration = _configuration.GetSection("Trigger");
            _trigger = TriggerFactory.Instance.GetTrigger(triggerConfiguration);
        }
        
        [HttpGet]
        [Route("ping")]
        public IActionResult Ping()
        {
            return Ok(true);
        }

        [HttpPost("{pipelineName:string}", Name = "CreateCollectedEvent")]
        public IActionResult CreateCollectedEvent(string pipelineName, [FromBody] JToken collectedEventAsJson)
        {
            var triggerArgs = new WebTriggerArg(pipelineName, collectedEventAsJson);
            _trigger.RaiseEvent(this, triggerArgs);
            return CreatedAtRoute("CreateCollectedEvent", new { pipelineName=pipelineName}, true);
        }
    }

    public class WebTriggerArg : TriggerEventArgs
    {
        public WebTriggerArg(string pipelineName, JToken collectedEventAsJson)
        {
            PipelineName = pipelineName;
            RawEvent = collectedEventAsJson;
        }

        public JToken RawEvent { get; set; }

        public string PipelineName { get; set; }
    }
}