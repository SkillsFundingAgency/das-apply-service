using System;
using System.Collections.Generic;
using SFA.DAS.ApplyService.Types;

namespace SFA.DAS.ApplyService.InternalApi.Types.QueryResults
{
    public class CompletedOversightReviews
    {
        public List<CompletedOversightReview> Reviews { get; set; }
    }

    public class CompletedOversightReview
    {
        public Guid ApplicationId { get; set; }
        public string OrganisationName { get; set; }

        public string Ukprn { get; set; }
        public string ProviderRoute { get; set; }
        public string ApplicationReferenceNumber { get; set; }
        public DateTime ApplicationSubmittedDate { get; set; }
        public OversightReviewStatus OversightStatus { get; set; }
        public DateTime ApplicationDeterminedDate { get; set; }
    }
}
