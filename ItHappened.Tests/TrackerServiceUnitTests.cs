using System;
using AutoFixture;
using ItHappened.Application;
using ItHappened.Domain;
using NUnit.Framework;
using System.Collections.Generic;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Tests
{
    public class TrackerServiceUnitTests
    {
        [SetUp]
        public void SetUp()
        {
            _user = _fixture.Create<User>();
            _trackerId = Guid.NewGuid();
            _title = "Title";
            _customizations = _fixture.Create<ISet<CustomizationType>>();
            _trackerService = new TrackerService(_mockTrackerRepository);
        }
        [Test]
        public void UserGetsOwnTracker_GotTracker()
        {
            var userTracker = new Tracker(_trackerId, _user.Id, _title,
                DateTime.Now, DateTime.Now, _customizations);
            _mockTrackerRepository.Save(userTracker);

            var askedTracker = _trackerService.GetTracker(_user.Id, _trackerId);
            
            Assert.IsTrue(askedTracker.IsSome);
            Assert.AreEqual(_title, askedTracker.ValueUnsafe().Title);
        }
        
        [Test]
        public void UserGetsSomeonesTracker_DidNotGetTracker()
        {
            var someonesTracker = new Tracker(_trackerId, Guid.NewGuid(), _title,
                DateTime.Now, DateTime.Now, _customizations);
            _mockTrackerRepository.Save(someonesTracker);

            var askedTracker = _trackerService.GetTracker(_user.Id, _trackerId);

            Assert.IsFalse(askedTracker.IsSome);
        }

        [Test]
        public void UserEditsOwnTracker_TrackerWasEdited()
        {
            var userTracker = new Tracker(_trackerId, _user.Id, _title,
                DateTime.Now, DateTime.Now, _customizations);
            _mockTrackerRepository.Save(userTracker);
            var trackerEditingForm = _fixture.Create<TrackerForm>();
            
            _trackerService.EditTracker(_user.Id, _trackerId,  trackerEditingForm);
            
            var editedTracker = _trackerService.GetTracker(_user.Id, _trackerId);
            Assert.IsTrue(editedTracker.IsSome);
            Assert.AreEqual(trackerEditingForm.Title, editedTracker.ValueUnsafe().Title);
        }
        
        [Test]
        public void UserEditsSomeonesTracker_TrackerWasNotEdited()
        {
            var someonesId = Guid.NewGuid();
            var someonesTracker = new Tracker(_trackerId, someonesId, _title,
                DateTime.Now, DateTime.Now, _customizations);
            _mockTrackerRepository.Save(someonesTracker);
            var trackerEditingForm = _fixture.Create<TrackerForm>();
            
            _trackerService.EditTracker(_user.Id, _trackerId,  trackerEditingForm);
            
            var editedTracker = _trackerService.GetTracker(someonesId, _trackerId);
            Assert.IsTrue(editedTracker.IsSome);
            Assert.AreEqual(_title, editedTracker.ValueUnsafe().Title);
        }
        
        [Test]
        public void UserDeletesOwnTracker_TrackerWasDeleted()
        {
            var userTracker = new Tracker(_trackerId, _user.Id, _title,
                DateTime.Now, DateTime.Now, _customizations);
            _mockTrackerRepository.Save(userTracker);

            _trackerService.DeleteTracker(_user.Id, _trackerId);
            
            var deletedTracker = _trackerService.GetTracker(_user.Id, _trackerId);
            Assert.IsFalse(deletedTracker.IsSome);
        }
        
        [Test]
        public void UserDeletesSomeonesTracker_TrackerWasNotDeleted()
        {
            var someonesId = Guid.NewGuid();
            var someonesTracker = new Tracker(_trackerId, someonesId, _title,
                DateTime.Now, DateTime.Now, _customizations);
            _mockTrackerRepository.Save(someonesTracker);

            _trackerService.DeleteTracker(_user.Id, _trackerId);
            
            var deletedTracker = _trackerService.GetTracker(someonesId, _trackerId);
            Assert.IsTrue(deletedTracker.IsSome);
            Assert.AreEqual(_title, deletedTracker.ValueUnsafe().Title);
        }

        private Fixture _fixture = new Fixture();
        private RepositoryMock<Tracker> _mockTrackerRepository = new RepositoryMock<Tracker>();
        private User _user;
        private Guid _trackerId;
        private string _title;
        private ISet<CustomizationType> _customizations;
        private TrackerService _trackerService;
    }
}