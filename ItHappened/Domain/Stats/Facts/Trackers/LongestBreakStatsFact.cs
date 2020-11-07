using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain.Customizations;
using LanguageExt;

namespace ItHappened.Domain.Stats.Facts.Trackers
{
    public class LongestBreakStatsFact : IStatsFact
    {
        public StatsFactType Type => StatsFactType.LongestBreak;
        public string Description => $"Самый большой перерыв между событиями: {Break.Days} дней";
        public TimeSpan Break { get; private set; }

        public Option<IStatsFact> Apply(IEnumerable<Event> events)
        {
            if(events.Length() < 2) return Option<IStatsFact>.None;
            events = events.OrderBy(@event => @event.CreationDate);
            
            for(int i = 0, j = 1; j < events.Length(); i++, j++)
            {
                var @break = 
                    events.ElementAt(j).CreationDate.Date - events.ElementAt(i).CreationDate.Date;
                if (@break.Days > Break.Days)
                    Break = @break;
            }
            return this;
        }
    }
}