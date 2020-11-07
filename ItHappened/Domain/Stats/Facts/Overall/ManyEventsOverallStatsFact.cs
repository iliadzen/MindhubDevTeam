using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using static ItHappened.Domain.Stats.StatsFactType;

namespace ItHappened.Domain.Stats
{
    public class ManyEventsOverallStatsFact : IStatsFact
    {
        public StatsFactType Type => ManyEventsOverall;
        public string Description => $"У вас произошло уже {EventsCount} событий!";

        public int EventsCount { get; private set; }

        public Option<IStatsFact> Apply(IEnumerable<Event> events) =>
            events.Count() <= NotApplicableEventCount
                ? Option<IStatsFact>.None
                : new ManyEventsOverallStatsFact
                {
                    EventsCount = events.Count()
                };

        private const int NotApplicableEventCount = 5;
    }
}