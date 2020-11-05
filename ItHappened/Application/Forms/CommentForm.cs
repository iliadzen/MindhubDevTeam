using Serilog;

namespace ItHappened.Application
{
    public class CommentForm : IForm
    {
        public string Content { get; }

        public CommentForm(string content)
        {
            Content = content;
        }

        public bool IsCorrectlyFilled()
        {
            if (!string.IsNullOrEmpty(Content))
                return true;
            Log.Error($"Comment form filled incorrectly: string is null or empty.");
            return false;
        }
    }
}