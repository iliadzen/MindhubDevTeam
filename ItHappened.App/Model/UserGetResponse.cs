using System;
using ItHappened.Domain;

namespace ItHappened.App.Model
{
    public class UserGetResponse
    {
        public Guid Id { get; }
        public string Username { get; }
        public License License { get; }
        public DateTime CreationDate { get; }
        public DateTime ModificationDate { get; }

        public UserGetResponse(User user)
        {
            Id = user.Id;
            Username = user.Username;
            License = user.License;
            CreationDate = user.CreationDate;
            ModificationDate = user.ModificationDate;
        }
    }
}