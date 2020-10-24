using System;

namespace ItHappened.Domain
{
    public class User : IEntity
    {
        public Guid Id { get; }
        public string Username { get; }
        public string PasswordHash { get; }
        public License License { get; }
        public DateTime CreationDate { get; }
        public DateTime ModificationDate { get; }

        public User(Guid id, string username, string passwordHash, License license,
            DateTime creationDate, DateTime modificationDate)
        {
            Id = id;
            Username = username ?? throw new ArgumentNullException(nameof(username));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            License = license;
            CreationDate = creationDate;
            ModificationDate = modificationDate;
        }
    }
}