using System;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Infrastructure;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using Serilog;

namespace ItHappened.Application
{
    public class UserService : IUserService
    {
        public UserService(IRepository<User> userRepository, IRepository<License> licenseRepository, IHasher hasher)
        {
            _userRepository = userRepository;
            _licenseRepository = licenseRepository;
            //_trackerRepository = trackerRepository;
            _hasher = hasher;
        }

        public Option<User> GetUserById(Guid actorId, Guid userId)
        {
            if (actorId == userId)
                return _userRepository.Get(userId);
            Log.Error($"User {actorId} tried to get {userId} account");
            return Option<User>.None;
        }

        public Option<User> GetUserByUsername(string username)
        {
            var users = _userRepository.GetAll().Where(user => user.Username == username);
            if (users.Count() == 0)
            {
                Log.Error($"User {username} not exists");
                return Option<User>.None;
            }

            if (users.Count() == 2)
            {
                Log.Error($"Several users with same username {username} exists");
                return Option<User>.None;
            }

            return users.FirstOrDefault();
        }

        public void CreateUser(UserForm form)
        {
            if (CheckFormIsComplete(form, "Creating"))
            {
                var user = new User(Guid.NewGuid(),
                    form.Username, _hasher.MakeSaltedHash(form.Password),DateTime.Now, DateTime.Now);
                var license = new License(Guid.NewGuid(), user.Id, LicenseType.Free, DateTime.MaxValue);
                
                _userRepository.Save(user);
                _licenseRepository.Save(license);
                Log.Information($"User {form.Username} with ID {user.Id} created");
            }
        }
        
        public void EditUser(Guid actorId, Guid userId, UserForm form)
        {
            if (actorId != userId)
            {
                Log.Error($"User {actorId} tried to edit {userId} account");
                return;
            }
            
            if (CheckFormIsComplete(form, "Editing"))
            {
                var oldUser = _userRepository.Get(userId);
                oldUser.Do(user =>
                {
                    user.Username = form.Username;
                    user.PasswordHash = _hasher.MakeSaltedHash(form.Password);
                    user.ModificationDate = DateTime.Now;
                    _userRepository.Update(user);
                    Log.Information($"User {form.Username} with ID {user.Id} updated");
                });
            }
        }

        public void DeleteUser(Guid actorId, Guid userId)
        {
            if (actorId != userId)
            {
                Log.Error($"User {actorId} tried to delete {userId} account");
                return;
            }
            _userRepository.Delete(userId);
        }

        public Option<User> LogInByCredentials(string username, string password)
        {
            
            var user = GetUserByUsername(username);
            if (user.IsNone)
                return Option<User>.None;

            if (!_hasher.VerifySaltedHash(password, user.ValueUnsafe().PasswordHash))
            {
                Log.Information($"{username} inserted wrong password");
                return Option<User>.None;
            }
            return user;
        }

        private bool CheckFormIsComplete(UserForm form, string actionWithForm)
        {
            if (!form.IsNull())
            {
                if (form.Username.IsNull() || form.Password.IsNull())
                {
                    Log.Error($"{actionWithForm} user failed: form incomplete"); 
                    return false;
                }
                
                if (GetUserByUsername(form.Username).IsSome)
                { 
                    Log.Error($"{actionWithForm} user failed: username {form.Username} taken"); 
                    return false; 
                }
                return true;
            }
            Log.Error($"{actionWithForm} user failed: form is null");
            return false;
        }
        
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<License> _licenseRepository;
        private readonly IHasher _hasher;
    }
}