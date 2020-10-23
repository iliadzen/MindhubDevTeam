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
            _customizations = _fixture.Create<ISet<CustomizationType>>();
            _trackerService = new TrackerService(_mockTrackerRepository, _mockEventRepository);
        }
        [Test]
        public void GetTracker_UserGetsOwnTracker_GotTracker()
        {
            var userTracker = CreateSomeTracker(_userId);

            var askedTracker = _trackerService.GetTracker(_userId, userTracker.Id);
            
            Assert.IsTrue(askedTracker.IsSome);
            Assert.AreEqual(userTracker.Title, askedTracker.ValueUnsafe().Title);
        }
        
        [Test]
        public void GetTracker_UserGetsSomeonesTracker_DidNotGetTracker()
        {
            var someonesTracker = CreateSomeTracker(Guid.NewGuid());

            var askedTracker = _trackerService.GetTracker(_userId, someonesTracker.Id);

            Assert.IsFalse(askedTracker.IsSome);
        }

        [Test]
        public void EditTracker_UserEditsOwnTracker_TrackerWasEdited()
        {
            var userTracker = CreateSomeTracker(_userId);
            var trackerEditingForm = _fixture.Create<TrackerForm>();
            
            _trackerService.EditTracker(_userId, userTracker.Id,  trackerEditingForm);
            
            var trackers = _mockTrackerRepository.GetAll();
            Assert.AreEqual(trackerEditingForm.Title, trackers.ElementAt(0).Title);
        }
        
        [Test]
        public void EditTracker_UserEditsSomeonesTracker_TrackerWasNotEdited()
        {
            var someonesTracker = CreateSomeTracker(_someonesId);
            var trackerEditingForm = _fixture.Create<TrackerForm>();
            
            _trackerService.EditTracker(_userId, someonesTracker.Id,  trackerEditingForm);
            
            var trackers = _mockTrackerRepository.GetAll();
            Assert.AreEqual(someonesTracker.Title, trackers.ElementAt(0).Title);
        }
        
        [Test]
        public void DeleteTracker_UserDeletesOwnTracker_TrackerAndItsEventsWereDeleted()
        {
            var userTracker = CreateSomeTracker(_userId);
            CreateSomeEvent(userTracker.Id);
            CreateSomeEvent(userTracker.Id);

            _trackerService.DeleteTracker(_userId, userTracker.Id);
            
            var trackers = _mockTrackerRepository.GetAll();
            var events = _mockEventRepository.GetAll();
            Assert.AreEqual(0, trackers.Count);
            Assert.AreEqual(0, events.Count);
        }
        
        [Test]
        public void DeleteTracker_UserDeletesSomeonesTracker_TrackerAndItsEventsWereNotDeleted()
        {
            var someonesTracker = CreateSomeTracker(_someonesId);
            CreateSomeEvent(someonesTracker.Id);
            CreateSomeEvent(someonesTracker.Id);

            _trackerService.DeleteTracker(_userId, someonesTracker.Id);
            
            var trackers = _mockTrackerRepository.GetAll();
            var events = _mockEventRepository.GetAll();
            Assert.AreEqual(1, trackers.Count);
            Assert.AreEqual(2, events.Count);
            Assert.AreEqual(someonesTracker.Title, trackers.ElementAt(0).Title);
        }
        
        private Tracker CreateSomeTracker(Guid userId)
        {
            var trackerId = Guid.NewGuid();
            var tracker = new Tracker(
                trackerId, 
                userId, 
                $"{trackerId}", 
                DateTime.Now, 
                DateTime.Now, 
                _customizations);
            _mockTrackerRepository.Save(tracker);
            return tracker;
        }

        private Event CreateSomeEvent(Guid trackerId)
        {
            var eventId = Guid.NewGuid();
            var @event = new Event(
                eventId, 
                trackerId, 
                $"{eventId}", 
                DateTime.Now, 
                DateTime.Now);
            _mockEventRepository.Save(@event);
            return @event;
        }

        private Fixture _fixture;
        private RepositoryMock<Tracker> _mockTrackerRepository;
        private RepositoryMock<Event> _mockEventRepository;
        private Guid _userId;
        private Guid _someonesId;
        private ISet<CustomizationType> _customizations;
        private TrackerService _trackerService;
    }
}