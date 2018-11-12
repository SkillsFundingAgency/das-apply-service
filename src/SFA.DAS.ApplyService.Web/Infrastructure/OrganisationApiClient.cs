using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class OrganisationApiClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public OrganisationApiClient(IConfigurationService configurationService)
        {
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(configurationService.GetConfig().Result.InternalApi.Uri);
            }
        }

        public async Task<Organisation> GetByName(string name)
        {
            // TODO - is this even required?
            return await (await _httpClient.GetAsync($"/Organisations/name/{name}")).Content
                .ReadAsAsync<Organisation>();
        }

        public async Task<Organisation> GetByContactEmail(string email)
        {
            return await (await _httpClient.GetAsync($"/Organisations/email/{email}")).Content
                .ReadAsAsync<Organisation>();
        }

        public async Task<Organisation> Create(Organisation organisation)
        {
            // TODO: Consider request object
            return await (await _httpClient.PostAsJsonAsync($"/Organisations", organisation)).Content
                .ReadAsAsync<Organisation>();
        }

        public async Task<Organisation> Update(Organisation organisation)
        {
            // TODO: Consider request object
            // TODO - is this even required?
            return await (await _httpClient.PutAsJsonAsync($"/Organisations", organisation)).Content
                .ReadAsAsync<Organisation>();
        }
    }
}
