using System;

namespace SFA.DAS.ApplyService.InternalApi.Models.AssessorService
{
    public class OrganisationData
    {
        public string CompanyNumber { get; set; }
        public string CharityNumber { get; set; }
        public string LegalName { get; set; }
        public string WebsiteLink { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Postcode { get; set; }
        public FHADetails FHADetails { get; set; }
    }

    public class FHADetails
    {
        public DateTime? FinancialDueDate { get; set; }
        public bool? FinancialExempt { get; set; }
    }
}
