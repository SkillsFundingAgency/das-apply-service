using AutoMapper;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Web.Configuration;
using NotRequiredOverrideConfiguration = SFA.DAS.ApplyService.Web.Configuration.NotRequiredOverrideConfiguration;

namespace SFA.DAS.ApplyService.Web.AutoMapper
{
    public class NotRequiredOverridesProfile : Profile
    {
        public NotRequiredOverridesProfile()
        {
            CreateMap<NotRequiredOverrideConfiguration, NotRequiredOverride>()
                .ForMember(dest => dest.SectionId, opt => opt.MapFrom(source => source.SectionId))
                .ForMember(dest => dest.SequenceId, opt => opt.MapFrom(source => source.SequenceId))
                .ForMember(dest => dest.Conditions, opt => opt.MapFrom(source => source.Conditions))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }

    public class NotRequiredConditionsProfile : Profile
    {
        public NotRequiredConditionsProfile()
        {
            CreateMap<Configuration.NotRequiredCondition, Application.Apply.Roatp.NotRequiredCondition>()
                .ForMember(dest => dest.ConditionalCheckField, opt => opt.MapFrom(source => source.ConditionalCheckField))
                .ForMember(dest => dest.MustEqual, opt => opt.MapFrom(source => source.MustEqual))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(source => source.Value))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }
}
