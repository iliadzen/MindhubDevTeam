using Serilog;

namespace ItHappened.Application
{
    public class GeotagForm : IForm
    {
        public decimal Longitude { get; }
        public decimal Latitude { get; }

        public GeotagForm(decimal longitude, decimal latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        public bool IsCorrectlyFilled()
        {
            if (!(Longitude == 0 && Latitude == 0))
                return true;
            Log.Error("Geotag form filled incorrect: null coordinates");
            return false;
        }
    }
}