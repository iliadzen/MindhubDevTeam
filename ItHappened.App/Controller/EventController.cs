using System;
using System.Collections.Generic;
using System.Security.Claims;
using ItHappened.App.Authentication;
using ItHappened.App.Model;
using ItHappened.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.App.Controller
{
    [Authorize]
    [Route("trackers/{trackerId}")]
    public class EventController : ControllerBase
    {
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }
        
        [HttpPost]
        [Route("events")]
        public IActionResult CreateEvent([FromRoute] Guid trackerId, [FromBody] EventCreateRequest request)
        {
            var actorId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var form = new EventContent(request.Title);
            _eventService.CreateEvent(actorId, trackerId, form);
            return Ok();
        }
        
        [HttpGet]
        [Route("events")]
        public IActionResult GetEvents([FromRoute] Guid trackerId)
        {
            var actorId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var events = _eventService.GetEventsByTrackerId(actorId, trackerId);
            var response = new List<EventGetResponse>();
            foreach (var @event in events)
                response.Add(new EventGetResponse(@event, new List<ICustomizationGetResponse>()));
            
            return Ok(response);
        }

        [HttpGet]
        [Route("events/{eventId}")]
        public IActionResult GetEvent([FromRoute] Guid eventId)
        {
            var actorId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var optionEvent = _eventService.GetEvent(actorId, eventId);
            return optionEvent.Match<IActionResult>(
                Some: @event =>
                {
                    var response = new EventGetResponse(@event, new List<ICustomizationGetResponse>());
                    return Ok(response);
                },
                None: Ok(new
                    {
                        errors = new
                        {
                            commonError = "Event doesn't exist or no permissions to get."
                        }
                    }
                ));
        }
        
        [HttpPut]
        [Route("events/{eventId}")]
        public IActionResult UpdateEvent([FromRoute] Guid eventId, [FromBody] EventCreateRequest request)
        {
            var actorId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var optionEvent = _eventService.GetEvent(actorId, eventId);
            return optionEvent.Match<IActionResult>(
                Some: tracker =>
                {
                    var form = new EventContent(request.Title);
                    _eventService.EditEvent(actorId, eventId, form);
                    return Ok();
                },
                None: Ok(new
                    {
                        errors = new
                        {
                            commonError = "Event doesn't exist or no permissions to edit."
                        }
                    }
                ));
        }
        
        [HttpDelete]
        [Route("events/{eventId}")]
        public IActionResult DeleteEvent([FromRoute] Guid eventId)
        {
            var actorId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var optionEvent = _eventService.GetEvent(actorId, eventId);
            return optionEvent.Match<IActionResult>(
                Some: @event =>
                {
                    _eventService.DeleteEvent(actorId, eventId);
                    return Ok();
                },
                None: Ok(new
                    {
                        errors = new
                        {
                            commonError = "Event doesn't exist or no permissions to delete."
                        }
                    }
                ));
        }

        private readonly IEventService _eventService;
    }
}