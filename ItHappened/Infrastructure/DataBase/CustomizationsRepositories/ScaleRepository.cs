using ItHappened.Domain.Customizations;

namespace ItHappened.Infrastructure
{
    public class ScaleRepository : CommonDbRepository<Scale>
    {
        public ScaleRepository(CommonDbContext context) : base(context.Scales, context)
        {
        }
    }
}