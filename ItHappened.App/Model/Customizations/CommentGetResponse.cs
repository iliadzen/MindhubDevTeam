using System;

namespace ItHappened.App.Model
{
    public class CommentGetResponse : ICustomizationGetResponse
    {
        public string Content { get; }
        public DateTime CreationDate { get; }

        public CommentGetResponse(string content, DateTime creationDate)
        {
            Content = content;
            CreationDate = creationDate;
        }
    }
}