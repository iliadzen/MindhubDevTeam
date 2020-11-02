using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using ItHappened.Application;
using ItHappened.Domain;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;

namespace ItHappened.Tests
{
    [TestFixture]
    public class EventServiceTests
    {
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _mockUserRepository = new RepositoryMock<User>();
            _mockTrackerRepository = new RepositoryMock<Tracker>();
            _mockEventRepository = new RepositoryMock<Event>();

            _userOne = EntityMaker.CreateSomeUser(_mockUserRepository, "userOne");
            _userTwo = EntityMaker.CreateSomeUser(_mockUserRepository, "userTwo");

            _trackerUserOne = EntityMaker.CreateSomeTracker(_userOne.Id, _mockTrackerRepository);
            _trackerUserTwo = EntityMaker.CreateSomeTracker(_userTwo.Id, _mockTrackerRepository);
            
            _eventService = new EventService(_mockEventRepository, _mockTrackerRepository);
        }

        [Test]
        public void CreateEvent_ValidData_EventCreated()
        {
            var eventContent = new EventContent("testTitle");

            _eventService.CreateEvent(_userOne.Id, _trackerUserOne.Id, eventContent);

            var @event = _mockEventRepository.GetAll().First();
            Assert.AreEqual(_trackerUserOne.Id, @event.TrackerId);
            Assert.AreEqual(eventContent.Title, @event.Title);
        }

        [Test]
        public void CreateEvent_NullContent_EventNotCreated()
        {
            var eventContent = new EventContent(null);

            _eventService.CreateEvent(_userOne.Id, _trackerUserOne.Id, eventContent);

            var eventsCount = _mockEventRepository.GetAll().Count;
            Assert.AreEqual(0, eventsCount);
        }

        [Test]
        public void GetEvent_UserGetsOwnEvent_SuccessfullyGot()
        {
            var @event = EntityMaker.CreateSomeEvent(_trackerUserOne.Id, _mockEventRepository);
            var rr = EntityMaker.CreateSomeEvent(_trackerUserOne.Id, _mockEventRepository);
            var gottenEvent = _eventService.GetEvent(_userOne.Id, @event.Id).ValueUnsafe();

            Assert.AreEqual(@event.Id, gottenEvent.Id);
            Assert.AreEqual(@event.TrackerId, gottenEvent.TrackerId);
            Assert.AreEqual(@event.Title, gottenEvent.Title);
        }

        [Test]
        public void GetEvent_UserGetsSomeonesEvent_EventNotGot()
        {
            var @event = EntityMaker.CreateSomeEvent(_trackerUserOne.Id, _mockEventRepository);
            
            var gottenEvent = _eventService.GetEvent(_userTwo.Id, @event.Id).ValueUnsafe();

            Assert.Null(gottenEvent);
        }

        [Test]
        public void GetEvent_UserGetsNoExistedEvent_EventNotGot()
        {
            var @event = EntityMaker.CreateSomeEvent(_trackerUserOne.Id, _mockEventRepository);
            
            var gottenEvent = _eventService.GetEvent(_userOne.Id, Guid.NewGuid()).ValueUnsafe();

            Assert.Null(gottenEvent);
        }

        [Test]
        public void GetEventsByTrackerId_UserHaveTwoEventsInRepositoryAndGetsIt_SuccessfullyGotTwo()
        {
            var eventOne = EntityMaker.CreateSomeEvent(_trackerUserOne.Id, _mockEventRepository);
            var eventTwo = EntityMaker.CreateSomeEvent(_trackerUserOne.Id, _mockEventRepository);
            
            var events = _eventService.GetEventsByTrackerId(_userOne.Id, _trackerUserOne.Id);

            Assert.AreEqual(2, events.Count);
        }

        [Test]
        public void GetEventsByTrackerId_SomeoneWantsToGetOthersEvents_EventsNotGot()
        {
            var eventOne = EntityMaker.CreateSomeEvent(_trackerUserOne.Id, _mockEventRepository);
            var eventTwo = EntityMaker.CreateSomeEvent(_trackerUserOne.Id, _mockEventRepository);
            
            var events = _eventService.GetEventsByTrackerId(_userTwo.Id, _trackerUserOne.Id);

            Assert.AreEqual(0, events.Count);
        }

        [Test]
        public void GetEventsByTrackerId_SomeoneWantsToGetEventsFromNotExistedTracker_EventsNotGot()
        {
            var eventOne = EntityMaker.CreateSomeEvent(_trackerUserOne.Id, _mockEventRepository);
            var eventTwo = EntityMaker.CreateSomeEvent(_trackerUserOne.Id, _mockEventRepository);

            
            var events = _eventService.GetEventsByTrackerId(_userOne.Id, Guid.NewGuid());

            Assert.AreEqual(0, events.Count);
        }

        [Test]
        public void EditEvent_UserEditOwnEvent_SuccessfullyEdited()
        {
            var @event = EntityMaker.CreateSomeEvent(_trackerUserOne.Id, _mockEventRepository);
            
            _eventService.EditEvent(_userOne.Id, @event.Id, new EventContent("2"));

            var editedEvent = _mockEventRepository.Get(@event.Id).ValueUnsafe();
            Assert.AreEqual("2", editedEvent.Title);
        }

        [Test]
        public void EditEvent_UserEditOthersEvent_EventNotChanges()
        {
            var @event = EntityMaker.CreateSomeEvent(_trackerUserOne.Id, _mockEventRepository);
            
            _eventService.EditEvent(_userTwo.Id, @event.Id, new EventContent("2"));

            var editedEvent = _mockEventRepository.Get(@event.Id).ValueUnsafe();
            Assert.AreEqual(@event.Title, editedEvent.Title);
        }

        [Test]
        public void EditEvent_UserEditNotExistedEvent_NothingHappens()
        {
            var @event = EntityMaker.CreateSomeEvent(_trackerUserOne.Id, _mockEventRepository);
            
            _eventService.EditEvent(_userOne.Id, Guid.NewGuid(), new EventContent("2"));

            Assert.Pass();
        }

        [Test]
        public void DeleteEvent_UserDeletesOwnEvent_SuccessfullyDeleted()
        {
            var @event = EntityMaker.CreateSomeEvent(_trackerUserOne.Id, _mockEventRepository);
            
            _eventService.DeleteEvent(_userOne.Id, @event.Id);

            var eventsCount = _mockEventRepository.GetAll().Count;
            Assert.AreEqual(0, eventsCount);
        }

        [Test]
        public void DeleteEvent_UserDeletesSomeonesEvent_EventNotDeleted()
        {
            var @event = EntityMaker.CreateSomeEvent(_trackerUserOne.Id, _mockEventRepository);
            
            _eventService.DeleteEvent(_userTwo.Id, @event.Id);

            var eventsCount = _mockEventRepository.GetAll().Count;
            Assert.AreEqual(1, eventsCount);
        }

        [Test]
        public void DeleteEvent_UserDeletesNotExistedEvent_NothingHappens()
        {
            var @event = EntityMaker.CreateSomeEvent(_trackerUserOne.Id, _mockEventRepository);
            
            _eventService.DeleteEvent(_userOne.Id, Guid.NewGuid());

            var eventsCount = _mockEventRepository.GetAll().Count;
            Assert.AreEqual(1, eventsCount);
        }

        private Fixture _fixture;
        private IEventService _eventService;
        private IRepository<User> _mockUserRepository;
        private IRepository<Tracker> _mockTrackerRepository;
        private IRepository<Event> _mockEventRepository;
        private User _userOne;
        private User _userTwo;
        private Tracker _trackerUserOne;
        private Tracker _trackerUserTwo;
    }
}