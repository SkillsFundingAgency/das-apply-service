using Newtonsoft.Json;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Types;
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

        public async Task<Organisation> Create(OrganisationSearchResult organisation1, string createdBy)
        {
            // TODO: Consider request object. This is what the Internal Controller posts
            //new CreateOrganisationRequest { Name = request.Name, OrganisationType = request.OrganisationType, OrganisationUkprn = request.OrganisationUkprn, OrganisationDetails = request.OrganisationDetails, CreatedBy = request.CreatedBy, PrimaryContactEmail = request.Email };

            return await (await _httpClient.PostAsJsonAsync($"/Organisations", string.Empty)).Content
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
