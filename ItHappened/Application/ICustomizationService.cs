using System;
using ItHappened.Domain;
using ItHappened.Domain.Customizations;
using LanguageExt;

namespace ItHappened.Application
{
    public interface ICustomizationService
    {
        void AddCommentToEvent(Guid actorId, Guid eventId, CommentForm form);
        void AddRatingToEvent(Guid actorId, Guid eventId, RatingForm form);
        void AddScaleToEvent(Guid actorId, Guid eventId, ScaleForm form);
        void AddGeotagToEvent(Guid actorId, Guid eventId, GeotagForm form);
        void AddPhotoToEvent(Guid actorId, Guid eventId, PhotoForm form);

        Option<Comment> GetComment(Guid actorId, Guid eventId);
        Option<Rating> GetRating(Guid actorId, Guid eventId);
        Option<Scale> GetScale(Guid actorId, Guid eventId);
        Option<Geotag> GetGeotag(Guid actorId, Guid eventId);
        Option<Photo> GetPhoto(Guid actorId, Guid eventId);
    }
}