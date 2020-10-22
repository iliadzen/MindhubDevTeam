using System;
using ItHappened.Domain;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Application;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using List = LanguageExt.List;

namespace ItHappened.Tests
{
    public class RepositoryMock<T> : IRepository<T>
        where T : IEntity
    {
        public List<T> _repository;

        public RepositoryMock()
        {
            _repository = new List<T>();
        }

        public void Save(T entity)
        {
            if (!_repository.Contains(entity))
            {
                _repository.Add(entity);
            }
        }

        public Option<T> Get(Guid id)
        {
            return _repository.SingleOrDefault(entity => entity.Id == id);
        }

        public IReadOnlyCollection<T> GetAll()
        {
            return (IReadOnlyCollection<T>) _repository;
        }

        public void Update(T entity)
        {
            Option<T> oldEntity = _repository.SingleOrDefault(e => e.Id == entity.Id);
            oldEntity.Do(e =>
            {
                _repository.Remove(e);
                _repository.Add(entity);
            });
        }

        public void Delete(Guid id)
        {
            Option<T> entity = _repository.SingleOrDefault(e => e.Id == id);
            entity.Do(e =>
            {
                _repository.Remove(e);
            });
        }
    }
    public class EventServiceTests
    {
        [Test]
        public void CreateEvent_ValidData_SuccessfullyCreated()
        {
            // Arrange
            var userRepository = new RepositoryMock<User>();
            var userId = Guid.NewGuid();
            var user = new User(
                userId,
                "testUsername",
                "testPasswordHash",
                new License(LicenseType.Premium, DateTime.Now),
                DateTime.Now,
                DateTime.Now);
            userRepository.Save(user);

            var trackerRepository = new RepositoryMock<Tracker>();
            var trackerId = Guid.NewGuid();
            trackerRepository.Save(
                new Tracker(
                    trackerId,
                    userId,
                    "testTitle",
                    DateTime.Now,
                    DateTime.Now,
                    new System.Collections.Generic.HashSet<CustomizationType>()));
            var eventRepository = new RepositoryMock<Event>();
            
            
            var eventService = new EventService(eventRepository, trackerRepository);
            var eventId = Guid.NewGuid();
            var eventContent = new EventContent("testTitle");
            
            // Act
            eventService.CreateEvent(userId, trackerId, eventContent);
            
            // Assert
            var @event = eventRepository.GetAll().First();
            Assert.AreEqual(trackerId, @event.TrackerId);
            Assert.AreEqual(eventContent.Title, @event.Title);
        }
        
        [Test]
        public void GetEvent_ValidData_SuccessfullyGot()
        {
            // Arrange
            var userRepository = new RepositoryMock<User>();
            var userId = Guid.NewGuid();
            var user = new User(
                userId,
                "testUsername",
                "testPasswordHash",
                new License(LicenseType.Premium, DateTime.Now),
                DateTime.Now,
                DateTime.Now);
            userRepository.Save(user);

            var trackerRepository = new RepositoryMock<Tracker>();
            var trackerId = Guid.NewGuid();
            trackerRepository.Save(
                new Tracker(
                    trackerId,
                    userId,
                    "testTitle",
                    DateTime.Now,
                    DateTime.Now,
                    new System.Collections.Generic.HashSet<CustomizationType>()));
            var eventRepository = new RepositoryMock<Event>();
            
            
            var eventService = new EventService(eventRepository, trackerRepository);
            var eventContent = new EventContent("testTitle");
            var createdEvent = eventService.CreateEvent(userId, trackerId, eventContent);
            var eventId = createdEvent.ValueUnsafe().Id;

            // Act
            var @event = eventService.GetEvent(userId, eventId).ValueUnsafe();

            // Assert
            Assert.AreEqual(trackerId, @event.TrackerId);
            Assert.AreEqual(eventContent.Title, @event.Title);
        }
        
        [Test]
        public void GetEventsByTrackerId_InRepositoryTwoNeededEvents_SuccessfullyGotTwo()
        {
            // Arrange
            var userRepository = new RepositoryMock<User>();
            var userId = Guid.NewGuid();
            var user = new User(
                userId,
                "testUsername",
                "testPasswordHash",
                new License(LicenseType.Premium, DateTime.Now),
                DateTime.Now,
                DateTime.Now);
            userRepository.Save(user);

            var trackerRepository = new RepositoryMock<Tracker>();
            var trackerId = Guid.NewGuid();
            trackerRepository.Save(
                new Tracker(
                    trackerId,
                    userId,
                    "testTitle",
                    DateTime.Now,
                    DateTime.Now,
                    new System.Collections.Generic.HashSet<CustomizationType>()));
            var eventRepository = new RepositoryMock<Event>();
            
            
            var eventService = new EventService(eventRepository, trackerRepository);
            
            eventService.CreateEvent(userId, trackerId, new EventContent("1"));
            eventService.CreateEvent(userId, trackerId, new EventContent("2"));

            // Act
            var events = eventService.GetEventsByTrackerId(userId, trackerId);

            // Assert
            Assert.AreEqual(2, events.Count);
        }
        
        [Test]
        public void EditEvent_ValidData_SuccessfullyEdited()
        {
            // Arrange
            var userRepository = new RepositoryMock<User>();
            var userId = Guid.NewGuid();
            var user = new User(
                userId,
                "testUsername",
                "testPasswordHash",
                new License(LicenseType.Premium, DateTime.Now),
                DateTime.Now,
                DateTime.Now);
            userRepository.Save(user);

            var trackerRepository = new RepositoryMock<Tracker>();
            var trackerId = Guid.NewGuid();
            trackerRepository.Save(
                new Tracker(
                    trackerId,
                    userId,
                    "testTitle",
                    DateTime.Now,
                    DateTime.Now,
                    new System.Collections.Generic.HashSet<CustomizationType>()));
            var eventRepository = new RepositoryMock<Event>();
            
            
            var eventService = new EventService(eventRepository, trackerRepository);
            
            var @event = eventService.CreateEvent(userId, trackerId, new EventContent("1")).ValueUnsafe();

            // Act
            eventService.EditEvent(userId, @event.Id, new EventContent("2"));

            // Assert
            var editedEvent = eventRepository.Get(@event.Id).ValueUnsafe();
            Assert.AreEqual("2", editedEvent.Title);
        }
        
        [Test]
        public void DeleteEvent_ValidData_SuccessfullyDeleted()
        {
            // Arrange
            var userRepository = new RepositoryMock<User>();
            var userId = Guid.NewGuid();
            var user = new User(
                userId,
                "testUsername",
                "testPasswordHash",
                new License(LicenseType.Premium, DateTime.Now),
                DateTime.Now,
                DateTime.Now);
            userRepository.Save(user);

            var trackerRepository = new RepositoryMock<Tracker>();
            var trackerId = Guid.NewGuid();
            trackerRepository.Save(
                new Tracker(
                    trackerId,
                    userId,
                    "testTitle",
                    DateTime.Now,
                    DateTime.Now,
                    new System.Collections.Generic.HashSet<CustomizationType>()));
            var eventRepository = new RepositoryMock<Event>();
            
            
            var eventService = new EventService(eventRepository, trackerRepository);
            
            var @event = eventService.CreateEvent(userId, trackerId, new EventContent("1")).ValueUnsafe();

            // Act
            eventService.DeleteEvent(userId, @event.Id);

            // Assert
            var eventsCount = eventRepository._repository.Count();
            Assert.AreEqual(0, eventsCount);
        }
    }
}