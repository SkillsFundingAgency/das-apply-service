using AutoMapper;

namespace SFA.DAS.ApplyService.InternalApi.AutoMapper
{
    public class AssessorServiceOrganisationProfile : Profile
    {
        public AssessorServiceOrganisationProfile()
        {
            CreateMap<Models.AssessorService.Organisation, Types.Organisation>()
                .ForMember(dest => dest.Ukprn, opt => opt.MapFrom(source => source.EndPointAssessorUkprn ?? 0))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.EndPointAssessorName))
                //.ForMember(dest => dest.Address, opt => opt.MapFrom(source => Mapper.Map<Models.AssessorService.OrganisationData, Types.OrganisationAddress>(source.OrganisationData)))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }
}
