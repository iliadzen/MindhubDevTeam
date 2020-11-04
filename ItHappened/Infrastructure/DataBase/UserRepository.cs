using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ItHappened.Infrastructure
{
    public class UserRepository : CommonDbRepository<User>
    {
        public UserRepository(CommonDbContext context) : base(context.Users, context)
        {
        }
    }
}