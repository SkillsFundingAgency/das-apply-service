using AutoMapper;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Infrastructure.ApiClients;
using SFA.DAS.ApplyService.InternalApi.Models.ProviderRegister;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    public class ProviderRegisterApiClient : ApiClientBase<ProviderRegisterApiClient>
    {
        private readonly IApplyConfig _config;

        public ProviderRegisterApiClient(ILogger<ProviderRegisterApiClient> logger, IConfigurationService configurationService) : base(logger)
        {
            _config = configurationService.GetConfig().Result;

            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(_config.ProviderRegisterApiAuthentication.ApiBaseAddress);
            }
        }

        public async Task<IEnumerable<Types.OrganisationSearchResult>> SearchOrgansiationByName(string name, bool exactMatch)
        {
            _logger.LogInformation($"Searching Provider Register. Name: {name}");
            var apiResponse = await Get<List<Provider>>($"/providers/search?keywords={name}");

            if (exactMatch)
            {
                apiResponse = apiResponse?.Where(r => r.ProviderName.Equals(name, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            return Mapper.Map<IEnumerable<Provider>, IEnumerable<Types.OrganisationSearchResult>>(apiResponse);
        }

        public async Task<Types.OrganisationSearchResult> SearchOrgansiationByUkprn(int ukprn)
        {
            _logger.LogInformation($"Searching Provider Register. Ukprn: {ukprn}");
            var apiResponse = await Get<Provider>($"/providers/{ukprn}");

            return Mapper.Map<Provider, Types.OrganisationSearchResult>(apiResponse);
        }
    }
}
