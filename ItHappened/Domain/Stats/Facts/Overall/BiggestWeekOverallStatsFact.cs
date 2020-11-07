using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using static System.DayOfWeek;
using static ItHappened.Domain.Stats.StatsFactType;

namespace ItHappened.Domain.Stats
{
    public class BiggestWeekOverallStatsFact : IStatsFact
    {
        public StatsFactType Type => BiggestWeekOverall;
        public string Description => "Самая насыщенная событиями неделя была " +
                                     $"с {BiggestWeekStart:d} до {BiggestWeekEnd:d}," +
                                     $"событий в ту неделю: {EventsCount}";
        
        public int EventsCount { get; private set; }
        public DateTime BiggestWeekStart { get; private set; }
        public DateTime BiggestWeekEnd { get; private set; }

        public Option<IStatsFact> Apply(IEnumerable<Event> events)
        {
            var biggestWeek = events
                .GroupBy(e => new
                {
                    Start = e.CreationDate.GetLast(Monday),
                    End = e.CreationDate.GetNext(Sunday)
                })
                .Select(g => new
                {
                    Week = g.Key,
                    EventsCount = g.Count()
                })
                .OrderBy(week => week.EventsCount)
                .Last();

            return biggestWeek.EventsCount <= NotApplicableEventCount
                ? Option<IStatsFact>.None
                : new BiggestWeekOverallStatsFact()
                {
                    BiggestWeekStart = biggestWeek.Week.Start,
                    BiggestWeekEnd = biggestWeek.Week.End,
                    EventsCount = biggestWeek.EventsCount
                };
        }
        
        private const int NotApplicableEventCount = 1;
    }
}