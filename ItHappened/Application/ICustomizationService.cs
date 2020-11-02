using System;
using ItHappened.Domain;
using ItHappened.Domain.Customizations;

namespace ItHappened.Application
{
    public interface ICustomizationService
    {
        public void AddCommentToEvent(Guid actorId, Guid eventId, CommentForm form);
    }
}