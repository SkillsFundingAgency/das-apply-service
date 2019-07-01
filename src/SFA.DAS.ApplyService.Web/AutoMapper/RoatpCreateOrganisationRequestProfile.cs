using AutoMapper;

namespace SFA.DAS.ApplyService.Web.AutoMapper
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.CompaniesHouse;
    using Domain.Roatp;
    using Domain.Ukrlp;
    using InternalApi.Types;

    public class RoatpCreateOrganisationRequestProfile : Profile
    {
        public RoatpCreateOrganisationRequestProfile()
        {
            CreateMap<ApplicationDetails, CreateOrganisationRequest>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.UkrlpLookupDetails.ProviderName))
                .ForMember(dest => dest.OrganisationType,
                    opt => opt.MapFrom(source => ApplicationDetails.OrganisationType))
                .ForMember(dest => dest.OrganisationUkprn,
                    opt => opt.MapFrom(source => source.UkrlpLookupDetails.UKPRN))
                .ForMember(dest => dest.PrimaryContactEmail,
                    opt => opt.MapFrom(source => source.UkrlpLookupDetails.PrimaryContactDetails.ContactEmail))
                .ForMember(dest => dest.OrganisationDetails,
                    opt => opt.ResolveUsing(new RoatpOrganisationDetailsCustomResolver()))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(source => source.CreatedBy))                
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }

    public class RoatpOrganisationDetailsCustomResolver : IValueResolver<ApplicationDetails, CreateOrganisationRequest, OrganisationDetails>
    {
        public OrganisationDetails Resolve(ApplicationDetails source, CreateOrganisationRequest destination,
                                           OrganisationDetails destMember, ResolutionContext context)
        {
            destMember = new OrganisationDetails();

            destMember.Address1 = source.UkrlpLookupDetails?.PrimaryContactDetails?.ContactAddress.Address1;
            destMember.Address2 = source.UkrlpLookupDetails?.PrimaryContactDetails?.ContactAddress.Address2;
            destMember.Address3 = source.UkrlpLookupDetails?.PrimaryContactDetails?.ContactAddress.Address3;
            destMember.CharityNumber = source.CharitySummary?.CharityNumber;
            if (source.CharitySummary != null)
            {
                destMember.CharityCommissionDetails = new CharityCommissionDetails
                {
                    RegistrationDate = source.CharitySummary.IncorporatedOn,
                    Trustees = Mapper.Map<List<Domain.CharityCommission.Trustee>>(source.CharitySummary.Trustees),
                    TrusteeManualEntryRequired = source.CharitySummary.TrusteeManualEntryRequired
                };
            }

            destMember.City = source.UkrlpLookupDetails?.PrimaryContactDetails?.ContactAddress.Town;
            destMember.CompanyNumber = source.CompanySummary?.CompanyNumber;
            if (source.CompanySummary != null)
            {
                destMember.CompaniesHouseDetails = new CompaniesHouseDetails
                {
                    CompanyType = source.CompanySummary.CompanyTypeDescription,
                    IncorporationDate = source.CompanySummary.IncorporationDate,
                    Directors = Mapper.Map<List<DirectorInformation>>(source.CompanySummary.Directors),
                    PersonsSignificationControl =
                        Mapper.Map<List<PersonSignificantControlInformation>>(source.CompanySummary
                            .PersonsSignificationControl)
                };
            }

            destMember.LegalName = source.UkrlpLookupDetails?.ProviderName;
            destMember.ProviderName = source.UkrlpLookupDetails?.ProviderName;
            destMember.OrganisationReferenceId = source.UkrlpLookupDetails?.UKPRN;
            destMember.OrganisationReferenceType = "UKRLP";
            destMember.Postcode = source.UkrlpLookupDetails?.PrimaryContactDetails?.ContactAddress.PostCode;
            var primaryAlias = source.UkrlpLookupDetails?.ProviderAliases.FirstOrDefault();
            if (primaryAlias != null)
            {
                destMember.TradingName = primaryAlias.Alias;
            }

            destMember.UKRLPDetails = new UKRLPDetails
            {
                Alias = primaryAlias?.Alias,
                UKPRN = source.UkrlpLookupDetails?.UKPRN,
                OrganisationName = source.UkrlpLookupDetails?.ProviderName,
                ContactNumber = source.UkrlpLookupDetails.PrimaryContactDetails?.ContactTelephone1,
                Website = source.UkrlpLookupDetails?.PrimaryContactDetails?.ContactWebsiteAddress,
                PrimaryContactAddress = Mapper.Map<ContactAddress>(source.UkrlpLookupDetails?.PrimaryContactDetails),
                VerificationDetails = source.UkrlpLookupDetails?.VerificationDetails
            };

            destMember.FHADetails = new FHADetails();

            return destMember;
        }
    }

    public class RoatpContactAddressProfile : Profile
    {
        public RoatpContactAddressProfile()
        {
            CreateMap<ProviderContact, ContactAddress>()
                .ForMember(dest => dest.Address1, opt => opt.MapFrom(source => source.ContactAddress.Address1))
                .ForMember(dest => dest.Address2, opt => opt.MapFrom(source => source.ContactAddress.Address2))
                .ForMember(dest => dest.Address3, opt => opt.MapFrom(source => source.ContactAddress.Address3))
                .ForMember(dest => dest.Address4, opt => opt.MapFrom(source => source.ContactAddress.Address4))
                .ForMember(dest => dest.Town, opt => opt.MapFrom(source => source.ContactAddress.Town))
                .ForMember(dest => dest.PostCode, opt => opt.MapFrom(source => source.ContactAddress.PostCode));
        }
    }
}