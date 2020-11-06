using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using static ItHappened.Domain.Stats.StatsFactType;

namespace ItHappened.Domain.Stats
{
    public class BiggestDayOverallStatsFact : IStatsFact
    {
        public StatsFactType Type => BiggestDayOverall;
        public string Description => $"Самый насыщенный событиями день был {BiggestDay:d}, " +
                                     $"событий в тот день: {EventsCount}";
        
        public int EventsCount { get; private set; }
        public DateTime BiggestDay { get; private set; }

        public Option<IStatsFact> Apply(IEnumerable<Event> events)
        {
            var biggestDay = events
                .GroupBy(e => e.CreationDate.Date)
                .Select(g => new {Date = g.Key, EventsCount = g.Count()})
                .OrderBy(day => day.EventsCount)
                .Last();

            return biggestDay.EventsCount <= NotApplicableEventCount
                ? Option<IStatsFact>.None
                : new BiggestDayOverallStatsFact
                {
                    BiggestDay = biggestDay.Date,
                    EventsCount = biggestDay.EventsCount
                };
        }
        
        private const int NotApplicableEventCount = 1;
    }
}