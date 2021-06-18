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
        public DateTime? SubmittedDate { get; set; }
        public string ExternalComments { get; set; }
        public string EmailAddress { get; set; }
        public string FinancialGrade { get; set; }
        public string FinancialReviewStatus { get; set; }
        public string FinancialExternalComments { get; set; }
        public string GatewayReviewStatus { get; set; }
        public string ModerationStatus { get; set; }
        public string OversightInProgressExternalComments { get; set; }

        public int? SubcontractingLimit { get; set; }

        public string SubcontractingLimitFormatted => !string.IsNullOrEmpty(SubcontractingLimit?.ToString()) ?SubcontractingLimit?.ToString("N0") : null;

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
