using System;

namespace SFA.DAS.ApplyService.Web.Infrastructure.FeatureToggles
{

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class FeatureToggleAttribute : Attribute
    {
        public string FeatureToogle { get; set; }

        public string RedirectToController { get; set; }

        public string RedirectToAction { get; set; }

        public FeatureToggleAttribute(string featureToogle, string redirectToController, string redirectToAction)
        {
            FeatureToogle = featureToogle;
            RedirectToController = redirectToController;
            RedirectToAction = redirectToAction;
        }
    }
}
