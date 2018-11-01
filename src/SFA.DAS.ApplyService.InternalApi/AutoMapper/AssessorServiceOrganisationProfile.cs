using AutoMapper;

namespace SFA.DAS.ApplyService.InternalApi.AutoMapper
{
    public class AssessorServiceOrganisationProfile : Profile
    {
        public AssessorServiceOrganisationProfile()
        {
            CreateMap<Models.AssessorService.OrganisationSummary, Types.Organisation>()
                .ForMember(dest => dest.Ukprn, opt => opt.MapFrom(source => source.Ukprn))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(source => Mapper.Map<Models.AssessorService.OrganisationData, Types.OrganisationAddress>(source.OrganisationData)))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(source => Mapper.Map<Models.AssessorService.OrganisationSummary, Types.OrganisationType>(source)))
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

    public class AssessorServiceOrganisationTypeProfile : Profile
    {
        public AssessorServiceOrganisationTypeProfile()
        {
            CreateMap<Models.AssessorService.OrganisationSummary, Types.OrganisationType>()
                .BeforeMap((source, dest) => dest.Status = null)
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.OrganisationTypeId))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(source => source.OrganisationType))
                .ForAllOtherMembers(dest => dest.Ignore());

            CreateMap<Models.AssessorService.OrganisationType, Types.OrganisationType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(source => source.Type))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(source => source.Status))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }
}
