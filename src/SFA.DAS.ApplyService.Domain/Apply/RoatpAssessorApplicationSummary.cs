using System;

namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class RoatpAssessorApplicationSummary
    {
        public Guid ApplicationId { get; set; }
        public string OrganisationName { get; set; }
        public string Ukprn { get; set; }
        public string ApplicationReferenceNumber { get; set; }
        public string ProviderRoute { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string Assessor1Name { get; set; }
        public string Assessor2Name { get; set; }
    }
}
