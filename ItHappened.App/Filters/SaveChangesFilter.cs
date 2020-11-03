using ItHappened.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ItHappened.App.Filters
{
    public class SaveChangesFilter : ActionFilterAttribute
    {
        private readonly CommonDbContext _dbContext;

        public SaveChangesFilter(CommonDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null)
            {
                _dbContext.SaveChanges();
            }
        }
    }
}