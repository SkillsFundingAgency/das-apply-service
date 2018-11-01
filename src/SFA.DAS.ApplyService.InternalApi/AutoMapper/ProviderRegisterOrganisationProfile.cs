using AutoMapper;
using System.Linq;

namespace SFA.DAS.ApplyService.InternalApi.AutoMapper
{
    public class ProviderRegisterOrganisationProfile : Profile
    {
        public ProviderRegisterOrganisationProfile()
        {
            CreateMap<Models.ProviderRegister.Provider, Types.Organisation>()
                .ForMember(dest => dest.Ukprn, opt => opt.ResolveUsing(source => { if (long.TryParse(source.Ukprn, out var ukprn)) { return ukprn as long?; } else { return null; } }))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.ProviderName))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(source => Mapper.Map<Models.ProviderRegister.Address, Types.OrganisationAddress>(source.Addresses.FirstOrDefault())))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }

    public class ProviderRegisterOrganisationAddressProfile : Profile
    {
        public ProviderRegisterOrganisationAddressProfile()
        {
            CreateMap<Models.ProviderRegister.Address, Types.OrganisationAddress>()
                .ForMember(dest => dest.Address1, opt => opt.MapFrom(source => source.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(source => source.Town))
                .ForMember(dest => dest.Postcode, opt => opt.MapFrom(source => source.PostCode))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }
}
