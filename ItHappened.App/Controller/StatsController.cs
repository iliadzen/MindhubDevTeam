using System;
using System.Security.Claims;
using ItHappened.App.Authentication;
using ItHappened.Application;
using ItHappened.Domain;
using LanguageExt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.App.Controller
{
    [Authorize]
    [Route("stats")]
    public class StatsController : ControllerBase
    {
        private StatsService _statsService;
        private TrackerService _trackerService;

        public StatsController(StatsService statsService, TrackerService trackerService)
        {
            _statsService = statsService;
            _trackerService = trackerService;
        }

        [HttpGet]
        public IActionResult GetStatsForCurrentUser()
        {
            Guid userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            return Ok(_statsService.GetStatsFactsForUser(userId));
        }
        
        [HttpGet]
        [Route("{trackerId}")]
        public IActionResult GetStatsForTracker([FromRoute] Guid trackerId)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            Option<Tracker> tracker = _trackerService.GetTracker(userId, trackerId);
            return tracker.Match<IActionResult>(
                Some: someTracker => Ok(_statsService.GetStatsFactsForTracker(someTracker.Id)),
                None: NotFound());
        }
    }
}