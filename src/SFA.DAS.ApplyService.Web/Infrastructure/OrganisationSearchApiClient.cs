using SFA.DAS.ApplyService.Web.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;


namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class OrganisationSearchApiClient
    {
        private readonly HttpClient _httpClient;

        public OrganisationSearchApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Organisation>> SearchOrganisation(string searchTerm)
        {
            return await (await _httpClient.GetAsync($"/OrganisationSearch?searchTerm={searchTerm}")).Content.ReadAsAsync<IEnumerable<Organisation>>();
        }

        public async Task<Organisation> GetOrganisationByEmail(string emailAddress)
        {
            return await (await _httpClient.GetAsync($"/OrganisationByEmail?emailAddress={emailAddress}")).Content.ReadAsAsync<Organisation>();
        }

        public async Task<IEnumerable<string>> GetOrganisationTypes()
        {
            return await (await _httpClient.GetAsync($"/OrganisationTypes")).Content.ReadAsAsync<IEnumerable<string>>();
        }
    }
}
