using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace ItHappened.Domain.Stats.Facts.Trackers
{
    public class WorseTrackerEventStatsFact : TheMostEventStatsFact, IStatsFact
    {
        public new StatsFactType Type => StatsFactType.WorstEvent;
        public new string Description => $"Самый низкий рейтинг {Rating} у события {Event.Title}";
        
        public Option<IStatsFact> Apply(Dictionary<Event, int> eventsWithRating)
        {
            return FindMost(eventsWithRating, (x, y) => x < y);
        }
    }
}