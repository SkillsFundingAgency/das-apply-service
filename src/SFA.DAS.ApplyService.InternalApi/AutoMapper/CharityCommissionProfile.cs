using AutoMapper;

namespace SFA.DAS.ApplyService.InternalApi.AutoMapper
{
    public class CharityCommissionProfile : Profile
    {
        public CharityCommissionProfile()
        {
            CreateMap<CharityCommissionService.Charity, Types.CharityCommission.Charity>()
                .ForMember(dest => dest.CharityNumber, opt => opt.MapFrom(source => source.RegisteredCharityNumber))
                .ForMember(dest => dest.CompanyNumber, opt => opt.MapFrom(source => source.RegisteredCompanyNumber))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.CharityName.Trim()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(source => source.Status))
                .ForMember(dest => dest.NatureOfBusiness, opt => opt.MapFrom(source => source.NatureOfBusiness))
                .ForMember(dest => dest.IncorporatedOn, opt => opt.MapFrom(source => source.RegistrationDate))
                .ForMember(dest => dest.DissolvedOn, opt => opt.MapFrom(source => source.RegistrationRemovalDate))
                .ForMember(dest => dest.RegisteredOfficeAddress, opt => opt.MapFrom(source => Mapper.Map<CharityCommissionService.Address, Types.CharityCommission.Address>(source.Address))) 
                .ForMember(dest => dest.Accounts, opt => opt.MapFrom(source => Mapper.Map<CharityCommissionService.LatestFiling, Types.CharityCommission.Accounts>(source.LatestFiling)))
                .ForMember(dest => dest.Trustees, opt => opt.MapFrom(source => Mapper.Map<CharityCommissionService.Trustee[], Types.CharityCommission.Trustee[]>(source.Trustees)))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }

    public class CharityCommissionAddressProfile : Profile
    {
        public CharityCommissionAddressProfile()
        {
            CreateMap<CharityCommissionService.Address, Types.CharityCommission.Address>()
                .ForMember(dest => dest.AddressLine1, opt => opt.MapFrom(source => source.Line1))
                .ForMember(dest => dest.AddressLine2, opt => opt.MapFrom(source => source.Line2))
                .ForMember(dest => dest.City, opt => opt.MapFrom(source => source.Line3)) // not sure what line 3 is
                .ForMember(dest => dest.County, opt => opt.MapFrom(source => source.Line4)) // not sure what line 4 is
                .ForMember(dest => dest.Country, opt => opt.MapFrom(source => source.Line5)) // not sure what line 5 is
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(source => source.Postcode))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }

    public class CharityCommissionAccountsProfile : Profile
    {
        public CharityCommissionAccountsProfile()
        {
            CreateMap<CharityCommissionService.LatestFiling, Types.CharityCommission.Accounts>()
                .ForMember(dest => dest.LastAccountsDate, opt => opt.ResolveUsing(source => source.AccountsPeriodDateTime > source.AnnualReturnPeriodDateTime ? source.AccountsPeriodDateTime : source.AnnualReturnPeriodDateTime))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }

    public class CharityCommissionTrusteeProfile : Profile
    {
        public CharityCommissionTrusteeProfile()
        {
            CreateMap<CharityCommissionService.Trustee, Types.CharityCommission.Trustee>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.TrusteeNumber))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.TrusteeName))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }
}
