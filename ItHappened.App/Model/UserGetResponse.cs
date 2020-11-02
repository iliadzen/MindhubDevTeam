using System;
using ItHappened.Domain;

namespace ItHappened.App.Model
{
    public class UserGetResponse
    {
        public Guid Id { get; }
        public string Username { get; }
        public DateTime CreationDate { get; }
        public DateTime ModificationDate { get; }

        public UserGetResponse(User user)
        {
            Id = user.Id;
            Username = user.Username;
            CreationDate = user.CreationDate;
            ModificationDate = user.ModificationDate;
        }
    }
}