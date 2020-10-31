namespace ItHappened.Domain
{
    public class Rating : EventCustomizationData
    {
        public enum StarsRating
        {
            One = 1,
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5
        }

        public StarsRating Stars { get; }

        public Rating(StarsRating stars)
        {
            Stars = stars;
        }
    }
}