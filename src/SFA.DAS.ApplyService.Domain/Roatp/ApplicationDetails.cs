namespace SFA.DAS.ApplyService.Domain.Roatp
{
    using System;
    using SFA.DAS.ApplyService.Domain.CharityCommission;
    using SFA.DAS.ApplyService.Domain.CompaniesHouse;
    using SFA.DAS.ApplyService.Domain.Ukrlp;

    public class ApplicationDetails
    {
        public ApplicationRoute ApplicationRoute { get; set; }
        public ProviderDetails UkrlpLookupDetails { get; set; }
        public long UKPRN { get; set; }
        public CompaniesHouseSummary CompanySummary { get; set; }
        public CharityCommissionSummary CharitySummary { get; set; }
        public OrganisationRegisterStatus RoatpRegisterStatus { get; set; }
        public Guid CreatedBy { get; set; }

        public const string OrganisationType = "TrainingProvider";
    }
}
