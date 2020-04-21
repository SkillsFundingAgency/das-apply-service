using AutoMapper;
using SFA.DAS.ApplyService.Domain.CompaniesHouse;
using System.Linq;

namespace SFA.DAS.ApplyService.Web.AutoMapper
{
    public class CompaniesHouseSummaryProfile : Profile
    {
        public CompaniesHouseSummaryProfile()
        {
            CreateMap<InternalApi.Types.CompaniesHouse.Company, CompaniesHouseSummary>()
                .ForMember(dest => dest.CompanyNumber, opt => opt.MapFrom(source => source.CompanyNumber))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(source => source.Status))
                .ForMember(dest => dest.CompanyType, opt => opt.MapFrom(source => source.Type))
                .ForMember(dest => dest.IncorporationDate, opt => opt.MapFrom(source => source.IncorporatedOn))
                .ForMember(dest => dest.Directors,
                    opt => opt.MapFrom(source => source.Officers.Where(x => x.Role.ToLower() == "director")))
                .ForMember(dest => dest.PersonsWithSignificantControl,
                    opt => opt.MapFrom(source => source.PeopleWithSignificantControl))
                .ForMember(dest => dest.CompanyTypeDescriptions, opt => opt.Ignore())
                .ForMember(dest => dest.ManualEntryRequired, opt => opt.Ignore());
        }
    }

    public class DirectorInformationProfile : Profile
    {
        public DirectorInformationProfile()
        {
            CreateMap<InternalApi.Types.CompaniesHouse.Officer, DirectorInformation>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(source => source.DateOfBirth))
                .ForMember(dest => dest.AppointedDate, opt => opt.MapFrom(source => source.AppointedOn))
                .ForMember(dest => dest.ResignedDate, opt => opt.MapFrom(source => source.ResignedOn));
        }
    }

    public class PersonSignificantControlInformationProfile : Profile
    {
        public PersonSignificantControlInformationProfile()
        {
            CreateMap<InternalApi.Types.CompaniesHouse.PersonWithSignificantControl, PersonSignificantControlInformation>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(source => source.DateOfBirth))
                .ForMember(dest => dest.NotifiedDate, opt => opt.MapFrom(source => source.NotifiedOn))
                .ForMember(dest => dest.CeasedDate, opt => opt.MapFrom(source => source.CeasedOn));
        }
    }
}
