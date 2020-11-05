namespace ItHappened.App.Model
{
    public class ScaleGetResponse
    {
        public decimal Value { get; }

        public ScaleGetResponse(decimal value)
        {
            Value = value;
        }
    }
}