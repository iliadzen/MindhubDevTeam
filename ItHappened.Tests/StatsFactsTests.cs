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

            Assert.AreEqual(5, ((BestTrackerEventStatsFact)statsFact.ValueUnsafe()).Rating);
            Assert.AreEqual(event1.Title, 
                ((BestTrackerEventStatsFact)statsFact.ValueUnsafe()).Event.Title);
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

            Assert.AreEqual(5, ((BestTrackerEventStatsFact)statsFact.ValueUnsafe()).Rating);
            Assert.AreEqual(event2.Title, 
                ((BestTrackerEventStatsFact)statsFact.ValueUnsafe()).Event.Title);
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
        
        
        [Test]
        public void LongestBreakStatsFact_OnlyOneEvent_FactIsNone()
        {
            var tracker = EntityMaker.CreateSomeTracker(Guid.NewGuid(), _mockTrackerRepository);
            EntityMaker.CreateSomeEvent(tracker.Id, _mockEventRepository);
            
            var statsFact = new LongestBreakStatsFact()
                .Apply(_mockEventRepository.GetAll());

            Assert.IsTrue(statsFact.IsNone);
        }

        [Test]
        public void LongestBreakStatsFact_TwoEventsWithThreeDaysBreak_CorrectBreak()
        {
            var tracker = EntityMaker.CreateSomeTracker(Guid.NewGuid(), _mockTrackerRepository);
            var date1 = new DateTime(2020, 1, 1, 1, 0, 0);
            var date2 = new DateTime(2020, 1, 4, 2, 0, 0);
            var event1 = new Event(Guid.NewGuid(), tracker.Id, "Event1", date1, date1);
            var event2 = new Event(Guid.NewGuid(), tracker.Id, "Event2", date2, date2);
            _mockEventRepository.Save(event1);
            _mockEventRepository.Save(event2);
            
            var statsFact = new LongestBreakStatsFact()
                .Apply(_mockEventRepository.GetAll());

            Assert.AreEqual(3, ((LongestBreakStatsFact)statsFact).Break.Days);
        }
        
        [Test]
        public void LongestBreakStatsFact_ThreeEventsWithMaxFourDaysBreak_CorrectBreak()
        {
            var tracker = EntityMaker.CreateSomeTracker(Guid.NewGuid(), _mockTrackerRepository);
            var date1 = new DateTime(2020, 1, 1, 1, 0, 0);
            var date2 = new DateTime(2020, 1, 5, 2, 0, 0);
            var date3 = new DateTime(2020, 1, 6, 3, 0, 0);
            var event1 = new Event(Guid.NewGuid(), tracker.Id, "Event1", date1, date1);
            var event2 = new Event(Guid.NewGuid(), tracker.Id, "Event2", date2, date2);
            var event3 = new Event(Guid.NewGuid(), tracker.Id, "Event3", date3, date3);
            _mockEventRepository.Save(event1);
            _mockEventRepository.Save(event2);
            _mockEventRepository.Save(event3);
            
            var statsFact = new LongestBreakStatsFact()
                .Apply(_mockEventRepository.GetAll());

            Assert.AreEqual(4, ((LongestBreakStatsFact)statsFact).Break.Days);
        }

        private RepositoryMock<Tracker> _mockTrackerRepository;
        private RepositoryMock<Event> _mockEventRepository;
    }
}