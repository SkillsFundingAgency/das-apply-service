﻿using AutoMapper;
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
                cfg.AddProfile<CompaniesHouseCompanyProfile>();
                cfg.AddProfile<CompaniesHouseAccountsProfile>();
                cfg.AddProfile<CompaniesHouseRegisteredOfficeAddressProfile>();
                cfg.AddProfile<CompaniesHouseOfficerAddressProfile>();
                cfg.AddProfile<CompaniesHouseOfficerProfile>();
                cfg.AddProfile<CompaniesHouseOfficerDisqualificationProfile>();
                cfg.AddProfile<CompaniesHousePersonWithSignificantControlProfile>();
                cfg.AddProfile<CompaniesHousePersonWithSignificantControlAddressProfile>();

                
                cfg.AddProfile<UkrlpVerificationDetailsProfile>();
                cfg.AddProfile<UkrlpContactPersonalDetailsProfile>();
                cfg.AddProfile<UkrlpContactAddressProfile>();
                cfg.AddProfile<UkrlpProviderAliasProfile>();
                cfg.AddProfile<UkrlpProviderContactProfile>();
                cfg.AddProfile<UkrlpProviderDetailsProfile>();

                cfg.AddProfile<RoatpProfile>();


                cfg.AddProfile<UkrlpCharityCommissionProfile>();
                cfg.AddProfile<UkrlpCompaniesHouseProfile>();
                cfg.AddProfile<UkrlpDirectorInformationProfile>();
                cfg.AddProfile<UkrlpPersonSignificantControlInformationProfile>();
            });

            Mapper.AssertConfigurationIsValid();
        }
    }
}
