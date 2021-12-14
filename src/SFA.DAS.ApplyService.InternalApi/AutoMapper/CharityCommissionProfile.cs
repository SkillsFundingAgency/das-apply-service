using AutoMapper;

namespace SFA.DAS.ApplyService.InternalApi.AutoMapper
{
    public class CharityCommissionProfile : Profile
    {
        public CharityCommissionProfile()
        {
            CreateMap<Infrastructure.CharityCommission.Entities.Charity, Types.CharityCommission.Charity>()
                .ForMember(dest => dest.CharityNumber, opt => opt.MapFrom(source => source.RegisteredCharityNumber))
                .ForMember(dest => dest.CompanyNumber, opt => opt.MapFrom(source => source.RegisteredCompanyNumber))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.CharityName.Trim()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(source => source.Status))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(source => source.Type))
                .ForMember(dest => dest.NatureOfBusiness, opt => opt.MapFrom(source => source.NatureOfBusiness))
                .ForMember(dest => dest.IncorporatedOn, opt => opt.MapFrom(source => source.RegistrationDate))
                .ForMember(dest => dest.DissolvedOn, opt => opt.MapFrom(source => source.RegistrationRemovalDate))
                .ForMember(dest => dest.TelephoneNumber, opt => opt.MapFrom(source => source.TelephoneNumber))
                .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(source => source.EmailAddress))
                .ForMember(dest => dest.Website, opt => opt.MapFrom(source => source.WebsiteAddress))
                .ForMember(dest => dest.RegisteredOfficeAddress, opt => opt.MapFrom(source => Mapper.Map<Infrastructure.CharityCommission.Entities.Charity, Types.CharityCommission.Address>(source)))
                .ForMember(dest => dest.Accounts, opt => opt.MapFrom(source => Mapper.Map<Infrastructure.CharityCommission.Entities.Charity, Types.CharityCommission.Accounts>(source)))
                .ForMember(dest => dest.Trustees, opt => opt.MapFrom(source => Mapper.Map<Infrastructure.CharityCommission.Entities.Trustee[], Types.CharityCommission.Trustee[]>(source.Trustees)))
                .ForMember(dest => dest.TrusteeManualEntryRequired, opt => opt.MapFrom(source => source.Trustees == null || source.Trustees.Length == 0))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }

    public class CharityCommissionAddressProfile : Profile
    {
        public CharityCommissionAddressProfile()
        {
            CreateMap<Infrastructure.CharityCommission.Entities.Charity, Types.CharityCommission.Address>()
                .BeforeMap((source, dest) => dest.Country = "United Kingdom")
                .ForMember(dest => dest.AddressLine1, opt => opt.MapFrom(source => source.AddressLine1))
                .ForMember(dest => dest.AddressLine2, opt => opt.MapFrom(source => string.IsNullOrEmpty(source.AddressLine3) ? null : source.AddressLine2)) // sometimes city is on line 2
                .ForMember(dest => dest.City, opt => opt.MapFrom(source => source.AddressLine3 ?? source.AddressLine2)) // cope for when it is on line 2, instead of line 3
                .ForMember(dest => dest.County, opt => opt.MapFrom(source => source.AddressLine5 ?? source.AddressLine4))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(source => source.AddressPostcode))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }

    public class CharityCommissionAccountsProfile : Profile
    {
        public CharityCommissionAccountsProfile()
        {
            CreateMap<Infrastructure.CharityCommission.Entities.Charity, Types.CharityCommission.Accounts>()
                .ForMember(dest => dest.LastAccountsDate, opt => opt.MapFrom(source => source.LatestAccountsEndDate))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }

    public class CharityCommissionTrusteeProfile : Profile
    {
        public CharityCommissionTrusteeProfile()
        {
            CreateMap<Infrastructure.CharityCommission.Entities.Trustee, Types.CharityCommission.Trustee>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }
}
