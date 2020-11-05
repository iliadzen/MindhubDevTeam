using Serilog;

namespace ItHappened.Application
{
    public class EventForm : IForm
    {
        public string Title { get; }

        public EventForm(string title)
        {
            Title = title;
        }

        public bool IsCorrectlyFilled()
        {
            if (!string.IsNullOrEmpty(Title))
                return true;
            Log.Error($"Event form filled incorrectly: string is null or empty.");
            return false;
        }
    }
}