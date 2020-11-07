using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ItHappened.App.Authentication;
using ItHappened.Application;
using ItHappened.Domain;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.App.Controllers
{
    [Authorize]
    [Route("stats")]
    public class StatsController : ControllerBase
    {
        public StatsController(ITrackerService trackerService, IEventService eventService, 
            ICustomizationService customizationService)
        {
            _trackerService = trackerService;
            _eventService = eventService;
            _customizationService = customizationService;
        }

        [HttpGet]
        public IActionResult GetStatsForCurrentUser()
        {
            Guid actorId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var trackers = _trackerService.GetUserTrackers(actorId);
            var events = new List<Event>();
            foreach (var tracker in trackers)
                events = events.Concat(_eventService.GetEventsByTrackerId(actorId, tracker.Id)).ToList();
            return Ok(StatsService.GetStatsFactsForUser(events));
        }
        
        [HttpGet]
        [Route("{trackerId}")]
        public IActionResult GetStatsForTracker([FromRoute] Guid trackerId)
        {
            Guid actorId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var eventsWithRating = new Dictionary<Event, int>();
            var events = _eventService.GetEventsByTrackerId(actorId, trackerId);
            foreach (var @event in events)
            {
                var rating = _customizationService.GetRating(actorId, @event.Id);
                if(rating.IsSome)
                    eventsWithRating.Add(@event, (int)rating.ValueUnsafe().Stars);
            }

            return Ok(StatsService.GetStatsFactsForTracker(eventsWithRating));
        }

        private readonly ICustomizationService _customizationService;
        private readonly ITrackerService _trackerService;
        private readonly IEventService _eventService;
    }
}