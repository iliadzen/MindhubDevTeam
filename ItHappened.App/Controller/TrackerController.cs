using System;
using System.Security.Claims;
using ItHappened.App.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.App.Controller
{
    [Authorize]
    [Route("trackers")]
    public class TrackerController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public IActionResult GetTrackers()
        {
            var userId = User.FindFirstValue(JwtClaimTypes.Id);

            return Ok();
        }
        
        [HttpPost]
        [Route("")]
        IActionResult CreateTracker()
        {
            var userId = User.FindFirstValue(JwtClaimTypes.Id);

            return Ok();
        }
        
        [HttpGet]
        [Route("{trackerId}")]
        IActionResult GetTracker([FromQuery] Guid trackerId)
        {
            var userId = User.FindFirstValue(JwtClaimTypes.Id);

            return Ok();
        }

        [HttpPut]
        [Route("{trackerId}")]
        IActionResult UpdateTracker([FromQuery] Guid trackerId)
        {
            var userId = User.FindFirstValue(JwtClaimTypes.Id);

            return Ok();
        }
        
        [HttpDelete]
        [Route("{trackerId}")]
        IActionResult DeleteTracker([FromQuery] Guid trackerId)
        {
            var userId = User.FindFirstValue(JwtClaimTypes.Id);

            return Ok();
        }
    }
}