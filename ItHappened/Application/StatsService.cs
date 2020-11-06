using System;
using System.Linq;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Stats;
using ItHappened.Domain.Stats.Facts.Trackers;

namespace ItHappened.Application
{
    public class StatsService
    {
        public static IEnumerable<IStatsFact> GetStatsFactsForTracker(Dictionary<Event, int> eventsWithRating)
        {
            return new List<IStatsFact>()
                .Concat(new BestTrackerEventStatsFact().Apply(eventsWithRating));
        }

        public static IEnumerable<IStatsFact> GetStatsFactsForUser(IEnumerable<Event> events)
        {
            return new List<IStatsFact>()
                .Concat(new ManyEventsOverallStatsFact().Apply(events))
                .Concat(new BiggestDayOverallStatsFact().Apply(events))
                .Concat(new BiggestWeekOverallStatsFact().Apply(events));
        }
    }
}