using AutoMapper;

namespace SFA.DAS.ApplyService.Web.AutoMapper
{
    using Domain.Roatp;
    using InternalApi.Types;

    public class RoatpCreateOrganisationRequestProfile : Profile
    {
        public RoatpCreateOrganisationRequestProfile()
        {
            CreateMap<ApplicationDetails, CreateOrganisationRequest>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.UkrlpLookupDetails.ProviderName))
                .ForMember(dest => dest.OrganisationType,
                    opt => opt.MapFrom(source => ApplicationDetails.OrganisationType))
                .ForMember(dest => dest.OrganisationUkprn,
                    opt => opt.MapFrom(source => source.UkrlpLookupDetails.UKPRN))
                .ForMember(dest => dest.PrimaryContactEmail,
                    opt => opt.MapFrom(source => source.UkrlpLookupDetails.PrimaryContactDetails.ContactEmail))
                //.ForMember(dest => dest.OrganisationDetails, opt => opt.MapFrom<RoatpOrganisationDetailsCustomResolver>()))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(source => source.CreatedBy));
        }
    }

}
