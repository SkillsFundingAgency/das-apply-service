using AutoMapper;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Infrastructure.ApiClients;
using SFA.DAS.ApplyService.InternalApi.Models.ProviderRegister;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    public class ProviderRegisterApiClient : ApiClientBase<ProviderRegisterApiClient>
    {
        public ProviderRegisterApiClient(HttpClient httpClient, ILogger<ProviderRegisterApiClient> logger) : base(httpClient, logger)
        {
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
