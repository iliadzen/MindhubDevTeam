namespace ItHappened.App.Model
{
    public class CustomizationsGetResponses
    {
        public GeotagGetResponse Geotag { get; set; }
        public CommentGetResponse Comment { get; set; }
        public ScaleGetResponse Scale { get; set; }
        public RatingGetResponse Rating { get; set; }
        
        /*
        public CustomizationsGetResponses(GeotagGetResponse geotag, CommentGetResponse comment,
            ScaleGetResponse scale, RatingGetResponse rating)
        {
            Geotag = geotag;
            Comment = comment;
            Scale = scale;
            Rating = rating;
        }
        */
    }
}