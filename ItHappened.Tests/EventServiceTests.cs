using System;
using ItHappened.Domain;
using NUnit.Framework;
using System.Linq;
using AutoFixture;
using ItHappened.Application;
using LanguageExt.UnsafeValueAccess;

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

            _userOne = _fixture.Create<User>();
            _userTwo = _fixture.Create<User>();
            _mockUserRepository.Save(_userOne);
            _mockUserRepository.Save(_userTwo);

            _trackerUserOne = CreateSomeTracker(_userOne.Id);
            _trackerUserTwo = CreateSomeTracker(_userTwo.Id);
            _mockTrackerRepository.Save(_trackerUserOne);
            _mockTrackerRepository.Save(_trackerUserTwo);

            _eventService = new EventService(_mockEventRepository, _mockTrackerRepository);
        }

        [Test]
        public void CreateEvent_ValidData_EventCreated()
        {
            // Arrange
            var eventContent = new EventContent("testTitle");

            // Act
            _eventService.CreateEvent(_userOne.Id, _trackerUserOne.Id, eventContent);

            // Assert
            var @event = _mockEventRepository.GetAll().First();
            Assert.AreEqual(_trackerUserOne.Id, @event.TrackerId);
            Assert.AreEqual(eventContent.Title, @event.Title);
        }

        [Test]
        public void CreateEvent_NullContent_EventNotCreated()
        {
            // Arrange
            var eventContent = new EventContent(null);
            
            // Act
            _eventService.CreateEvent(_userOne.Id, _trackerUserOne.Id, eventContent);
            
            // Assert
            var eventsCount = _mockEventRepository.GetAll().Count();
            Assert.AreEqual(0, eventsCount);
        }
        
        [Test]
        public void GetEvent_UserGetsOwnEvent_SuccessfullyGot()
        {
            // Arrange
            var @event = CreateSomeEvent(_trackerUserOne.Id);
            _mockEventRepository.Save(@event);
            
            // Act
            var gottenEvent = _eventService.GetEvent(_userOne.Id, @event.Id).ValueUnsafe();

            // Assert
            Assert.AreEqual(@event.Id, gottenEvent.Id);
            Assert.AreEqual(@event.TrackerId, gottenEvent.TrackerId);
            Assert.AreEqual(@event.Title, gottenEvent.Title);
        }
        
        [Test]
        public void GetEvent_UserGetsSomeonesEvent_EventNotGot()
        {
            // Arrange
            var @event = CreateSomeEvent(_trackerUserOne.Id);
            _mockEventRepository.Save(@event);
            
            // Act
            var gottenEvent = _eventService.GetEvent(_userTwo.Id, @event.Id).ValueUnsafe();

            // Assert
            Assert.AreEqual(null, gottenEvent);
        }
        
        [Test]
        public void GetEvent_UserGetsNoExistedEvent_EventNotGot()
        {
            // Arrange
            var @event = CreateSomeEvent(_trackerUserOne.Id);
            _mockEventRepository.Save(@event);
            
            // Act
            var gottenEvent = _eventService.GetEvent(_userOne.Id, Guid.NewGuid()).ValueUnsafe();

            // Assert
            Assert.AreEqual(null, gottenEvent);
        }
        
        [Test]
        public void GetEventsByTrackerId_UserHaveTwoEventsInRepositoryAndGetsIt_SuccessfullyGotTwo()
        {
            // Arrange
            var eventOne = CreateSomeEvent(_trackerUserOne.Id);
            var eventTwo = CreateSomeEvent(_trackerUserOne.Id);
            _mockEventRepository.Save(eventOne);
            _mockEventRepository.Save(eventTwo);
            
            // Act
            var events = _eventService.GetEventsByTrackerId(_userOne.Id, _trackerUserOne.Id);

            // Assert
            Assert.AreEqual(2, events.Count);
        }
        
        [Test]
        public void GetEventsByTrackerId_SomeoneWantsToGetOthersEvents_EventsNotGot()
        {
            // Arrange
            var eventOne = CreateSomeEvent(_trackerUserOne.Id);
            var eventTwo = CreateSomeEvent(_trackerUserOne.Id);
            _mockEventRepository.Save(eventOne);
            _mockEventRepository.Save(eventTwo);
            
            // Act
            var events = _eventService.GetEventsByTrackerId(_userTwo.Id, _trackerUserOne.Id);

            // Assert
            Assert.AreEqual(0, events.Count);
        }
        
        [Test]
        public void GetEventsByTrackerId_SomeoneWantsToGetEventsFromNotExistedTracker_EventsNotGot()
        {
            // Arrange
            var eventOne = CreateSomeEvent(_trackerUserOne.Id);
            var eventTwo = CreateSomeEvent(_trackerUserOne.Id);
            _mockEventRepository.Save(eventOne);
            _mockEventRepository.Save(eventTwo);
            
            // Act
            var events = _eventService.GetEventsByTrackerId(_userOne.Id, Guid.NewGuid());

            // Assert
            Assert.AreEqual(0, events.Count);
        }
        
        [Test]
        public void EditEvent_UserEditOwnEvent_SuccessfullyEdited()
        {
            // Arrange
            var @event = CreateSomeEvent(_trackerUserOne.Id);
            _mockEventRepository.Save(@event);
            
            // Act
            _eventService.EditEvent(_userOne.Id, @event.Id, new EventContent("2"));

            // Assert
            var editedEvent = _mockEventRepository.Get(@event.Id).ValueUnsafe();
            Assert.AreEqual("2", editedEvent.Title);
        }
        
        [Test]
        public void EditEvent_UserEditOthersEvent_EventNotChanges()
        {
            // Arrange
            var @event = CreateSomeEvent(_trackerUserOne.Id);
            _mockEventRepository.Save(@event);
            
            // Act
            _eventService.EditEvent(_userTwo.Id, @event.Id, new EventContent("2"));

            // Assert
            var editedEvent = _mockEventRepository.Get(@event.Id).ValueUnsafe();
            Assert.AreEqual(@event.Title, editedEvent.Title);
        }
        
        [Test]
        public void EditEvent_UserEditNotExistedEvent_NothingHappens()
        {
            // Arrange
            var @event = CreateSomeEvent(_trackerUserOne.Id);
            _mockEventRepository.Save(@event);
            
            // Act
            _eventService.EditEvent(_userOne.Id, Guid.NewGuid(), new EventContent("2"));

            // Assert
            Assert.Pass();
        }
        
        [Test]
        public void DeleteEvent_UserDeletesOwnEvent_SuccessfullyDeleted()
        {
            // Arrange
            var @event = CreateSomeEvent(_trackerUserOne.Id);
            _mockEventRepository.Save(@event);
            
            // Act
            _eventService.DeleteEvent(_userOne.Id, @event.Id);

            // Assert
            var eventsCount = _mockEventRepository.GetAll().Count();
            Assert.AreEqual(0, eventsCount);
        }
        
        [Test]
        public void DeleteEvent_UserDeletesSomeonesEvent_EventNotDeleted()
        {
            // Arrange
            var @event = CreateSomeEvent(_trackerUserOne.Id);
            _mockEventRepository.Save(@event);
            
            // Act
            _eventService.DeleteEvent(_userTwo.Id, @event.Id);

            // Assert
            var eventsCount = _mockEventRepository.GetAll().Count();
            Assert.AreEqual(1, eventsCount);
        }
        
        [Test]
        public void DeleteEvent_UserDeletesNotExistedEvent_NothingHappens()
        {
            // Arrange
            var @event = CreateSomeEvent(_trackerUserOne.Id);
            _mockEventRepository.Save(@event);
            
            // Act
            _eventService.DeleteEvent(_userOne.Id, Guid.NewGuid());

            // Assert
            var eventsCount = _mockEventRepository.GetAll().Count();
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
        
        private Tracker CreateSomeTracker(Guid userId)
        {
            var trackerId = Guid.NewGuid();
            return new Tracker(
                trackerId, 
                userId, 
                $"{trackerId}", 
                DateTime.Now, 
                DateTime.Now, 
                new System.Collections.Generic.HashSet<CustomizationType>());
        }
        
        private Event CreateSomeEvent(Guid trackerId)
        {
            var eventId = Guid.NewGuid();
            return new Event(
                eventId, 
                trackerId, 
                $"{eventId}", 
                DateTime.Now, 
                DateTime.Now);
        }
    }
}