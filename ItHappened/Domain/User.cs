using System;

namespace ItHappened.Domain
{
    public class User : IEntity
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }

        public User(Guid id, string username, string passwordHash,
            DateTime creationDate, DateTime modificationDate)
        {
            Id = id;
            Username = username ?? throw new ArgumentNullException(nameof(username));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            CreationDate = creationDate;
            ModificationDate = modificationDate;
        }

        public User()
        {
        }
    }
}