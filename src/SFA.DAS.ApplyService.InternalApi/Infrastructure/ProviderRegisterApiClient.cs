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

        public async Task<IEnumerable<Types.Organisation>> SearchOrgansiation(string searchTerm)
        {
            List<Types.Organisation> organisations = new List<Types.Organisation>();

            if (searchTerm.StartsWith("EPA", StringComparison.InvariantCultureIgnoreCase))
            {
                var organisation = await SearchOrgansiationByEpao(searchTerm);
                if(organisation != null) organisations.Add(organisation);
            }
            else if (int.TryParse(searchTerm, out var ukprn))
            {
                var organisation = await SearchOrgansiationByUkprn(ukprn);
                if (organisation != null) organisations.Add(organisation);
            }
            else
            {
                var orgs = await SearchOrgansiationByKeyword(searchTerm);
                if (orgs != null) organisations.AddRange(orgs);
            }

            return organisations;
        }

        private async Task<IEnumerable<Types.Organisation>> SearchOrgansiationByKeyword(string keyword)
        {
            _logger.LogInformation($"Searching Provider Register. Keyword: {keyword}");
            var apiResponse = await Get<IEnumerable<Provider>>($"/providers/search?keywords={keyword}");

            return Mapper.Map<IEnumerable<Provider>, IEnumerable<Types.Organisation>>(apiResponse);
        }

        private async Task<Types.Organisation> SearchOrgansiationByUkprn(int ukprn)
        {
            _logger.LogInformation($"Searching Provider Register. Ukprn: {ukprn}");
            var apiResponse = await Get<Provider>($"/providers/{ukprn}");

            return Mapper.Map<Provider, Types.Organisation>(apiResponse);
        }

        private async Task<Types.Organisation> SearchOrgansiationByEpao(string epao)
        {
            _logger.LogInformation($"Searching Provider Register. EPAO: {epao}");
            var apiResponse = await Get<Organisation>($"/assessment-organisations/{epao}");

            return Mapper.Map<Organisation, Types.Organisation>(apiResponse);
        }

        private async Task<T> Get<T>(string uri)
        {
            using (var response = await _client.GetAsync(new Uri(uri, UriKind.Relative)))
            {
                return await response.Content.ReadAsAsync<T>();
            }
        }

    }
}
