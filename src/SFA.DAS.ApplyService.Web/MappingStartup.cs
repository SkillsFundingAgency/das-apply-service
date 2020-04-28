using AutoMapper;
using SFA.DAS.ApplyService.Web.AutoMapper;

namespace SFA.DAS.ApplyService.Web
{
    public static class MappingStartup
    {
        public static void AddMappings()
        {
            Mapper.Reset();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<CompaniesHouseSummaryProfile>();
                cfg.AddProfile<DirectorInformationProfile>();
                cfg.AddProfile<PersonSignificantControlInformationProfile>();
                cfg.AddProfile<CharityCommissionProfile>();
                cfg.AddProfile<CharityTrusteeProfile>();
                cfg.AddProfile<RoatpCreateOrganisationRequestProfile>();
                cfg.AddProfile<RoatpContactAddressProfile>();
                cfg.AddProfile<NotRequiredOverridesProfile>();
                cfg.AddProfile<NotRequiredConditionsProfile>();
            });

            Mapper.AssertConfigurationIsValid();
        }
    }
}
