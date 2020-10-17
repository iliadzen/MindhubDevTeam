using System;
using LanguageExt;

namespace ItHappened.Application
{
    public class UserForm
    {
        public Option<Guid> Id { get; set; }
        public Option<string> Username { get; set; }
        public Option<string> Password { get; set; }
    }
}