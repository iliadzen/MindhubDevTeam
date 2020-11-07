namespace ItHappened.App.Model
{
    public class CustomizationsGetResponses
    {
        public GeotagGetResponse Geotag { get; set; }
        public CommentGetResponse Comment { get; set; }
        public ScaleGetResponse Scale { get; set; }
        public RatingGetResponse Rating { get; set; }
        public PhotoGetResponse Photo { get; set; }
    }
}