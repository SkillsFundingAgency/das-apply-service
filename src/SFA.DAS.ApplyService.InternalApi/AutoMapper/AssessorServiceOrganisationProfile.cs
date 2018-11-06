using AutoMapper;

namespace SFA.DAS.ApplyService.InternalApi.AutoMapper
{
    public class AssessorServiceOrganisationProfile : Profile
    {
        public AssessorServiceOrganisationProfile()
        {
            CreateMap<Models.AssessorService.OrganisationSummary, Types.Organisation>()
                .BeforeMap((source, dest) => dest.OrganisationReferenceType = "RoEPAO")
                .ForMember(dest => dest.Ukprn, opt => opt.MapFrom(source => source.Ukprn))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
                .ForMember(dest => dest.OrganisationType, opt => opt.MapFrom(source => source.OrganisationType))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(source => Mapper.Map<Models.AssessorService.OrganisationData, Types.OrganisationAddress>(source.OrganisationData)))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }

    public class AssessorServiceOrganisationAddressProfile : Profile
    {
        public AssessorServiceOrganisationAddressProfile()
        {
            CreateMap<Models.AssessorService.OrganisationData, Types.OrganisationAddress>()
                .ForMember(dest => dest.Address1, opt => opt.MapFrom(source => source.Address1))
                .ForMember(dest => dest.Address2, opt => opt.MapFrom(source => source.Address2))
                .ForMember(dest => dest.Address3, opt => opt.MapFrom(source => source.Address3))
                .ForMember(dest => dest.City, opt => opt.MapFrom(source => source.Address4))
                .ForMember(dest => dest.Postcode, opt => opt.MapFrom(source => source.Postcode))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }
}
