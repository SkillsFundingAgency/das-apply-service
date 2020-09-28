using System;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class ApplicationSummaryViewModel
    {
        public Guid ApplicationId { get; set; }
        public string ApplicationReference { get; set; }
        public string UKPRN { get; set; }
        public string OrganisationName { get; set; }
        public string TradingName { get; set; }
        public string ApplicationRouteId { get; set; }
        public string EmailAddress { get; set; }

        public string ApplicationRouteShortText
        {
            get
            {
                switch (ApplicationRouteId)
                {
                    case "1":
                        {
                            return "Main";
                        }
                    case "2":
                        {
                            return "Employer";
                        }
                    case "3":
                        {
                            return "Supporting";
                        }
                    default:
                        {
                            return "(not set)";
                        }
                }
            }
        }

    }
}
