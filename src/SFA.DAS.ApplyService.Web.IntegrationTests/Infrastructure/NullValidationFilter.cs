using Microsoft.AspNetCore.Mvc.Filters;

namespace SFA.DAS.ApplyService.Web.IntegrationTests.Infrastructure
{
    public class NullValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context){}

        public void OnActionExecuted(ActionExecutedContext context){}
    }
}