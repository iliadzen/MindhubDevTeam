using System;
using System.Linq;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Stats;

namespace ItHappened.Application
{
    public class StatsService
    {
        private readonly IRepository<Event> _eventRepository;
        private readonly IRepository<Tracker> _trackerRepository;
        
        public StatsService(IRepository<Event> eventRepository, IRepository<Tracker> trackerRepository)
        {
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _trackerRepository = trackerRepository ?? throw new ArgumentNullException(nameof(trackerRepository));
        }

        private IEnumerable<Guid> GetTrackersForUser(Guid userId) =>
            _trackerRepository.GetAll()
                .Where(t => t.UserId == userId)
                .Select(t => t.Id);

        public IEnumerable<IStatsFact> GetStatsFactsForUser(Guid userId) =>
            GetStatsFactsForTrackers(GetTrackersForUser(userId));

        public IEnumerable<IStatsFact> GetStatsFactsForTracker(Guid trackerId) =>
            GetStatsFactsForTrackers(new[] {trackerId});

        public IEnumerable<IStatsFact> GetStatsFactsForTrackers(IEnumerable<Guid> trackerIds)
        {
            HashSet<Guid> trackerIdSet = new HashSet<Guid>(trackerIds);
            Event[] events = _eventRepository.GetAll()
                .Where(e => trackerIdSet.Contains(e.TrackerId)).ToArray();

            return new List<IStatsFact>()
                .Concat(new ManyEventsOverallStatsFact().Apply(events))
                .Concat(new BiggestDayOverallStatsFact().Apply(events))
                .Concat(new BiggestWeekOverallStatsFact().Apply(events));
        }
    }
}