namespace SFA.DAS.ApplyService.Domain.Ukrlp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ProviderDetails
    {
        private const string LegalAddressIdentifier = "L";

        public string UKPRN { get; set; }
        public string ProviderName { get; set; }
        public string ProviderStatus { get; set; }
        public List<ProviderContact> ContactDetails { get; set; }
        public DateTime? VerificationDate { get; set; }
        public List<ProviderAlias> ProviderAliases { get; set; }
        public List<VerificationDetails> VerificationDetails { get; set; }

        public bool VerifiedByCompaniesHouse
        {
            get
            {
                return VerificationDetails.Any(x =>
                    x.VerificationAuthority == VerificationAuthorities.CompaniesHouseAuthority);
            }
        }

        public bool VerifiedbyCharityCommission
        {
            get
            {
                return VerificationDetails.Any(x =>
                    x.VerificationAuthority == VerificationAuthorities.CharityCommissionAuthority);
            }
        }

        public ProviderContact PrimaryContactDetails
        {
            get { return ContactDetails.FirstOrDefault(x => x.ContactType == LegalAddressIdentifier); }
        }

    }

    public class ProviderContact
    {
        public string ContactType { get; set; }
        public ContactAddress ContactAddress { get; set; }
        public ContactPersonalDetails ContactPersonalDetails { get; set; }
        public string ContactRole { get; set; }
        public string ContactTelephone1 { get; set; }
        public string ContactTelephone2 { get; set; }
        public string ContactWebsiteAddress { get; set; }
        public string ContactEmail { get; set; }
        public DateTime? LastUpdated { get; set; }
    }

    public class ProviderAlias
    {
        public string Alias { get; set; }
        public DateTime? LastUpdated { get; set; }
    }

    public class ContactAddress
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Town { get; set; }
        public string PostCode { get; set; }

        public string FormattedAddress()
        {
            var address = Address1 + "<br />";

            if (!String.IsNullOrWhiteSpace(Address2))
            {
                address = address + Address2 + "<br />";
            }
            if (!String.IsNullOrWhiteSpace(Address3))
            {
                address = address + Address3 + "<br />";
            }
            if (!String.IsNullOrWhiteSpace(Address4))
            {
                address = address + Address4 + "<br />";
            }
            if (!String.IsNullOrWhiteSpace(Town))
            {
                address = address + Town + "<br />";
            }
            if (!String.IsNullOrWhiteSpace(PostCode))
            {
                address = address + PostCode + "<br />";
            }

            return address;
        }
    }

    public class ContactPersonalDetails
    {
        public string PersonNameTitle { get; set; }
        public string PersonGivenName { get; set; }
        public string PersonFamilyName { get; set; }
        public string PersonNameSuffix { get; set; }
    }

    public class VerificationDetails
    {
        public string VerificationAuthority { get; set; }
        public string VerificationId { get; set; }
    }

    public class VerificationAuthorities
    {
        public const string CompaniesHouseAuthority = "Companies House";
        public const string CharityCommissionAuthority = "Charity Commission";
    }
}
