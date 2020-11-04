using System;
using System.Collections.Generic;
using System.Security.Claims;
using ItHappened.App.Authentication;
using ItHappened.App.Model;
using ItHappened.Application;
using ItHappened.Domain.Customizations;
using LanguageExt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ItHappened.App.Controller
{
    [Authorize]
    [Route("trackers/{trackerId}")]
    public class EventController : ControllerBase
    {
        public EventController(IEventService eventService, ICustomizationService customizationService)
        {
            _eventService = eventService;
            _customizationService = customizationService;
        }
        
        [HttpPost]
        [Route("events")]
        public IActionResult CreateEvent([FromRoute] Guid trackerId, [FromBody] EventCreateRequest request)
        {
            var actorId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var form = new EventForm(request.Title);
            var eventId = _eventService.CreateEvent(actorId, trackerId, form);
            if (eventId != Guid.Empty)
            {
                if (!request.Customizations.IsNull())
                    AddCustomizationsToEvent(actorId, trackerId, eventId, request.Customizations);
            }
            else
                return Ok("Tracker doesn't exists or no permissions to create.");
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
                    var form = new EventForm(request.Title);
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

        private void AddCustomizationsToEvent(Guid actorId, Guid trackerId, Guid eventId,
            CustomizationsCreateRequests createRequests)
        {
            if(!createRequests.CommentCreateRequest.IsNull())
                _customizationService.AddCommentToEvent(actorId, eventId, 
                    new CommentForm(createRequests.CommentCreateRequest.Content));
            
            if(!createRequests.RatingCreateRequest.IsNull())
                _customizationService.AddRatingToEvent(actorId, eventId,
                    new RatingForm(createRequests.RatingCreateRequest.Stars));
            
            if(!createRequests.ScaleCreateRequest.IsNull())
                _customizationService.AddScaleToEvent(actorId, eventId, 
                    new ScaleForm(createRequests.ScaleCreateRequest.Value));
                
            if(!createRequests.GeotagCreateRequest.IsNull())
                _customizationService.AddGeotagToEvent(actorId, eventId,
                    new GeotagForm(
                        createRequests.GeotagCreateRequest.Longitude, 
                        createRequests.GeotagCreateRequest.Latitude));
        }

        private readonly IEventService _eventService;
        private readonly ICustomizationService _customizationService;
    }
}