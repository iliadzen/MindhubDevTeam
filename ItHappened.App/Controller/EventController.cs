using System;
using System.Security.Claims;
using ItHappened.App.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.App.Controller
{
    [Authorize]
    [Route("trackers/{trackerId}")]
    public class EventController : ControllerBase
    {
        [HttpGet]
        [Route("events")]
        public IActionResult GetEvents([FromRoute] Guid trackerId)
        {
            var userId = User.FindFirstValue(JwtClaimTypes.Id);
            
            return Ok();
        }
        
        [HttpPost]
        [Route("events")]
        public IActionResult CreateEvent([FromBody] Guid trackerId)
        {
            var userId = User.FindFirstValue(JwtClaimTypes.Id);

            return Ok();
        }
        
        [HttpGet]
        [Route("events/{eventId}")]
        public IActionResult GetEvent([FromQuery] Guid trackerId, [FromQuery] Guid eventId)
        {
            var userId = User.FindFirstValue(JwtClaimTypes.Id);

            return Ok();
        }

        [HttpPut]
        [Route("events/{eventId}")]
        IActionResult UpdateEvent([FromQuery] Guid trackerId, [FromQuery] Guid eventId)
        {
            var userId = User.FindFirstValue(JwtClaimTypes.Id);

            return Ok();
        }
        
        [HttpDelete]
        [Route("events/{eventId}")]
        IActionResult DeleteEvent([FromQuery] Guid trackerId, [FromQuery] Guid eventId)
        {
            var userId = User.FindFirstValue(JwtClaimTypes.Id);

            return Ok();
        }
    }
}