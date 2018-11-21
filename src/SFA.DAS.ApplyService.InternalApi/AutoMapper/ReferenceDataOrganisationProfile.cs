using AutoMapper;

namespace SFA.DAS.ApplyService.InternalApi.AutoMapper
{
    public class ReferenceDataOrganisationProfile : Profile
    {
        public ReferenceDataOrganisationProfile()
        {
            CreateMap<Models.ReferenceData.Organisation, Types.OrganisationSearchResult>()
                .BeforeMap((source, dest) => dest.Ukprn = null)
                .BeforeMap((source, dest) => dest.OrganisationReferenceType = "EASAPI")
                .BeforeMap((source, dest) => dest.Email = null)
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Code))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(source => Mapper.Map<Models.ReferenceData.Address, Types.OrganisationAddress>(source.Address)))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }

    public class ReferenceDataOrganisationAddressProfile : Profile
    {
        public ReferenceDataOrganisationAddressProfile()
        {
            CreateMap<Models.ReferenceData.Address, Types.OrganisationAddress>()
                .ForMember(dest => dest.Address1, opt => opt.MapFrom(source => source.Line1))
                .ForMember(dest => dest.Address2, opt => opt.MapFrom(source => source.Line2))
                .ForMember(dest => dest.Address3, opt => opt.MapFrom(source => source.Line3))
                .ForMember(dest => dest.City, opt => opt.MapFrom(source => source.Line4))
                .ForMember(dest => dest.Postcode, opt => opt.MapFrom(source => source.Postcode))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }
}
