namespace ItHappened.Application
{
    public class EventForm
    {
        public string Title { get; }

        public EventForm(string title)
        {
            Title = title;
        }

        public bool IsNull()
        {
            return Title == null;
        }
    }
}