using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SFA.DAS.ApplyService.Configuration;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class PerformValidationFilter : IActionFilter
    {
        private readonly IConfigurationService _configurationService;

        public PerformValidationFilter(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }
        
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var config = _configurationService.GetConfig().Result;
            
            var performValidationAttribute = context.ActionDescriptor.FilterDescriptors.Select(x => x.Filter)
                .OfType<PerformValidationAttribute>().FirstOrDefault();
            if (performValidationAttribute == null) return;
            
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(config.InternalApi.Uri);
                var response = httpClient.PostAsJsonAsync(performValidationAttribute.ValidationEndpoint,
                    context.ActionArguments.Values.First()).Result;
                var content = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode != HttpStatusCode.BadRequest) return;
                    
                var json = (JObject)JsonConvert.DeserializeObject(content);
                foreach (var fieldError in json)
                {
                    var errorArray = JArray.Parse(fieldError.Value.ToString());
                    foreach (var error in errorArray)
                    {
                        context.ModelState.AddModelError(fieldError.Key, error.ToString());
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context){}
    }
}