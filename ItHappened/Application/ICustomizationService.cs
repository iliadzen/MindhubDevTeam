using System;
using ItHappened.Domain;
using ItHappened.Domain.Customizations;

namespace ItHappened.Application
{
    public interface ICustomizationService
    {
        void AddCommentToEvent(Guid actorId, Guid eventId, CommentForm form);
        void AddTrackerCustomization(Guid actorId, Guid trackerId, CustomizationType type);
    }
}