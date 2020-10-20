using System;

namespace ItHappened.Domain
{
    public class EventComment : EventCustomizationData
    {
        public string Content { get; }
        public DateTime CreationDate { get; }

        public EventComment(string content, DateTime creationDate)
        {
            Content = content;
            CreationDate = creationDate;
        }
    }
}