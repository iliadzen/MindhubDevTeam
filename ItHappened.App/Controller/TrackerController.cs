using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.App.Controller
{
    // [Authorize]
    [Route("trackers")]
    public class TrackerController : ControllerBase
    {
        // Tracker controllers
        [HttpGet]
        [Route("")]
        IActionResult GetTrackers()
        {
            return Ok();
        }
        
        [HttpPost]
        [Route("")]
        IActionResult CreateTracker()
        {
            return Ok();
        }
        
        [HttpGet]
        [Route("{trackerId}")]
        IActionResult GetTracker([FromQuery]string trackerId)
        {
            return Ok();
        }

        [HttpPut]
        [Route("{trackerId}")]
        IActionResult UpdateTracker([FromQuery]string trackerId)
        {
            return Ok();
        }
        
        [HttpDelete]
        [Route("{trackerId}")]
        IActionResult DeleteTracker([FromQuery]string trackerId)
        {
            return Ok();
        }
        
        // TrackerEventControllers
        
        [HttpGet]
        [Route("{trackerId}/events")]
        IActionResult GetEvents()
        {
            return Ok();
        }
        
        [HttpPost]
        [Route("{trackerId}/events")]
        IActionResult CreateEvent()
        {
            return Ok();
        }
        
        [HttpGet]
        [Route("{trackerId}/events/{eventId}")]
        IActionResult GetEvent([FromQuery]string trackerId, [FromQuery]string eventId)
        {
            return Ok();
        }

        [HttpPut]
        [Route("{trackerId}/events/{eventId}")]
        IActionResult UpdateEvent([FromQuery]string trackerId, [FromQuery]string eventId)
        {
            return Ok();
        }
        
        [HttpDelete]
        [Route("{trackerId}/events/{eventId}")]
        IActionResult DeleteEvent([FromQuery]string trackerId, [FromQuery]string eventId)
        {
            return Ok();
        }
    }
}