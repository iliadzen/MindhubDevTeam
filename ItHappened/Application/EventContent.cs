namespace ItHappened.Application
{
    public class EventContent
    {
        public string Title { get; }

        public EventContent(string title)
        {
            Title = title;
        }

        public bool IsNull()
        {
            return Title == null;
        }
    }
}