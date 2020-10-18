using System;
using LanguageExt;

namespace ItHappened.Domain
{
    public class EventCustomizationData
    {
        public Guid EventId { get; }
        public Option<Photo> Photo { get; }
        public Option<Comment> Comment { get; }
        public Option<Geotag> Geotag { get; }
        public EventRating Rating { get; }
        public double Scale { get; }
        

        public EventCustomizationData(Guid eventId, Photo photo, Comment comment, Geotag geotag, 
            EventRating rating, double scale)
        {
            if(photo != null)
                Photo = Option<Photo>.Some(photo);
            if(comment != null)
                Comment = Option<Comment>.Some(comment);
            if(geotag != null)
                Geotag = Option<Geotag>.Some(geotag);

            Rating = rating;
            Scale = scale;
            EventId = eventId;
        }
    }
}