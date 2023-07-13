using System;

namespace SFA.DAS.ApplyService.Web.ViewModels
{
    public class ChangeSignInDetailsViewModel
    {
        private readonly string _integrationUrlPart = "";
        public ChangeSignInDetailsViewModel(string environment)
        {
            if (!environment.Equals("prd", StringComparison.CurrentCultureIgnoreCase))
            {
                _integrationUrlPart = ".integration";
            }
        }

        public string SettingsLink => $"https://home{_integrationUrlPart}.account.gov.uk/settings";
    }
}
