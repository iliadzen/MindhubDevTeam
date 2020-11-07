using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ItHappened.App.Authentication;
using ItHappened.App.Model;
using ItHappened.Application;
using ItHappened.Domain.Stats;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ItHappened.App.Controllers
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
            if (eventId == Guid.Empty)
                return NotFound(new
                {
                    errors = new
                    {
                        commonError = $"Tracker {trackerId} doesn't exists or no permissions to create."
                    }
                });
            if (!request.Customizations.IsNull())
                    AddCustomizationsToEvent(actorId, trackerId, eventId, request.Customizations);
            
            return NoContent();
        }
        
        [HttpGet]
        [Route("events")]
        public IActionResult GetEvents([FromRoute] Guid trackerId, 
            [FromQuery] string period, [FromQuery] string fromDate, [FromQuery] string toDate,
            [FromQuery] int ratingMin, [FromQuery] int ratingMax,
            [FromQuery] decimal scaleMin, [FromQuery] decimal scaleMax)
        {
            var actorId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var events = _eventService.GetEventsByTrackerId(actorId, trackerId);
            
            if(!period.IsNull())
                if (period == "thisWeek")
                    events = events.Where(@event => @event.CreationDate > DateTime.Now.GetLast(DayOfWeek.Monday))
                        .ToList().AsReadOnly();
            
            //Date filter
            if (!fromDate.IsNull())
                events = events.Where(@event => @event.CreationDate > DateTime.Parse(fromDate))
                    .ToList().AsReadOnly();
            if(!toDate.IsNull())
                events = events.Where(@event => @event.CreationDate < DateTime.Parse(toDate))
                    .ToList().AsReadOnly();
            
            //Rating filter
            if(ratingMin != 0)
                events = events.Where(@event => CompareEventRating(actorId, @event.Id, ratingMin, 
                        (x, y) => x >= y))
                .ToList().AsReadOnly();
            if(ratingMax != 0)
                events = events.Where(@event => CompareEventRating(actorId, @event.Id, ratingMax,
                        (x, y) => x <= y))
                    .ToList().AsReadOnly();
            
            //Scale filter
            if (scaleMin != 0)
                events = events.Where(@event => CompareEventScale(actorId, @event.Id, scaleMin,
                        (x, y) => x >= y))
                    .ToList().AsReadOnly();
            if (scaleMax != 0)
                events = events.Where(@event => CompareEventScale(actorId, @event.Id, scaleMax,
                        (x, y) => x <= y))
                    .ToList().AsReadOnly();

            var response = new List<EventGetResponse>();
            foreach (var @event in events)
                response.Add(new EventGetResponse(@event, 
                    FillCustomizationsGetResponses(actorId, @event.Id)));
            
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
                    var response = new EventGetResponse(@event, 
                        FillCustomizationsGetResponses(actorId, eventId));
                    return Ok(response);
                },
                None: NotFound(new
                    {
                        errors = new
                        {
                            commonError = "Event doesn't exist."
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
                    return NoContent();
                },
                None: NotFound(new
                    {
                        errors = new
                        {
                            commonError = "Event doesn't exist."
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
                    return NoContent();
                },
                None: NotFound(new
                    {
                        errors = new
                        {
                            commonError = "Event doesn't exist."
                        }
                    }
                ));
        }

        private void AddCustomizationsToEvent(Guid actorId, Guid trackerId, Guid eventId,
            CustomizationsCreateRequests createRequests)
        {
            if(!createRequests.Comment.IsNull())
                _customizationService.AddCommentToEvent(actorId, eventId, 
                    new CommentForm(createRequests.Comment.Content));
            
            if(!createRequests.Rating.IsNull())
                _customizationService.AddRatingToEvent(actorId, eventId,
                    new RatingForm(createRequests.Rating.Stars));
            
            if(!createRequests.Scale.IsNull())
                _customizationService.AddScaleToEvent(actorId, eventId, 
                    new ScaleForm(createRequests.Scale.Value));
                
            if(!createRequests.Geotag.IsNull())
                _customizationService.AddGeotagToEvent(actorId, eventId,
                    new GeotagForm(
                        createRequests.Geotag.Longitude, 
                        createRequests.Geotag.Latitude));
            
            if(!createRequests.Photo.IsNull())
                _customizationService.AddPhotoToEvent(actorId, eventId,
                    new PhotoForm(createRequests.Photo.DataUrl));
        }

        private CustomizationsGetResponses FillCustomizationsGetResponses(Guid actorId, Guid eventId)
        {
            var response = new CustomizationsGetResponses();
            var optionComment = _customizationService.GetComment(actorId, eventId);
            var optionRating = _customizationService.GetRating(actorId, eventId);
            var optionScale = _customizationService.GetScale(actorId, eventId);
            var optionGeotag = _customizationService.GetGeotag(actorId, eventId);
            var optionPhoto = _customizationService.GetPhoto(actorId, eventId);

            optionComment.Do(comment => response.Comment = new CommentGetResponse(comment.Content));
            optionRating.Do(rating => response.Rating = new RatingGetResponse((int)rating.Stars));
            optionScale.Do(scale => response.Scale = new ScaleGetResponse(scale.Value));
            optionGeotag.Do(tag => response.Geotag = 
                new GeotagGetResponse(tag.Longitude, tag.Latitude));
            optionPhoto.Do(photo => response.Photo = new PhotoGetResponse(photo.Image));
            return response;
        }

        private bool CompareEventRating(Guid actorId, Guid eventId, int threshold, Func<int, int, bool> Compare)
        {
            var optionRating = _customizationService.GetRating(actorId, eventId);
            if (optionRating.IsNone) return false;
            return Compare((int) optionRating.ValueUnsafe().Stars, threshold);
        }
        
        private bool CompareEventScale(Guid actorId, Guid eventId, decimal threshold, 
            Func<decimal, decimal, bool> Compare)
        {
            var optionScale = _customizationService.GetScale(actorId, eventId);
            if (optionScale.IsNone) return false;
            return Compare(optionScale.ValueUnsafe().Value, threshold);
        }

        private readonly IEventService _eventService;
        private readonly ICustomizationService _customizationService;
    }
}