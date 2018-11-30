using AutoMapper;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.InternalApi.Models.ProviderRegister;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    public class ProviderRegisterApiClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<ProviderRegisterApiClient> _logger;

        public ProviderRegisterApiClient(HttpClient client, ILogger<ProviderRegisterApiClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<IEnumerable<Types.OrganisationSearchResult>> SearchOrgansiationByName(string name)
        {
            _logger.LogInformation($"Searching Provider Register. Name: {name}");
            var apiResponse = await Get<IEnumerable<Provider>>($"/providers/search?keywords={name}");

            return Mapper.Map<IEnumerable<Provider>, IEnumerable<Types.OrganisationSearchResult>>(apiResponse);
        }

        public async Task<Types.OrganisationSearchResult> SearchOrgansiationByUkprn(int ukprn)
        {
            _logger.LogInformation($"Searching Provider Register. Ukprn: {ukprn}");
            var apiResponse = await Get<Provider>($"/providers/{ukprn}");

            return Mapper.Map<Provider, Types.OrganisationSearchResult>(apiResponse);
        }

        public async Task<Types.OrganisationSearchResult> SearchOrgansiationByEpao(string epao)
        {
            _logger.LogInformation($"Searching Provider Register. EPAO: {epao}");
            var apiResponse = await Get<Organisation>($"/assessment-organisations/{epao}");

            return Mapper.Map<Organisation, Types.OrganisationSearchResult>(apiResponse);
        }

        private async Task<T> Get<T>(string uri)
        {
            using (var response = await _client.GetAsync(new Uri(uri, UriKind.Relative)))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<T>();
                }

                return default(T);
            }
        }

    }
}
