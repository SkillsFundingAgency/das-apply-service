using AutoMapper;
using SFA.DAS.ApplyService.InternalApi.AutoMapper;

namespace SFA.DAS.ApplyService.InternalApi
{
    public static class MappingStartup
    {
        public static void AddMappings()
        {
            Mapper.Reset();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AssessorServiceOrganisationProfile>();
                cfg.AddProfile<AssessorServiceOrganisationAddressProfile>();
                cfg.AddProfile<AssessorServiceOrganisationTypeProfile>();

                cfg.AddProfile<ProviderRegisterOrganisationProfile>();
                cfg.AddProfile<ProviderRegisterOrganisationAddressProfile>();

                cfg.AddProfile<ReferenceDataOrganisationProfile>();
                cfg.AddProfile<ReferenceDataOrganisationAddressProfile>();

                cfg.AddProfile<CompaniesHouseCompanyProfile>();
                cfg.AddProfile<CompaniesHouseAccountsProfile>();
                cfg.AddProfile<CompaniesHouseRegisteredOfficeAddressProfile>();
                cfg.AddProfile<CompaniesHouseOfficerAddressProfile>();
                cfg.AddProfile<CompaniesHouseOfficerProfile>();
                cfg.AddProfile<CompaniesHouseOfficerDisqualificationProfile>();
                cfg.AddProfile<CompaniesHousePersonWithSignificantControlProfile>();
                cfg.AddProfile<CompaniesHousePersonWithSignificantControlAddressProfile>();

                cfg.AddProfile<CharityCommissionProfile>();
                cfg.AddProfile<CharityCommissionAddressProfile>();
                cfg.AddProfile<CharityCommissionAccountsProfile>();
                cfg.AddProfile<CharityCommissionTrusteeProfile>();

                cfg.AddProfile<UkrlpVerificationDetailsProfile>();
                cfg.AddProfile<UkrlpContactPersonalDetailsProfile>();
                cfg.AddProfile<UkrlpContactAddressProfile>();
                cfg.AddProfile<UkrlpProviderAliasProfile>();
                cfg.AddProfile<UkrlpProviderContactProfile>();
                cfg.AddProfile<UkrlpProviderDetailsProfile>();

                cfg.AddProfile<RoatpProfile>();
            });

            Mapper.AssertConfigurationIsValid();
        }
    }
}
