using Canopee.Common;
using Canopee.Common.Hosting.Web;
using Canopee.Core.Pipelines;
using Microsoft.AspNetCore.Http;
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
        private readonly ITrigger _trigger;

        public WebAPIHostEventsController()
        {
             _trigger = HttpContext.RequestServices.GetService(typeof(ITrigger)) as ITrigger;
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
            var triggerArgs = new WebTriggerArg(pipelineName, collectedEventAsJson.ToString());
            _trigger.RaiseEvent(this, triggerArgs);
            return CreatedAtRoute("CreateCollectedEvent", new { pipelineName=pipelineName}, true);
        }
    }
}