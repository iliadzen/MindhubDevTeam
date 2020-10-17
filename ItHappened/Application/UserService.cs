using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Infrastructure;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using Serilog;

namespace ItHappened.Application
{
    public class UserService
    {
        private IRepository<User> _userRepository;
        private IHasher _hasher;

        public UserService(IRepository<User> userRepository, IHasher hasher)
        {
            _userRepository = userRepository;
            _hasher = hasher;
        }

        public IEnumerable<User> GetAll() =>
            _userRepository.GetAll();

        public Option<User> GetById(Guid userId) =>
            _userRepository.Get(userId);

        public Option<User> GetByUsername(string username) =>
            _userRepository.GetAll().SingleOrDefault(user => user.Username == username);

        public Option<User> CreateUser(UserForm userForm)
        {
            string intent = $"create new user named '{userForm.Username}'";

            if (userForm.Username.IsNone || userForm.Password.IsNone)
            {
                Log.Information($"Failed: {intent} - form incomplete");
                return Option<User>.None;
            }
            if (GetByUsername(userForm.Username.ValueUnsafe()).IsSome)
            {
                Log.Information($"Failed: {intent} - username taken");
                return Option<User>.None;
            }
            User user = new User(Guid.NewGuid(),
                userForm.Username.ValueUnsafe(), _hasher.MakeSaltedHash(userForm.Password.ValueUnsafe()),
                new License(LicenseType.Free, DateTime.MaxValue), DateTime.Now, DateTime.Now);
            
            _userRepository.Save(user);
            Log.Information($"Success: {intent}");
            return user;
        }
        
        public Option<User> EditUser(UserForm userForm)
        {
            string intent = $"edit user '{userForm.Id}'";
            if (userForm.Id.IsNone)
            {
                Log.Information($"Failed: {intent} - form incomplete");
                return Option<User>.None;
            }
            Option<User> user = GetById(userForm.Id.ValueUnsafe());
            user.Do(u =>
            {
                _userRepository.Save(u);
                Log.Information($"Success: {intent}");
            }); 
            return user;
        }

        public void SetPassword(Guid principalId, Guid userId, string oldPassword, string newPassword)
        {
            string intent = $"modify password for user '{userId}'";
            Option<User> modifiedUser = GetById(userId);
            if (!modifiedUser.Exists(user => 
                    user.Id == principalId 
                    && _hasher.VerifySaltedHash(oldPassword, user.PasswordHash)))
                Log.Information($"Access denied: {intent}");

            modifiedUser
                .Do(user => user.PasswordHash = _hasher.MakeSaltedHash(newPassword))
                .Do(user => user.ModificationDate = DateTime.Now);
            Log.Information($"Success: {intent}");
        }

        public Option<User> LogInByCredentials(string username, string password)
        {
            string intent = $"log in as user named '{username}'";
            Option<User> user = GetByUsername(username);
            if (!user.Exists(u => _hasher.VerifySaltedHash(password, u.PasswordHash)))
            {
                Log.Information($"Access denied: {intent}");
                return Option<User>.None;
            }
            return user;
        }
    }
}