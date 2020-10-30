using System;
using System.Linq;
using LanguageExt;
using static System.DayOfWeek;
using static ItHappened.Domain.Stats.StatsFactType;

namespace ItHappened.Domain.Stats
{
    public class ManyEventsOverallStatsFact : IStatsFact
    {
        public StatsFactType Type => ManyEventsOverall;
        public double Priority => Math.Log(EventsCount);
        public string Description => $"У вас произошло уже {EventsCount} событий!";

        public int EventsCount { get; private set; }

        public Option<IStatsFact> Apply(Event[] events) =>
            events.Length <= NotApplicableEventCount
                ? Option<IStatsFact>.None
                : new ManyEventsOverallStatsFact
                {
                    EventsCount = events.Length
                };

        private const int NotApplicableEventCount = 5;
    }
    
    public class BiggestDayOverallStatsFact : IStatsFact
    {
        public StatsFactType Type => BiggestDayOverall;
        public double Priority => 1.5 * EventsCount;
        public string Description => $"Самый насыщенный событиями день был {BiggestDay:d}, " +
                                     $"событий в тот день: {EventsCount}";
        
        public int EventsCount { get; private set; }
        public DateTime BiggestDay { get; private set; }

        public Option<IStatsFact> Apply(Event[] events)
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
    
    public class BiggestWeekOverallStatsFact : IStatsFact
    {
        public StatsFactType Type => BiggestWeekOverall;
        public double Priority => 0.75 * EventsCount;
        public string Description => "Самая насыщенная событиями неделя была " +
                                     $"с {BiggestWeekStart:d} до {BiggestWeekEnd:d}," +
                                     $"событий в ту неделю: {EventsCount}";
        
        public int EventsCount { get; private set; }
        public DateTime BiggestWeekStart { get; private set; }
        public DateTime BiggestWeekEnd { get; private set; }

        public Option<IStatsFact> Apply(Event[] events)
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