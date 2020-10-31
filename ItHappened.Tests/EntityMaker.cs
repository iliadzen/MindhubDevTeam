using System;
using System.Collections.Generic;
using AutoFixture;
using ItHappened.Domain;

namespace ItHappened.Tests
{
    public class EntityMaker
    {
        public static User CreateSomeUser(IRepository<User> repository, string username = "admin")
        {
            var user = new User(
                Guid.NewGuid(), 
                username,
                "admin",
                new License(), 
                DateTime.Now, 
                DateTime.Now);
            repository.Save(user);
            return user;
        }
        
        public static Tracker CreateSomeTracker(Guid userId, IRepository<Tracker> repository)
        {
            var trackerId = Guid.NewGuid();
            var tracker = new Tracker(
                Guid.NewGuid(), 
                userId, 
                $"{trackerId}", 
                DateTime.Now, 
                DateTime.Now, 
                new HashSet<CustomizationType>());
            repository.Save(tracker);
            
            return tracker;
        }
        
        public static Event CreateSomeEvent(Guid trackerId, IRepository<Event> repository)
        {
            var eventId = Guid.NewGuid();
            var @event = new Event(
                eventId, 
                trackerId, 
                $"{eventId}", 
                DateTime.Now, 
                DateTime.Now);
            repository.Save(@event);
            
            return @event;
        }
    }
}