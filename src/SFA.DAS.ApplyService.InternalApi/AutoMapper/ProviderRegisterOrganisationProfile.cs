using AutoMapper;
using System.Linq;

namespace SFA.DAS.ApplyService.InternalApi.AutoMapper
{
    public class ProviderRegisterOrganisationProfile : Profile
    {
        public ProviderRegisterOrganisationProfile()
        {
            CreateMap<Models.ProviderRegister.Provider, Types.Organisation>()
                .ForMember(dest => dest.Ukprn, opt => opt.MapFrom(source => source.Ukprn))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.ProviderName))
                //.ForMember(dest => dest.Address, opt => opt.MapFrom(source => Mapper.Map<Models.ProviderRegister.Address, Types.OrganisationAddress>(source.Addresses.FirstOrDefault())))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }
}
