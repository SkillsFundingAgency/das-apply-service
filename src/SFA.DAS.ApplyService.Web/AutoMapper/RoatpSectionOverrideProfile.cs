using AutoMapper;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Web.Configuration;

namespace SFA.DAS.ApplyService.Web.AutoMapper
{
    public class RoatpSectionOverrideProfile : Profile
    {
        public RoatpSectionOverrideProfile()
        {
            CreateMap<NotRequiredOverrideConfiguration, NotRequiredSectionOverride>()
                .ForMember(dest => dest.SectionId, opt => opt.MapFrom(source => source.SectionId))
                .ForMember(dest => dest.SequenceId, opt => opt.MapFrom(source => source.SequenceId))
                .ForMember(dest => dest.Conditions, opt => opt.MapFrom(source => source.Conditions));

            CreateMap<Configuration.NotRequiredCondition, Domain.Entities.NotRequiredCondition>()
                .ForMember(dest => dest.ConditionalCheckField, opt => opt.MapFrom(source => source.ConditionalCheckField))
                .ForMember(dest => dest.MustEqual, opt => opt.MapFrom(source => source.MustEqual))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(source => source.Value));
        }
    }
}
