using ItHappened.Domain.Customizations;

namespace ItHappened.Infrastructure
{
    public class RatingRepository : CommonDbRepository<Rating>
    {
        public RatingRepository(CommonDbContext context) : base(context.Ratings, context)
        {
        }
    }
}