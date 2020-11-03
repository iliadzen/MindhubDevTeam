using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Application;
using ItHappened.Domain;
using ItHappened.Infrastructure;
using NUnit.Framework;

namespace ItHappened.Tests
{
    [TestFixture]
    public class StatsServiceTests
    {
        [Test]
        public void GetStatsFactsForUser_TooFewEvents_IsEmpty()
        {
            // Arrange
            IRepository<Tracker> trackers = new InMemoryRepository<Tracker>();
            IRepository<Event> events = new InMemoryRepository<Event>();
            StatsService statsService = new StatsService(events, trackers);
            
            Guid userId = Guid.NewGuid();
            
            Guid trackerId = Guid.NewGuid();
            Tracker tracker = new Tracker(trackerId, userId,
                "Smonking", DateTime.Now, DateTime.Now,
                new HashSet<CustomizationType>());
            trackers.Save(tracker);
            
            Event @event = new Event(Guid.NewGuid(), trackerId,
                "Lord, forgive me for I smonked again...",
                DateTime.Now, DateTime.Now);
            events.Save(@event);
            
            IEnumerable<string> expectedDescriptions = new List<string>();
            
            // Act
            IEnumerable<string> actualDescriptions = statsService
                .GetStatsFactsForUser(userId)
                .Select(f => f.Description);

             // Assert
            Assert.AreEqual(expectedDescriptions, actualDescriptions);
        }
    }
}