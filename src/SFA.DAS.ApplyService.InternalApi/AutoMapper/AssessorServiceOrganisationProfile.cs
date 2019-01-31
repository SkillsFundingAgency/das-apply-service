using AutoMapper;

namespace SFA.DAS.ApplyService.InternalApi.AutoMapper
{
    public class AssessorServiceOrganisationProfile : Profile
    {
        public AssessorServiceOrganisationProfile()
        {
            CreateMap<Models.AssessorService.OrganisationSummary, Types.OrganisationSearchResult>()
                .BeforeMap((source, dest) => dest.OrganisationReferenceType = "RoEPAO")
                .BeforeMap((source, dest) => dest.RoEPAOApproved = true)
                .BeforeMap((source, dest) => dest.EasApiOrganisationType = null)
                .ForMember(dest => dest.Ukprn, opt => opt.MapFrom(source => source.Ukprn))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.TradingName, opt => opt.MapFrom(source => source.Name))
                .ForMember(dest => dest.LegalName, opt => opt.ResolveUsing(source => source.OrganisationData?.LegalName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(source => source.Email))
                .ForMember(dest => dest.OrganisationType, opt => opt.MapFrom(source => source.OrganisationType))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(source => Mapper.Map<Models.AssessorService.OrganisationData, Types.OrganisationAddress>(source.OrganisationData)))
                .ForMember(dest => dest.CompanyNumber, opt => opt.ResolveUsing(source => source.OrganisationData?.CompanyNumber))
                .ForMember(dest => dest.CharityNumber, opt => opt.ResolveUsing(source => source.OrganisationData?.CharityNumber))
                .ForMember(dest => dest.FinancialDueDate, opt => opt.ResolveUsing(source => source.OrganisationData?.FinancialDueDate))
                .ForMember(dest => dest.FinancialExempt, opt => opt.ResolveUsing(source => source.OrganisationData?.FinancialExempt))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }

    public class AssessorServiceOrganisationAddressProfile : Profile
    {
        public AssessorServiceOrganisationAddressProfile()
        {
            CreateMap<Models.AssessorService.OrganisationData, Types.OrganisationAddress>()
                .ForMember(dest => dest.Address1, opt => opt.MapFrom(source => source.Address1))
                .ForMember(dest => dest.Address2, opt => opt.MapFrom(source => source.Address2))
                .ForMember(dest => dest.Address3, opt => opt.MapFrom(source => source.Address3))
                .ForMember(dest => dest.City, opt => opt.MapFrom(source => source.Address4))
                .ForMember(dest => dest.Postcode, opt => opt.MapFrom(source => source.Postcode))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }

    public class AssessorServiceOrganisationTypeProfile : Profile
    {
        public AssessorServiceOrganisationTypeProfile()
        {
            CreateMap<Models.AssessorService.OrganisationType, Types.OrganisationType>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(source => source.Type))
                .ForMember(dest => dest.TypeDescription, opt => opt.MapFrom(source => source.TypeDescription))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }
}
