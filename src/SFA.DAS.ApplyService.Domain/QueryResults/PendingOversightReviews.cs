using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Domain.QueryResults
{
    public class PendingOversightReviews
    {
        public List<PendingOversightReview> Reviews { get; set; }
    }

    public class PendingOversightReview
    {
        public Guid ApplicationId { get; set; }
        public string OrganisationName { get; set; }

        public string Ukprn { get; set; }
        public string ProviderRoute { get; set; }
        public string ApplicationReferenceNumber { get; set; }
        public DateTime ApplicationSubmittedDate { get; set; }
        public string GatewayReviewStatus { get; set; }
        public string FinancialReviewStatus { get; set; }
        public string ModerationReviewStatus { get; set; }
    }
}
