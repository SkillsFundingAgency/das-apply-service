using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.InternalApi.Types;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class OrganisationSearchApiClient
    {
        private readonly ILogger<OrganisationSearchApiClient> _logger;
        private static readonly HttpClient _httpClient = new HttpClient();

        public OrganisationSearchApiClient(IConfigurationService configurationService, ILogger<OrganisationSearchApiClient> logger)
        {
            _logger = logger;
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(configurationService.GetConfig().Result.InternalApi.Uri);
            }
        }

        public async Task<IEnumerable<OrganisationSearchResult>> SearchOrganisation(string searchTerm)
        {
            return await (await _httpClient.GetAsync($"/OrganisationSearch?searchTerm={searchTerm}")).Content.ReadAsAsync<IEnumerable<OrganisationSearchResult>>();
        }

        public async Task<OrganisationSearchResult> GetOrganisationByEmail(string email)
        {
            var httpResponseMessage = await _httpClient.GetAsync($"/OrganisationSearch/email/{email}");

            var responseAsString = await httpResponseMessage.Content.ReadAsStringAsync();
            
            _logger.LogInformation($"Content received from OrganisationSearch/email: {responseAsString}");

            return JsonConvert.DeserializeObject<OrganisationSearchResult>(responseAsString);
            
//            return await httpResponseMessage.Content
//                .ReadAsAsync<OrganisationSearchResult>();
        }

        public async Task<IEnumerable<OrganisationType>> GetOrganisationTypes()
        {
            return await (await _httpClient.GetAsync($"/OrganisationTypes")).Content.ReadAsAsync<IEnumerable<OrganisationType>>();
        }
    }
}
