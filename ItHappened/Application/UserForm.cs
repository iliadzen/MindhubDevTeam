using ItHappened.Domain;

namespace ItHappened.Application
{
    public class UserForm
    {
        public string Username { get; }
        public string Password { get; }
        
        public UserForm(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}