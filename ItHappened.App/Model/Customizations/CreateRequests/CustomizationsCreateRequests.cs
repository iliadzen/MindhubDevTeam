namespace ItHappened.App.Model
{
    public class CustomizationsCreateRequests
    {
        public GeotagCreateRequest Geotag { get; set; }
        public CommentCreateRequest Comment { get; set; }
        public ScaleCreateRequest Scale { get; set; }
        public RatingCreateRequest Rating { get; set; }
    }
}