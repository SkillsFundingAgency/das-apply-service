using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.ApplyService.Web.Extensions
{
    public class RestoreModelStateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var tempDataFactory = filterContext.HttpContext.RequestServices.GetRequiredService<ITempDataDictionaryFactory>();
            var tempData = tempDataFactory.GetTempData(filterContext.HttpContext);
            var serializableModelState = tempData.Get<SerializableModelStateDictionary>();
            var modelState = serializableModelState?.ToModelState();

            filterContext.ModelState.Merge(modelState);
        }
    }
}
