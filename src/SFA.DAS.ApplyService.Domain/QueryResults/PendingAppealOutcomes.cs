using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.ApplyService.Domain.QueryResults
{
  
    public class PendingAppealOutcomes
    {
        public List<PendingAppealOutcome> Reviews { get; set; }
    }

    public class PendingAppealOutcome
    {
        public Guid ApplicationId { get; set; }
        public string OrganisationName { get; set; }
        public string Ukprn { get; set; }
        public string ProviderRoute { get; set; }
        public string ApplicationReferenceNumber { get; set; }
        public DateTime ApplicationSubmittedDate { get; set; }
        public DateTime ApplicationDeterminedDate { get; set; }
        public DateTime AppealSubmitedDate { get; set; }
        public string AppealStatus { get; set; }

    }
}
