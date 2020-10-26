using System;
using ItHappened.Domain;
using LanguageExt;

namespace ItHappened.Application
{
    public class UserForm
    {
        public string Username { get; }
        public string Password { get; }

        public License License { get; }
        public UserForm(string username, string password, License license)
        {
            Username = username;
            Password = password;
            License = license;
        }
    }
}