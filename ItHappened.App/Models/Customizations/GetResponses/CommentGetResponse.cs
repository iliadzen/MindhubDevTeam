using System;

namespace ItHappened.App.Model
{
    public class CommentGetResponse
    {
        public string Content { get; }

        public CommentGetResponse(string content)
        {
            Content = content;
        }
    }
}