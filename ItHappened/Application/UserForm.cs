using System;
using LanguageExt;

namespace ItHappened.Application
{
    public class UserForm
    {
        public Option<Guid> Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}