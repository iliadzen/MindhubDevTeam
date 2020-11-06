using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Stats;
using ItHappened.Domain.Stats.Facts.Trackers;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;

namespace ItHappened.Tests
{
    [TestFixture]
    public class StatsFactsTests
    {
        [SetUp]
        public void SetUp()
        {
            _mockTrackerRepository = new RepositoryMock<Tracker>();
            _mockEventRepository = new RepositoryMock<Event>();
        }

        [Test]
        public void BiggestDayOverallStatsFact_TooFewEvents_FactIsNone()
        {
            var tracker = EntityMaker.CreateSomeTracker(Guid.NewGuid(), _mockTrackerRepository);
            EntityMaker.CreateSomeEvent(tracker.Id, _mockEventRepository);
            
            var statsFact = new BiggestDayOverallStatsFact()
                .Apply(_mockEventRepository.GetAll());
            
            Assert.IsTrue(statsFact.IsNone);
        }
        
        [Test]
        public void BiggestDayOverallStatsFact_TwoEventsToday_CorrectEventsCountAndDay()
        {
            var tracker = EntityMaker.CreateSomeTracker(Guid.NewGuid(), _mockTrackerRepository);
            EntityMaker.CreateSomeEvent(tracker.Id, _mockEventRepository);
            EntityMaker.CreateSomeEvent(tracker.Id, _mockEventRepository);
            
            var statsFact = new BiggestDayOverallStatsFact()
                .Apply(_mockEventRepository.GetAll());

            Assert.AreEqual(2, ((BiggestDayOverallStatsFact)statsFact.ValueUnsafe()).EventsCount);
            Assert.AreEqual(DateTime.Now.Date, 
                ((BiggestDayOverallStatsFact)statsFact.ValueUnsafe()).BiggestDay);
        }
        
        [Test]
        public void BestTrackerEventStatsFact_OneEvent_CorrectBestEventRatingAndTitle()
        {
            var tracker = EntityMaker.CreateSomeTracker(Guid.NewGuid(), _mockTrackerRepository);
            var eventsWithRating = new Dictionary<Event, int>();
            var event1 = EntityMaker.CreateSomeEvent(tracker.Id, _mockEventRepository);
            eventsWithRating.Add(event1, 5);
            
            var statsFact = new BestTrackerEventStatsFact()
                .Apply(eventsWithRating);

            Assert.AreEqual(5, ((BestTrackerEventStatsFact)statsFact.ValueUnsafe()).BestRating);
            Assert.AreEqual(event1.Title, 
                ((BestTrackerEventStatsFact)statsFact.ValueUnsafe()).BestEvent.Title);
        }
        
        [Test]
        public void BestTrackerEventStatsFact_TwoEventsWithDifferentRating_CorrectBestEventRatingAndTitle()
        {
            var tracker = EntityMaker.CreateSomeTracker(Guid.NewGuid(), _mockTrackerRepository);
            var eventsWithRating = new Dictionary<Event, int>();
            var event1 = EntityMaker.CreateSomeEvent(tracker.Id, _mockEventRepository);
            eventsWithRating.Add(event1, 4);
            var event2 = EntityMaker.CreateSomeEvent(tracker.Id, _mockEventRepository);
            eventsWithRating.Add(event2, 5);
            
            var statsFact = new BestTrackerEventStatsFact()
                .Apply(eventsWithRating);

            Assert.AreEqual(5, ((BestTrackerEventStatsFact)statsFact.ValueUnsafe()).BestRating);
            Assert.AreEqual(event2.Title, 
                ((BestTrackerEventStatsFact)statsFact.ValueUnsafe()).BestEvent.Title);
        }
        
        [Test]
        public void BestTrackerEventStatsFact_TwoEventsWithSameRating_FactIsNone()
        {
            var tracker = EntityMaker.CreateSomeTracker(Guid.NewGuid(), _mockTrackerRepository);
            var eventsWithRating = new Dictionary<Event, int>();
            var event1 = EntityMaker.CreateSomeEvent(tracker.Id, _mockEventRepository);
            eventsWithRating.Add(event1, 5);
            var event2 = EntityMaker.CreateSomeEvent(tracker.Id, _mockEventRepository);
            eventsWithRating.Add(event2, 5);
            
            var statsFact = new BestTrackerEventStatsFact()
                .Apply(eventsWithRating);

            Assert.IsTrue(statsFact.IsNone);
        }

        private RepositoryMock<Tracker> _mockTrackerRepository;
        private RepositoryMock<Event> _mockEventRepository;
    }
}