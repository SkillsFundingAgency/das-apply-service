using AutoMapper;

namespace SFA.DAS.ApplyService.InternalApi.AutoMapper
{
    public class ReferenceDataOrganisationProfile : Profile
    {
        public ReferenceDataOrganisationProfile()
        {
            CreateMap<Models.ReferenceData.Organisation, Types.Organisation>()
                .ForMember(dest => dest.Ukprn, opt => opt.ResolveUsing(source => { int.TryParse(source.Code, out var ukprn); return ukprn; }))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }
}
