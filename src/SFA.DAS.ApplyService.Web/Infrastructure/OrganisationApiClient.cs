using Newtonsoft.Json;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using OrganisationDetails = SFA.DAS.ApplyService.InternalApi.Types.OrganisationDetails;

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

        public async Task<Organisation> Create(OrganisationSearchResult organisation, string createdBy)
        {
            var orgDetails = new OrganisationDetails
            {
                OrganisationReferenceType = organisation.OrganisationReferenceType,
                OrganisationReferenceId = organisation.OrganisationReferenceId,
                Address1 = organisation.Address?.Address1,
                Address2 = organisation.Address?.Address2,
                Address3 = organisation.Address?.Address3,
                City = organisation.Address?.City,
                Postcode = organisation.Address?.Postcode
            };

            var request = new CreateOrganisationRequest { Name = organisation.Name, OrganisationType = organisation.OrganisationType, OrganisationUkprn = organisation.Ukprn, OrganisationDetails = orgDetails, CreatedBy = createdBy, PrimaryContactEmail = organisation.Email };

            return await (await _httpClient.PostAsJsonAsync($"/Organisations", request)).Content
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
