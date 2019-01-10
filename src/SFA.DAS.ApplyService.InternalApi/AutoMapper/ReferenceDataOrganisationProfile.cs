using AutoMapper;
using SFA.DAS.ApplyService.InternalApi.Models.ReferenceData;

namespace SFA.DAS.ApplyService.InternalApi.AutoMapper
{
    public class ReferenceDataOrganisationProfile : Profile
    {
        public ReferenceDataOrganisationProfile()
        {
            CreateMap<Organisation, Types.OrganisationSearchResult>()
                .BeforeMap((source, dest) => dest.Ukprn = null)
                .BeforeMap((source, dest) => dest.OrganisationReferenceType = "EASAPI")
                .BeforeMap((source, dest) => dest.Email = null)
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Code))
                .ForMember(dest => dest.LegalName, opt => opt.MapFrom(source => source.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(source => Mapper.Map<Address, Types.OrganisationAddress>(source.Address)))
                .ForMember(dest => dest.CompanyNumber, opt => opt.ResolveUsing(source => source.Type == OrganisationType.Company ? source.Code : null))
                .ForMember(dest => dest.CharityNumber, opt => opt.ResolveUsing(source => source.Type == OrganisationType.Charity ? source.Code : null))
                .ForMember(dest => dest.EasApiOrganisationType, opt => opt.MapFrom(source => source.Type.ToString()))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }

    public class ReferenceDataOrganisationAddressProfile : Profile
    {
        public ReferenceDataOrganisationAddressProfile()
        {
            CreateMap<Address, Types.OrganisationAddress>()
                .ForMember(dest => dest.Address1, opt => opt.MapFrom(source => source.Line1))
                .ForMember(dest => dest.Address2, opt => opt.MapFrom(source => source.Line2))
                .ForMember(dest => dest.Address3, opt => opt.MapFrom(source => source.Line3))
                .ForMember(dest => dest.City, opt => opt.MapFrom(source => source.Line4))
                .ForMember(dest => dest.Postcode, opt => opt.MapFrom(source => source.Postcode))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }
}
