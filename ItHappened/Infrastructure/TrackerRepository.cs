using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using LanguageExt;

namespace ItHappened.Infrastructure
{
    public class TrackerRepository : CommonDbRepository<Tracker>
    {
        public TrackerRepository(CommonDbContext context) : base(context.Trackers, context)
        {
        }
    }
}