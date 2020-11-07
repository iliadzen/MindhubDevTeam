using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace ItHappened.Domain.Stats.Facts.Trackers
{
    public class TheMostEventStatsFact : IStatsFact
    {
        public StatsFactType Type { get; }
        public string Description { get; }
        public int Rating { get; private set; }
        public Event Event {get; private set;}
        
        public Option<IStatsFact> FindMost(Dictionary<Event, int> eventsWithRating, Func<int, int, bool> Compare)
        {
            if(eventsWithRating.Count == 0) return  Option<IStatsFact>.None;
            Rating = eventsWithRating.ElementAt(0).Value;
            Event = eventsWithRating.ElementAt(0).Key;
            bool severalBestEvents = false;
            
            foreach (var eventWithRating in eventsWithRating)
            {
                if (Compare(eventWithRating.Value,Rating))
                {
                    Rating = eventWithRating.Value;
                    Event = eventWithRating.Key;
                    severalBestEvents = false;
                }

                if (eventWithRating.Value == Rating && eventWithRating.Key != Event)
                    severalBestEvents = true;
            }
            return severalBestEvents ? Option<IStatsFact>.None : this;
        }
    }
}