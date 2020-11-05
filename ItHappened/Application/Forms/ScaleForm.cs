namespace ItHappened.Application
{
    public class ScaleForm : IForm
    {
        public decimal Scale { get; }

        public ScaleForm(decimal scale)
        {
            Scale = scale;
        }

        public bool IsCorrectlyFilled()
        {
            return true;
        }
    }
}