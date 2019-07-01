namespace SFA.DAS.ApplyService.Domain.Entities
{
    using System;
    using SFA.DAS.ApplyService.Domain.CompaniesHouse;
    using System.Collections.Generic;
    using CharityCommission;
    using Ukrlp;

    public class Organisation : EntityBase
    {
        public string Name { get; set; }
        public string OrganisationType { get; set; }
        public int? OrganisationUkprn { get; set; }
        public OrganisationDetails OrganisationDetails { get; set; }

        public bool RoEPAOApproved { get; set; }
        public bool RoATPApproved { get; set; }
    }

    public class OrganisationDetails
    {
        public string OrganisationReferenceType { get; set; } // "RoEPAO", "RoATP" or "EASAPI"
        public string OrganisationReferenceId { get; set; } // CSV list of known id's

        public string LegalName { get; set; }
        public string TradingName { get; set; }
        public string ProviderName { get; set; }

        public string CompanyNumber { get; set; }
        public string CharityNumber { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string EndPointAssessmentOrgId { get; set; }
        public FHADetails FHADetails { get; set; }
        public UKRLPDetails UKRLPDetails { get; set; }
        public CompaniesHouseDetails CompaniesHouseDetails { get; set; }
        public CharityCommissionDetails CharityCommissionDetails { get; set; }
    }

    public class FHADetails
    {
        public DateTime? FinancialDueDate { get; set; }
        public bool? FinancialExempt { get; set; }
    }

    public class UKRLPDetails
    {
        public string UKPRN { get; set; }
        public string OrganisationName { get; set; }
        public ContactAddress PrimaryContactAddress { get; set; }
        public string ContactNumber { get; set; }
        public string Website { get; set; }
        public string Alias { get; set; }
        public List<VerificationDetails> VerificationDetails { get; set; }
    }

    public class CompaniesHouseDetails
    {
        public string CompanyType { get; set; }
        public List<DirectorInformation> Directors { get; set; }
        public List<PersonSignificantControlInformation> PersonsSignificationControl { get; set; }
        public DateTime? IncorporationDate { get; set; }
    }

    public class CharityCommissionDetails
    {
        public DateTime? RegistrationDate { get; set; }
        public List<Trustee> Trustees { get; set; }
        public bool TrusteeManualEntryRequired { get; set; }
    }
}