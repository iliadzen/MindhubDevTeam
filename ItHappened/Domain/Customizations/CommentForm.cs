namespace ItHappened.Domain.Customizations
{
    public class CommentForm
    {
        public string Content { get; }

        public CommentForm(string content)
        {
            Content = content;
        }
    }
}