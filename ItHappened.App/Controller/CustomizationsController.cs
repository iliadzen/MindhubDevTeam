using System;
using System.Collections.Generic;
using System.Security.Claims;
using ItHappened.App.Authentication;
using ItHappened.App.Model;
using ItHappened.Application;
using ItHappened.Domain;
using ItHappened.Domain.Customizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.App.Controller
{
    [Authorize]
    [Route("trackers/{trackerId}")]
    public class CustomizationsController : ControllerBase
    {
        public CustomizationsController(ICustomizationService customizationService)
        {
            _customizationService = customizationService;
        }
        
        [HttpPost]
        [Route("customizations/comments")]
        public IActionResult AddCommentCustomizationToTracker([FromRoute] Guid trackerId)
        {
            var actorId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            _customizationService.AddTrackerCustomization(actorId, trackerId, CustomizationType.Comment);
            return Ok();
        }
        
        [HttpPost]
        [Route("events/{eventId}/comments")]
        public IActionResult AddCommentToEvent([FromRoute] Guid eventId, [FromBody] CommentCreateRequest request)
        {
            var actorId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var form = new CommentForm(request.Content);
            _customizationService.AddCommentToEvent(actorId, eventId, form);
            return Ok();
        }
        
        private readonly ICustomizationService _customizationService;
    }
}