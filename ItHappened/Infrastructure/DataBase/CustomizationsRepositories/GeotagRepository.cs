using ItHappened.Domain.Customizations;

namespace ItHappened.Infrastructure
{
    public class GeotagRepository : CommonDbRepository<Geotag>
    {
        public GeotagRepository(CommonDbContext context) : base(context.Geotags, context)
        {
        }
    }
}