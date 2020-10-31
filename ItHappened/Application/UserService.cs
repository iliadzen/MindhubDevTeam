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
        public UserService(IRepository<User> userRepository, IRepository<Tracker> trackerRepository, IHasher hasher)
        {
            _userRepository = userRepository;
            _trackerRepository = trackerRepository;
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
                    form.Username, _hasher.MakeSaltedHash(form.Password),
                    form.License, DateTime.Now, DateTime.Now);

                _userRepository.Save(user);
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
                    var newUser = new User(userId,
                        form.Username, _hasher.MakeSaltedHash(form.Password),
                        form.License, user.CreationDate, DateTime.Now);

                    _userRepository.Update(newUser);
                    Log.Information($"User {form.Username} with ID {newUser.Id} updated");
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
            DeleteUserTrackers(userId);
            _userRepository.Delete(userId);
        }

        private void DeleteUserTrackers(Guid userId)
        {
            var usersTrackers = _trackerRepository.GetAll()
                .Where(tracker => tracker.UserId == userId).ToList().AsReadOnly();
            foreach (var tracker in usersTrackers)
            {
                _trackerRepository.Delete(tracker.Id);
            }
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
                if (form.Username.IsNull() || form.Password.IsNull() || form.License.IsNull())
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
        private readonly IRepository<Tracker> _trackerRepository;
        private readonly IHasher _hasher;
    }
}