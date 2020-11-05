using ItHappened.Domain.Customizations;
using Serilog;

namespace ItHappened.Application
{
    public class RatingForm : IForm
    {
        public int Stars { get; }

        public RatingForm(int stars)
        {
            Stars = stars;
        }

        public bool IsCorrectlyFilled()
        {
            if (!(Stars < 1 || Stars > 5))
                return true;
            Log.Error($"Rating form filled incorrectly: stars are not in 1 to 5 range.");
            return false;
        }
    }
}