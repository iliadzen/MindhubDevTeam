namespace ItHappened.App.Model
{
    public class RatingGetResponse
    {
        public int Stars { get; }
        
        public RatingGetResponse(int stars)
        {
            Stars = stars;
        }
    }
}