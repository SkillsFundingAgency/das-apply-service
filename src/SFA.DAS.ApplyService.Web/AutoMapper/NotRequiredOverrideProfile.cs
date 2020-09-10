using AutoMapper;

namespace SFA.DAS.ApplyService.Web.AutoMapper
{
    public class NotRequiredOverrideProfile : Profile
    {
        public NotRequiredOverrideProfile()
        {
            CreateMap<Configuration.NotRequiredOverride, Domain.Entities.NotRequiredOverride>()
                .ForMember(dest => dest.SectionId, opt => opt.MapFrom(source => source.SectionId))
                .ForMember(dest => dest.SequenceId, opt => opt.MapFrom(source => source.SequenceId))
                .ForMember(dest => dest.Conditions, opt => opt.MapFrom(source => source.Conditions))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }

    public class NotRequiredConditionProfile : Profile
    {
        public NotRequiredConditionProfile()
        {
            CreateMap<Configuration.NotRequiredCondition, Domain.Entities.NotRequiredCondition>()
                .ForMember(dest => dest.ConditionalCheckField, opt => opt.MapFrom(source => source.ConditionalCheckField))
                .ForMember(dest => dest.MustEqual, opt => opt.MapFrom(source => source.MustEqual))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(source => source.Value))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }
}
