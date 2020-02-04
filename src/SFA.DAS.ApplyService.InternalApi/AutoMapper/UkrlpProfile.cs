using AutoMapper;

namespace SFA.DAS.ApplyService.InternalApi.AutoMapper
{
    using Domain.Ukrlp;
    using SFA.DAS.ApplyService.InternalApi.Models.Ukrlp;
    using ContactAddress = Models.Ukrlp.ContactAddress;
    using ContactPersonalDetails = Models.Ukrlp.ContactPersonalDetails;
    using ProviderAlias = Models.Ukrlp.ProviderAlias;
    using ProviderContact = Models.Ukrlp.ProviderContact;
    using ProviderDetails = Models.Ukrlp.ProviderDetails;
    using VerificationDetails = Models.Ukrlp.VerificationDetails;

    public class UkrlpProviderDetailsProfile : Profile
    {
        public UkrlpProviderDetailsProfile()
        {
            CreateMap<MatchingProviderRecords, ProviderDetails>()
                .ForMember(dest => dest.UKPRN,
                    opt => opt.MapFrom(source => source.UnitedKingdomProviderReferenceNumber))
                .ForMember(dest => dest.ProviderName, opt => opt.MapFrom(source => source.ProviderName))
                .ForMember(dest => dest.ProviderStatus, opt => opt.MapFrom(source => source.ProviderStatus))
                .ForMember(dest => dest.ContactDetails, opt => opt.MapFrom(source => source.ProviderContacts))
                .ForMember(dest => dest.VerificationDate, opt => opt.MapFrom(source => source.ProviderVerificationDate))
                .ForMember(dest => dest.ProviderAliases, opt => opt.MapFrom(source => source.ProviderAliases))
                .ForMember(dest => dest.VerificationDetails, opt => opt.MapFrom(source => source.VerificationDetails));
        }
    }

    public class UkrlpProviderContactProfile : Profile
    {
        public UkrlpProviderContactProfile()
        {
            CreateMap<ProviderContactStructure, ProviderContact>()
                .ForMember(dest => dest.ContactType, opt => opt.MapFrom(source => source.ContactType))
                .ForMember(dest => dest.ContactAddress, opt => opt.MapFrom(source => source.ContactAddress))
                .ForMember(dest => dest.ContactPersonalDetails,
                    opt => opt.MapFrom(source => source.ContactPersonalDetails))
                .ForMember(dest => dest.ContactRole, opt => opt.MapFrom(source => source.ContactRole))
                .ForMember(dest => dest.ContactTelephone1, opt => opt.MapFrom(source => source.ContactTelephone1))
                .ForMember(dest => dest.ContactTelephone2, opt => opt.MapFrom(source => source.ContactTelephone2))
                .ForMember(dest => dest.ContactWebsiteAddress,
                    opt => opt.MapFrom(source => source.ContactWebsiteAddress))
                .ForMember(dest => dest.ContactEmail, opt => opt.MapFrom(source => source.ContactEmail))
                .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(source => source.LastUpdated));
        }
    }

    public class UkrlpProviderAliasProfile : Profile
    {
        public UkrlpProviderAliasProfile()
        {
            CreateMap<ProviderAliasesStructure, ProviderAlias>()
                .ForMember(dest => dest.Alias, opt => opt.MapFrom(source => source.ProviderAlias))
                .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(source => source.LastUpdated));
        }
    }

    public class UkrlpContactAddressProfile : Profile
    {
        public UkrlpContactAddressProfile()
        {
            CreateMap<ProviderContactAddress, ContactAddress>()
                .ForMember(dest => dest.Address1, opt => opt.MapFrom(source => source.Address1))
                .ForMember(dest => dest.Address2, opt => opt.MapFrom(source => source.Address2))
                .ForMember(dest => dest.Address3, opt => opt.MapFrom(source => source.Address3))
                .ForMember(dest => dest.Address4, opt => opt.MapFrom(source => source.Address4))
                .ForMember(dest => dest.Town, opt => opt.MapFrom(source => source.Town))
                .ForMember(dest => dest.PostCode, opt => opt.MapFrom(source => source.PostCode));
        }
    }

    public class UkrlpContactPersonalDetailsProfile : Profile
    {
        public UkrlpContactPersonalDetailsProfile()
        {
            CreateMap<ContactPersonalDetailsStructure, ContactPersonalDetails>()
                .ForMember(dest => dest.PersonNameTitle, opt => opt.MapFrom(source => source.PersonNameTitle))
                .ForMember(dest => dest.PersonGivenName, opt => opt.MapFrom(source => source.PersonGivenName))
                .ForMember(dest => dest.PersonFamilyName, opt => opt.MapFrom(source => source.PersonFamilyName))
                .ForMember(dest => dest.PersonNameSuffix, opt => opt.MapFrom(source => source.PersonNameSuffix));
        }
    }

    public class UkrlpVerificationDetailsProfile : Profile
    {
        public UkrlpVerificationDetailsProfile()
        {
            CreateMap<VerificationDetailsStructure, VerificationDetails>()
                .ForMember(dest => dest.VerificationAuthority,
                    opt => opt.MapFrom(source => source.VerificationAuthority))
                .ForMember(dest => dest.VerificationId, opt => opt.MapFrom(source => source.VerificationId))
                .ForMember(dest => dest.PrimaryVerificationSource, opt => opt.MapFrom(source => source.PrimaryVerificationSource));
        }
    }
}
