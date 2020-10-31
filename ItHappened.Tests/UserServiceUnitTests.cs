using System;
using AutoFixture;
using ItHappened.Application;
using ItHappened.Domain;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;

namespace ItHappened.Tests
{
    public class UserServiceUnitTests
    {
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _mockUserRepository = new RepositoryMock<User>();
            _mockTrackerRepository = new RepositoryMock<Tracker>();
            _userService = new UserService(_mockUserRepository,  _mockTrackerRepository, new FakeHasher());
        }
        
        [Test]
        public void GetUserById_UserWithSuchIdExistsAndAskedByThisUser_GotUser()
        {
            var user = EntityMaker.CreateSomeUser(_mockUserRepository);

            var askedUser = _userService.GetUserById(user.Id, user.Id);
            
            Assert.IsTrue(askedUser.IsSome);
            Assert.AreEqual(user.Username, askedUser.ValueUnsafe().Username);
        }
        
        [Test]
        public void GetUserById_UserWithSuchIdExistsAndAskedByAnotherUser_DidNotGetUser()
        {
            var user = EntityMaker.CreateSomeUser(_mockUserRepository);

            var askedUser = _userService.GetUserById(Guid.NewGuid(), user.Id);
            
            Assert.IsTrue(askedUser.IsNone);
        }
        
        [Test]
        public void GetUserById_UserWithSuchIdNotExists_DidNotGetUser()
        {
            var id = Guid.NewGuid();
            var askedUser = _userService.GetUserById(id, id);
            
            Assert.IsTrue(askedUser.IsNone);
        }
        
        [Test]
        public void GetUserByUsername_UserWithSuchNameExists_GotUser()
        {
            var user = EntityMaker.CreateSomeUser(_mockUserRepository);

            var askedUser = _userService.GetUserByUsername(user.Username);
            
            Assert.IsTrue(askedUser.IsSome);
            Assert.AreEqual(user.Username, askedUser.ValueUnsafe().Username);
        }
        
        [Test]
        public void GetUserByUsername_UserWithSuchNameNotExists_DidNotGetUser()
        {
            EntityMaker.CreateSomeUser(_mockUserRepository);

            var askedUser = _userService.GetUserByUsername("user2");
            
            Assert.IsTrue(askedUser.IsNone);
        }
        
        [Test]
        public void GetUserByUsername_TwoUsersWithSuchNameExists_DidNotGetUser()
        {
            var name = "user";
            EntityMaker.CreateSomeUser(_mockUserRepository, name);
            EntityMaker.CreateSomeUser(_mockUserRepository, name);

            var askedUser = _userService.GetUserByUsername(name);
            
            Assert.IsTrue(askedUser.IsNone);
        }

        [Test]
        public void EditUser_UserEditsByHimself_UserWasEdited()
        {
            var form = _fixture.Create<UserForm>();
            var user = EntityMaker.CreateSomeUser(_mockUserRepository);
            
            _userService.EditUser(user.Id, user.Id, form);

            var editedUser = _mockUserRepository.Get(user.Id);
            Assert.IsTrue(editedUser.IsSome);
            Assert.AreEqual(form.Username, editedUser.ValueUnsafe().Username);
        }
        
        [Test]
        public void EditUser_UserEditsByAnotherUser_UserWasNotEdited()
        {
            var form = _fixture.Create<UserForm>();
            var initialName = "user";
            var user = EntityMaker.CreateSomeUser(_mockUserRepository, initialName);
            
            _userService.EditUser(Guid.NewGuid(), user.Id, form);

            var editedUser = _mockUserRepository.Get(user.Id);
            Assert.IsTrue(editedUser.IsSome);
            Assert.AreEqual(initialName, editedUser.ValueUnsafe().Username);
        }
        
        [Test]
        public void EditUser_FormIsNotComplete_UserWasNotEdited()
        {
            var form = new UserForm(null, null, new License());
            var initialName = "user";
            var user = EntityMaker.CreateSomeUser(_mockUserRepository, initialName);
            
            _userService.EditUser(user.Id, user.Id, form);

            var editedUser = _mockUserRepository.Get(user.Id);
            Assert.IsTrue(editedUser.IsSome);
            Assert.AreEqual(initialName, editedUser.ValueUnsafe().Username);
        }
        
        [Test]
        public void DeleteUser_UserDeletesByHimself_UserAndHisTrackersWereDeleted()
        {
            var user = EntityMaker.CreateSomeUser(_mockUserRepository);
            EntityMaker.CreateSomeTracker(user.Id, _mockTrackerRepository);
            EntityMaker.CreateSomeTracker(user.Id, _mockTrackerRepository);
            EntityMaker.CreateSomeTracker(Guid.NewGuid(), _mockTrackerRepository);
            
            _userService.DeleteUser(user.Id, user.Id);

            Assert.AreEqual(0, _mockUserRepository.GetAll().Count);
            Assert.AreEqual(1, _mockTrackerRepository.GetAll().Count);
        }
        
        [Test]
        public void DeleteUser_UserDeletesByAnotherUser_UserAndHisTrackersWereNotDeleted()
        {
            var user = EntityMaker.CreateSomeUser(_mockUserRepository);
            EntityMaker.CreateSomeTracker(user.Id, _mockTrackerRepository);
            EntityMaker.CreateSomeTracker(user.Id, _mockTrackerRepository);
            EntityMaker.CreateSomeTracker(Guid.NewGuid(), _mockTrackerRepository);
            
            _userService.DeleteUser(Guid.NewGuid(), user.Id);

            Assert.AreEqual(1, _mockUserRepository.GetAll().Count);
            Assert.AreEqual(3, _mockTrackerRepository.GetAll().Count);
        }

        [Test]
        public void LoginByCredentials_InsertedCorrectPassword_GotUser()
        {
            var user = EntityMaker.CreateSomeUser(_mockUserRepository);

            var loggedUser = _userService.LogInByCredentials(user.Username, user.PasswordHash);

            Assert.IsTrue(loggedUser.IsSome);
            Assert.AreEqual(user.Username, loggedUser.ValueUnsafe().Username);
        }
        
        [Test]
        public void LoginByCredentials_InsertedIncorrectPassword_DidNotGetUser()
        {
            var user = EntityMaker.CreateSomeUser(_mockUserRepository);

            var loggedUser = _userService.LogInByCredentials(user.Username, "qwerty123");
            
            Assert.IsTrue(loggedUser.IsNone);
        }
        
        [Test]
        public void LoginByCredentials_NoSuchUsername_DidNotGetUser()
        {
            var loggedUser = _userService.LogInByCredentials("admin", "qwerty123");
            
            Assert.IsTrue(loggedUser.IsNone);
        }

        private Fixture _fixture;
        private RepositoryMock<User> _mockUserRepository;
        private RepositoryMock<Tracker> _mockTrackerRepository;
        private UserService _userService;
    }
}