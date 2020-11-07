using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace ItHappened.Domain.Stats.Facts.Trackers
{
    public class BestTrackerEventStatsFact : TheMostEventStatsFact, IStatsFact
    {
        public new StatsFactType Type => StatsFactType.BestEvent;
        public new string Description => $"Самый высокий рейтинг {Rating} у события {Event.Title}";

        public Option<IStatsFact> Apply(Dictionary<Event, int> eventsWithRating)
        {
            return FindMost(eventsWithRating, (x, y) => x > y);
        }
    }
}