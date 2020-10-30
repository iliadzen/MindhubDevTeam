using System;
using System.Collections.Generic;
using System.Security.Claims;
using ItHappened.App.Authentication;
using ItHappened.App.Model;
using ItHappened.Application;
using ItHappened.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.App.Controller
{
    [Authorize]
    [Route("trackers")]
    public class TrackerController : ControllerBase
    {
        public TrackerController(ITrackerService trackerService, IJwtIssuer jwtIssuer)
        {
            _trackerService = trackerService;
        }
        
        [Authorize]
        [HttpGet]
        [Route("{trackerId}")]
        public IActionResult GetTracker([FromRoute] Guid trackerId)
        {
            var actorId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var optionTracker = _trackerService.GetTracker(actorId, trackerId);
            return optionTracker.Match<IActionResult>(
                Some: tracker =>
                {
                    var trackerGetResponse = new TrackerGetResponse(tracker);
                    return Ok(trackerGetResponse);
                },
                None: NotFound("Tracker doesn't exist or no permissions to get"));
        }
        
        [Authorize]
        [HttpPost]
        [Route("")]
        public IActionResult CreateTracker([FromBody] TrackerCreateRequest request)
        {
            var actorId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var form = new TrackerForm(request.Title, new HashSet<CustomizationType>());
            _trackerService.CreateTracker(actorId, form);
            return Ok();
        }
        
        [Authorize]
        [HttpGet]
        [Route("")]
        public IActionResult GetTrackers()
        {
            var actorId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var trackers = _trackerService.GetUserTrackers(actorId);
            var response = new List<TrackerGetResponse>();
            foreach (var tracker in trackers)
                response.Add(new TrackerGetResponse(tracker));
            return Ok(response);
        }

        [Authorize]
        [HttpPut]
        [Route("{trackerId}")]
        public IActionResult UpdateTracker([FromRoute] Guid trackerId, [FromBody] TrackerCreateRequest request)
        {
            var actorId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var optionTracker = _trackerService.GetTracker(actorId, trackerId);
            return optionTracker.Match<IActionResult>(
                Some: tracker =>
                {
                    var form = new TrackerForm(request.Title, new HashSet<CustomizationType>());
                    _trackerService.EditTracker(actorId, trackerId, form);
                    return Ok();
                },
                None: NotFound("Tracker doesn't exist or no permissions to edit"));
        }
        
        [Authorize]
        [HttpDelete]
        [Route("{trackerId}")]
        public IActionResult DeleteTracker([FromRoute] Guid trackerId)
        {
            var actorId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var optionTracker = _trackerService.GetTracker(actorId, trackerId);
            return optionTracker.Match<IActionResult>(
                Some: tracker =>
                {
                    _trackerService.DeleteTracker(actorId, trackerId);
                    return Ok();
                },
                None: NotFound("Tracker doesn't exist or no permissions to delete"));
        }
        
        private readonly ITrackerService _trackerService;
    }
}