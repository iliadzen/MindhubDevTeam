using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using LanguageExt;

namespace ItHappened.Infrastructure
{
    public class EventRepository : CommonDbRepository<Event>
    {
        public EventRepository(CommonDbContext context) : base(context.Events, context)
        {
        }
    }
}