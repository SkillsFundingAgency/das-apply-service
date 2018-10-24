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
                cfg.AddProfile<ProviderRegisterOrganisationProfile>();
                cfg.AddProfile<ReferenceDataOrganisationProfile>();
            });

            Mapper.AssertConfigurationIsValid();
        }
    }
}
