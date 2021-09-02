using SFA.DAS.ApplyService.Types;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Domain.QueryResults
{
    public class CompletedAppealOutcomes
    {
        public List<CompletedAppealOutcome> Reviews { get; set; }
    }
    public class CompletedAppealOutcome
    {
        public Guid ApplicationId { get; set; }
        public string OrganisationName { get; set; }
        public string Ukprn { get; set; }
        public string ProviderRoute { get; set; }
        public string ApplicationReferenceNumber { get; set; }
        public DateTime ApplicationSubmittedDate { get; set; }
        public DateTime AppealSubmittedDate { get; set; }
        public DateTime AppealDeterminedDate { get; set; }
        public OversightReviewStatus OversightStatus { get; set; }
        public string ApplicationStatus { get; set; }
    }
}