namespace ItHappened.Domain
{
    public class Geotag : EventCustomizationData
    {
        public double Longitude { get; }
        public double Latitude { get; }

        public Geotag(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }
    }
}