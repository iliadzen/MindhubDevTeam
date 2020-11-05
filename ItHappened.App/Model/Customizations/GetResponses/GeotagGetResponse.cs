namespace ItHappened.App.Model
{
    public class GeotagGetResponse
    {
        public decimal Longitude { get; }
        public decimal Latitude { get; }

        public GeotagGetResponse(decimal longitude, decimal latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }
    }
}