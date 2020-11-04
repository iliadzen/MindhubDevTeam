using ItHappened.Domain.Customizations;

namespace ItHappened.Infrastructure
{
    public class PhotoRepository : CommonDbRepository<Photo>
    {
        public PhotoRepository(CommonDbContext context) : base(context.Photos, context)
        {
        }
    }
}