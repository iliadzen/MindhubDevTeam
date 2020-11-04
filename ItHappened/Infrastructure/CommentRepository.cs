using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Customizations;
using LanguageExt;

namespace ItHappened.Infrastructure
{
    public class CommentRepository : CommonDbRepository<Comment>
    {
        public CommentRepository(CommonDbContext context) : base(context.Comments, context)
        {
        }
    }
}