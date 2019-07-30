using SFA.DAS.ApplyService.Domain.CharityCommission;
using SFA.DAS.ApplyService.Domain.CompaniesHouse;
using SFA.DAS.ApplyService.Domain.Ukrlp;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.Types
{
    public class CreateOrganisationRequest
    {
        public string Name { get; set; }
        public string OrganisationType { get; set; }
        public int? OrganisationUkprn { get; set; }

        public bool RoEPAOApproved { get; set; }
        public bool RoATPApproved { get; set; }

        public OrganisationDetails OrganisationDetails { get; set; }

        public Guid CreatedBy { get; set; }
        public string PrimaryContactEmail { get; set; }
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
        public FHADetails FHADetails { get; set; }
        public UKRLPDetails UKRLPDetails { get; set; }
        public CompaniesHouseDetails CompaniesHouseDetails { get; set; }
        public CharityCommissionDetails CharityCommissionDetails { get; set; }
        public RoatpRegisterDetails RoatpDetails { get; set; }
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
        public string CompanyName { get; set; }
        public string CompanyType { get; set; }
        public List<DirectorInformation> Directors { get; set; }
        public List<PersonSignificantControlInformation> PersonsSignificationControl { get; set; }
        public DateTime? IncorporationDate { get; set; }
        public bool ManualEntryRequired { get; set; }
    }

    public class CharityCommissionDetails
    {
        public string CharityName { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public List<Trustee> Trustees { get; set; }
        public bool TrusteeManualEntryRequired { get; set; }
    }

    public class RoatpRegisterDetails
    {
        public bool UkprnOnRegister { get; set; }
        public Guid? OrganisationId { get; set; }
        public int? ProviderTypeId { get; set; }
        public int? StatusId { get; set; }
        public int? RemovedReasonId { get; set; }
        public DateTime? StatusDate { get; set; }
    }
}
