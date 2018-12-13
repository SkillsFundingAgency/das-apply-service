using AutoMapper;
using System.Linq;

namespace SFA.DAS.ApplyService.InternalApi.AutoMapper
{
    public class ProviderRegisterOrganisationProfile : Profile
    {
        public ProviderRegisterOrganisationProfile()
        {
            CreateMap<Models.ProviderRegister.Provider, Types.OrganisationSearchResult>()
                .BeforeMap((source, dest) => dest.OrganisationReferenceType = "RoATP")
                .BeforeMap((source, dest) => dest.OrganisationType = "Training Provider")
                .BeforeMap((source, dest) => dest.RoATPApproved = true)
                .ForMember(dest => dest.Ukprn, opt => opt.MapFrom(source => source.Ukprn))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Ukprn))
                .ForMember(dest => dest.ProviderName, opt => opt.MapFrom(source => source.ProviderName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(source => source.Email))
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
