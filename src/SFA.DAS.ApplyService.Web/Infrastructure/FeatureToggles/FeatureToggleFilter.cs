using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Configuration;

namespace SFA.DAS.ApplyService.Web.Infrastructure.FeatureToggles
{
    public class FeatureToggleFilter : IActionFilter
    {
        private ILogger<FeatureToggleFilter> _logger;
        private readonly IFeatureToggles _featureToggles;

        public FeatureToggleFilter(ILogger<FeatureToggleFilter> logger, IFeatureToggles featureToggles)
        {
            _logger = logger;
            _featureToggles = featureToggles;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var filterAttribute = context.Controller.GetType().GetTypeInfo().GetCustomAttribute<FeatureToggleAttribute>();

            if (filterAttribute != null && !IsFeatureToggleEnabled(_featureToggles, filterAttribute.FeatureToogle))
            {
                if (string.IsNullOrWhiteSpace(filterAttribute.RedirectToController) || string.IsNullOrWhiteSpace(filterAttribute.RedirectToAction))
                {
                    _logger.LogInformation($"{filterAttribute.FeatureToogle} not enabled - Redirecting to default route");
                    context.Result = new RedirectToRouteResult("default", null);
                }
                else
                {
                    _logger.LogInformation($"{filterAttribute.FeatureToogle} not enabled - Redirecting to controller: {filterAttribute.RedirectToController} | action: {filterAttribute.RedirectToAction}");
                    context.Result = new RedirectToActionResult(filterAttribute.RedirectToAction, filterAttribute.RedirectToController, null);
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

        private static bool IsFeatureToggleEnabled(IFeatureToggles _featureToggles, string featureToggle)
        {
            var toggleEnabled = false;

            if (_featureToggles != null && !string.IsNullOrWhiteSpace(featureToggle))
            {
                try
                {
                    var property = _featureToggles.GetType().GetProperty(featureToggle);
                    var propertyValue = property.GetValue(_featureToggles, null);

                    if (propertyValue != null)
                    {
                        toggleEnabled = Convert.ToBoolean(propertyValue);
                    }
                }
                catch (SystemException ex) when (ex is InvalidCastException || ex is FormatException)
                {
                    toggleEnabled = false;
                }
            }

            return toggleEnabled;
        }
    }
}
