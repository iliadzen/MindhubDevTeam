using System;

namespace ItHappened.Domain
{
    public class User
    {
        public Guid Id { get; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public License License { get; set; }
        public DateTime CreationDate { get; }
        public DateTime ModificationDate { get; set; }

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