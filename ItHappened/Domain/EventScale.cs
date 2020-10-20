namespace ItHappened.Domain
{
    public class EventScale : EventCustomizationData
    {
        public double Scale { get; }

        public EventScale(double scale)
        {
            Scale = scale;
        }
    }
}