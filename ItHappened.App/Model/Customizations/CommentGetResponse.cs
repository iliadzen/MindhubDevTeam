using System;

namespace ItHappened.App.Model
{
    public class CommentGetResponse
    {
        public Guid Id { get; }
        public string Content { get; }
        public DateTime CreationDate { get; }

        public CommentGetResponse(Guid id, string content, DateTime creationDate)
        {
            Id = id;
            Content = content;
            CreationDate = creationDate;
        }
    }
}