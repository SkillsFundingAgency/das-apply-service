using System;

namespace SFA.DAS.ApplyService.InternalApi.Types
{
    public class OrganisationSearchResult
    {
        public string Id { get; set; }
        public int? Ukprn { get; set; }

        public string Name => LegalName ?? TradingName ?? ProviderName;
        public string Email { get; set; }

        public OrganisationAddress Address { get; set; }
        public string OrganisationType { get; set; }

        public string OrganisationReferenceType { get; set; }
        public string OrganisationReferenceId { get; set; }


        public string TradingName { get; set; }
        public string LegalName { get; set; }
        public string ProviderName { get; set; }

        public string CompanyNumber { get; set; }
        public string CharityNumber { get; set; }

        public bool RoEPAOApproved { get; set; }
        public bool RoATPApproved { get; set; }

        public string EasApiOrganisationType { get; set; }
        
        public DateTime? FinancialDueDate { get; set; }
        public bool? FinancialExempt { get; set; }

        public bool OrganisationIsAlive { get; set; }
    }

    public class OrganisationAddress
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
    }
}
