using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Canopee.Common;
using Canopee.Common.Pipelines;
using Canopee.Common.Pipelines.Events;
using Canopee.Core.Pipelines;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Configuration;

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

        /// <summary>
        /// Create a collected event, using a specific pipeline transformation
        /// </summary>
        /// <param name="pipelineId">the Id of the pipeline to be executed for the Event</param>
        /// <param name="collectedEvent">collected event received</param>
        /// <returns>
        ///     
        /// </returns>
        [HttpPost(Name="CreateCollectedEvent")]
        [ProducesResponseType(201,Type=typeof(bool))]
        [ProducesDefaultResponseType]
        public IActionResult CreateCollectedEvent([FromQuery, Required] string pipelineId, [FromBody] CollectedEvent collectedEvent)
        {
            var triggerArgs = new WebTriggerArg(pipelineId, collectedEvent);
            try
            {
                _trigger?.RaiseEvent(this, triggerArgs);
                return CreatedAtRoute("CreateCollectedEvent", new { pipelineId=pipelineId}, true);
            }
            catch (Exception e)
            {
                //TODO : log error locally
                ModelState.AddModelError("",$"Error while collecting an event : {e}");
                return StatusCode(500, ModelState);
            }
        }
    }
}