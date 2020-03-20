using AutoMapper;
using CharityCommissionService;
using SFA.DAS.ApplyService.Domain.CharityCommission;
using SFA.DAS.ApplyService.Domain.CompaniesHouse;
using System.Linq;
namespace SFA.DAS.ApplyService.InternalApi.AutoMapper
{
    public class UkrlpCharityCommissionProfile : Profile
    {
        public UkrlpCharityCommissionProfile()
        {
            CreateMap<InternalApi.Types.CharityCommission.Charity, CharityCommissionSummary>()
               .ForMember(dest => dest.CharityName, opt => opt.MapFrom(source => source.Name))
               .ForMember(dest => dest.CharityNumber, opt => opt.MapFrom(source => source.CharityNumber))
               .ForMember(dest => dest.IncorporatedOn, opt => opt.MapFrom(source => source.IncorporatedOn))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(source => source.Status))
               .ForMember(dest => dest.Type, opt => opt.MapFrom(source => source.Type))
               .ForMember(dest => dest.Trustees, opt => opt.MapFrom(source => source.Trustees))
               .ForMember(dest => dest.TrusteeManualEntryRequired, opt => opt.MapFrom(source => source.TrusteeManualEntryRequired))
               .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
    public class CharityTrusteeProfile : Profile
    {
        public CharityTrusteeProfile()
        {
            CreateMap<InternalApi.Types.CharityCommission.Trustee, Domain.CharityCommission.Trustee>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
