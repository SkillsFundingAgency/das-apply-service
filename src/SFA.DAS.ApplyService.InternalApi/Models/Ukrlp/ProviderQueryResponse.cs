namespace SFA.DAS.ApplyService.InternalApi.Models.Ukrlp
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [Serializable]
    public class MatchingProviderRecords
    {
        [XmlElement]
        public string UnitedKingdomProviderReferenceNumber { get; set; }

        [XmlElement]
        public string ProviderName { get; set; }

        [XmlElement]
        public string ProviderStatus { get; set; }

        [XmlArray(ElementName = "ProviderContact")]
        public List<ProviderContactStructure> ProviderContacts { get; set; }

        [XmlElement]
        public DateTime? ProviderVerificationDate { get; set; }

        [XmlElement]
        public List<ProviderAliasesStructure> ProviderAliases { get; set; }

        [XmlArray(ElementName = "VerificationDetails")]
        public List<VerificationDetailsStructure> VerificationDetails { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "ProviderContact")]
    public class ProviderContactStructure
    {
        [XmlElement]
        public string ContactType { get; set; }

        [XmlElement]
        public ProviderContactAddress ContactAddress { get; set; }
        
        [XmlElement]
        public ContactPersonalDetailsStructure ContactPersonalDetails { get; set; }

        [XmlElement]
        public string ContactRole { get; set; }

        [XmlElement]
        public string ContactTelephone1 { get; set; }

        [XmlElement]
        public string ContactTelephone2 { get; set; }

        [XmlElement]
        public string ContactWebsiteAddress { get; set; }

        [XmlElement]
        public string ContactEmail { get; set; }

        [XmlElement]
        public DateTime? LastUpdated { get; set; }
    }

    [Serializable]
    public class ProviderAliasesStructure
    {
        [XmlElement]
        public string ProviderAlias { get; set; }

        [XmlElement]
        public DateTime? LastUpdated { get; set; }
    }

    [Serializable]
    public class ProviderContactAddress
    {
        [XmlElement]
        public string Address1 { get; set; }

        [XmlElement]
        public string Address2 { get; set; }

        [XmlElement]
        public string Address3 { get; set; }
    
        [XmlElement]
        public string Address4 { get; set; }

        [XmlElement]
        public string Town { get; set; }

        [XmlElement]
        public string PostCode { get; set; }
    }

    [Serializable]
    public class ContactPersonalDetailsStructure
    {
        [XmlElement]
        public string PersonNameTitle { get; set; }

        [XmlElement]
        public string PersonGivenName { get; set; }

        [XmlElement]
        public string PersonFamilyName { get; set; }

        [XmlElement]
        public string PersonNameSuffix { get; set; }

        [XmlElement]
        public string PersonRequestedName { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "VerificationDetails")]
    public class VerificationDetailsStructure
    {
        [XmlElement]
        public string VerificationAuthority { get; set; }

        [XmlElement(ElementName = "VerificationID")]
        public string VerificationId { get; set; }

        [XmlElement]
        public bool PrimaryVerificationSource { get; set; }
    }
}
