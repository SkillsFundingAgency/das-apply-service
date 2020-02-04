namespace SFA.DAS.ApplyService.InternalApi.AutoMapper
{
    using global::AutoMapper;
    using SFA.DAS.ApplyService.Domain.Roatp;
    using SFA.DAS.ApplyService.InternalApi.Models.Roatp;

    public class RoatpProfile : Profile
    {
        public RoatpProfile()
        {
            CreateMap<ProviderType, ApplicationRoute>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.RouteName, opt => opt.MapFrom(source => source.Type))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(source => source.Description));
        }
    }
}
