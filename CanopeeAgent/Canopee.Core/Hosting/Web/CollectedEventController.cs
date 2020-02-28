using Canopee.Common;
using Canopee.Common.Hosting.Web;
using Canopee.Core.Pipelines;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Canopee.Core.Hosting.Web
{
    [Route("api/events")]
    [ApiController]
    public class CollectedEventController : Controller
    {
        private readonly ITrigger _trigger;

        public CollectedEventController(ITrigger trigger)
        {
             _trigger = trigger;
        }
        
        [HttpGet]
        [Route("ping")]
        public IActionResult Ping()
        {
            return Ok(true);
        }

        [HttpPost("{pipelineName}", Name = "CreateCollectedEvent")]
        public IActionResult CreateCollectedEvent(string pipelineName, [FromBody] JToken collectedEventAsJson)
        {
            var triggerArgs = new WebTriggerArg(pipelineName, collectedEventAsJson.ToString());
            _trigger.RaiseEvent(this, triggerArgs);
            return CreatedAtRoute("CreateCollectedEvent", new { pipelineName=pipelineName}, true);
        }
    }
}