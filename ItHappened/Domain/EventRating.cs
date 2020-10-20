namespace ItHappened.Domain
{
    public class EventRating : EventCustomizationData
    {
        public enum StarsRating
        {
            One = 1,
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5
        }

        public StarsRating Rating { get; }

        public EventRating(StarsRating rating)
        {
            Rating = rating;
        }
    }
}