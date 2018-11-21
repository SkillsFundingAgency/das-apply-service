using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.InternalApi.Types;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;


namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class OrganisationSearchApiClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public OrganisationSearchApiClient(IConfigurationService configurationService)
        {
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
            return await (await _httpClient.GetAsync($"/OrganisationSearch/email/{email}")).Content
                .ReadAsAsync<OrganisationSearchResult>();
        }

        public async Task<IEnumerable<OrganisationType>> GetOrganisationTypes()
        {
            return await (await _httpClient.GetAsync($"/OrganisationTypes")).Content.ReadAsAsync<IEnumerable<OrganisationType>>();
        }
    }
}
