namespace ItHappened.App.Model
{
    public class PhotoGetResponse
    {
        public string DataUrl { get; }

        public PhotoGetResponse(string dataUrl)
        {
            DataUrl = dataUrl;
        }
    }
}