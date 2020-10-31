using System;

namespace ItHappened.Domain
{
    public class Comment : EventCustomizationData
    {
        public string Content { get; }
        public DateTime CreationDate { get; }

        public Comment(string content, DateTime creationDate)
        {
            Content = content;
            CreationDate = creationDate;
        }
    }
}