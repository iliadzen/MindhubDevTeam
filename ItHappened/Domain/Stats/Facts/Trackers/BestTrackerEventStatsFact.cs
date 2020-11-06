using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace ItHappened.Domain.Stats.Facts.Trackers
{
    public class BestTrackerEventStatsFact : IStatsFact
    {
        public StatsFactType Type => StatsFactType.BestEvent;
        public string Description => $"Самый высокий рейтинг {BestRating} у события {BestEvent.Title}";
        public int BestRating { get; private set; }
        public Event BestEvent {get; private set;}
        
        public Option<IStatsFact> Apply(Dictionary<Event, int> eventsWithRating)
        {
            if(eventsWithRating.Count == 0) return  Option<IStatsFact>.None;
            BestRating = eventsWithRating.ElementAt(0).Value;
            BestEvent = eventsWithRating.ElementAt(0).Key;
            bool severalBestEvents = false;
            
            foreach (var eventWithRating in eventsWithRating)
            {
                if (eventWithRating.Value > BestRating)
                {
                    BestRating = eventWithRating.Value;
                    BestEvent = eventWithRating.Key;
                    severalBestEvents = false;
                }

                if (eventWithRating.Value == BestRating && eventWithRating.Key != BestEvent)
                    severalBestEvents = true;
            }
            return severalBestEvents ? Option<IStatsFact>.None : this;
            return this;
        }
    }
}