namespace ItHappened.Domain
{
    public class Scale : EventCustomizationData
    {
        public double Value { get; }

        public Scale(double value)
        {
            Value = value;
        }
    }
}