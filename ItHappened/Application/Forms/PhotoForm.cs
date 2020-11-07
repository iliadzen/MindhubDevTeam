using Serilog;

namespace ItHappened.Application
{
    public class PhotoForm : IForm
    {
        public string DataUrl { get; }

        public PhotoForm(string dataUrl)
        {
            DataUrl = dataUrl;
        }

        public bool IsCorrectlyFilled()
        {
            if (!string.IsNullOrEmpty(DataUrl))
                return true;
            Log.Error($"Photo form filled incorrectly: string is null or empty.");
            return false;
        }
    }
}