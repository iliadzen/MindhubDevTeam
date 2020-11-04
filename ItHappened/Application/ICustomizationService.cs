using System;
using ItHappened.Domain;
using ItHappened.Domain.Customizations;

namespace ItHappened.Application
{
    public interface ICustomizationService
    {
        void AddCommentToEvent(Guid actorId, Guid eventId, CommentForm form);
        void AddRatingToEvent(Guid actorId, Guid eventId, RatingForm form);
        void AddScaleToEvent(Guid actorId, Guid eventId, ScaleForm form);
        void AddGeotagToEvent(Guid actorId, Guid eventId, GeotagForm form);
        //void AddPhotoToEvent(Guid actorId, Guid eventId, PhotoForm form);
    }
}