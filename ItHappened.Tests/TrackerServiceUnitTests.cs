using System;
using AutoFixture;
using ItHappened.Application;
using ItHappened.Domain;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Tests
{
    public class TrackerServiceUnitTests
    {
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _userId = Guid.NewGuid();
            _someonesId = Guid.NewGuid();
            _mockTrackerRepository = new RepositoryMock<Tracker>();
            _mockEventRepository = new RepositoryMock<Event>();
            _trackerService = new TrackerService(_mockTrackerRepository, _mockEventRepository);
        }
        [Test]
        public void GetTracker_UserGetsOwnTracker_GotTracker()
        {
            var userTracker = EntityMaker.CreateSomeTracker(_userId, _mockTrackerRepository);

            var askedTracker = _trackerService.GetTracker(_userId, userTracker.Id);
            
            Assert.IsTrue(askedTracker.IsSome);
            Assert.AreEqual(userTracker.Title, askedTracker.ValueUnsafe().Title);
        }
        
        [Test]
        public void GetTracker_UserGetsSomeonesTracker_DidNotGetTracker()
        {
            var someonesTracker = EntityMaker.CreateSomeTracker(Guid.NewGuid(), _mockTrackerRepository);

            var askedTracker = _trackerService.GetTracker(_userId, someonesTracker.Id);

            Assert.IsFalse(askedTracker.IsSome);
        }

        [Test]
        public void EditTracker_UserEditsOwnTracker_TrackerWasEdited()
        {
            var userTracker = EntityMaker.CreateSomeTracker(_userId, _mockTrackerRepository);
            var trackerEditingForm = _fixture.Create<TrackerForm>();
            
            _trackerService.EditTracker(_userId, userTracker.Id,  trackerEditingForm);
            
            var trackers = _mockTrackerRepository.GetAll();
            Assert.AreEqual(trackerEditingForm.Title, trackers.ElementAt(0).Title);
        }
        
        [Test]
        public void EditTracker_UserEditsSomeonesTracker_TrackerWasNotEdited()
        {
            var someonesTracker = EntityMaker.CreateSomeTracker(_someonesId, _mockTrackerRepository);
            var trackerEditingForm = _fixture.Create<TrackerForm>();
            
            _trackerService.EditTracker(_userId, someonesTracker.Id,  trackerEditingForm);
            
            var trackers = _mockTrackerRepository.GetAll();
            Assert.AreEqual(someonesTracker.Title, trackers.ElementAt(0).Title);
        }
        
        [Test]
        public void DeleteTracker_UserDeletesOwnTracker_TrackerAndItsEventsWereDeleted()
        {
            var userTracker = EntityMaker.CreateSomeTracker(_userId, _mockTrackerRepository);
            EntityMaker.CreateSomeTracker(_someonesId, _mockTrackerRepository);
            EntityMaker.CreateSomeEvent(userTracker.Id, _mockEventRepository);
            EntityMaker.CreateSomeEvent(userTracker.Id, _mockEventRepository);
            EntityMaker.CreateSomeEvent(_someonesId, _mockEventRepository);

            _trackerService.DeleteTracker(_userId, userTracker.Id);
            
            var trackers = _mockTrackerRepository.GetAll();
            var events = _mockEventRepository.GetAll();
            Assert.AreEqual(1, trackers.Count);
            Assert.AreEqual(1, events.Count);
        }
        
        [Test]
        public void DeleteTracker_UserDeletesSomeonesTracker_TrackerAndItsEventsWereNotDeleted()
        {
            var someonesTracker = EntityMaker.CreateSomeTracker(_someonesId, _mockTrackerRepository);
            EntityMaker.CreateSomeEvent(someonesTracker.Id, _mockEventRepository);
            EntityMaker.CreateSomeEvent(someonesTracker.Id, _mockEventRepository);

            _trackerService.DeleteTracker(_userId, someonesTracker.Id);
            
            var trackers = _mockTrackerRepository.GetAll();
            var events = _mockEventRepository.GetAll();
            Assert.AreEqual(1, trackers.Count);
            Assert.AreEqual(2, events.Count);
            Assert.AreEqual(someonesTracker.Title, trackers.ElementAt(0).Title);
        }

        [Test]
        public void GetUserTrackers_UserGetsOwnTrackers_GotTrackers()
        {
            var tracker = EntityMaker.CreateSomeTracker(_userId, _mockTrackerRepository);

            var trackers = _trackerService.GetUserTrackers(_userId);
            
            Assert.AreEqual(tracker.Title, trackers.ElementAt(0).Title);
        }

        private Fixture _fixture;
        private RepositoryMock<Tracker> _mockTrackerRepository;
        private RepositoryMock<Event> _mockEventRepository;
        private Guid _userId;
        private Guid _someonesId;
        private TrackerService _trackerService;
    }
}