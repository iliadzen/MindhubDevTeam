using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using LanguageExt;

namespace ItHappened.Infrastructure
{
    public class LicenseRepository : CommonDbRepository<License>
    {
        public LicenseRepository(CommonDbContext context) : base(context.Licenses, context)
        {
        }
    }
}