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
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }
}
