using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.InternalApi.Models.AssessorService;
using SFA.DAS.ApplyService.InternalApi.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    public class AssessorServiceApiClient : ApiClientBase<AssessorServiceApiClient>
    {
        private readonly IApplyConfig _config;
        private readonly IHostingEnvironment _hostingEnvironment;

        public AssessorServiceApiClient(ILogger<AssessorServiceApiClient> logger, IConfigurationService configurationService, IHostingEnvironment hostingEnvironment) : base(logger)
        {
            _config = configurationService.GetConfig().Result;
            _hostingEnvironment = hostingEnvironment;

            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(_config.AssessorServiceApiAuthentication.ApiBaseAddress);
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
        }

        public async Task<IEnumerable<Types.OrganisationSearchResult>> SearchOrgansiation(string searchTerm)
        {
            _logger.LogInformation($"Searching EPAO Register. Search Term: {searchTerm}");
            var apiResponse =
                await Get<List<OrganisationSummary>>($"/api/ao/assessment-organisations/search/{searchTerm}");

            var organisationSearchResults =
                Mapper.Map<IEnumerable<OrganisationSummary>, IEnumerable<Types.OrganisationSearchResult>>(apiResponse);

            return organisationSearchResults;
        }

        public async Task<Types.OrganisationSearchResult> GetOrganisationByEmail(string emailAddress)
        {
            _logger.LogInformation($"Searching EPAO Register for. Email: {emailAddress}");
            var apiResponse = await Get<OrganisationSummary>($"/api/ao/assessment-organisations/email/{emailAddress}");

            return Mapper.Map<OrganisationSummary, Types.OrganisationSearchResult>(apiResponse);
        }

        public async Task<IEnumerable<Types.OrganisationType>> GetOrgansiationTypes(bool activeOnly = true)
        {
            _logger.LogInformation($"Getting Organisation Types from EPAO Register.");
            var apiResponse =
                await Get<List<Models.AssessorService.OrganisationType>>($"/api/ao/organisation-types");

            if (activeOnly)
            {
                apiResponse = apiResponse.Where(ot =>
                    "Live".Equals(ot.Status, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            return Mapper
                .Map<IEnumerable<Models.AssessorService.OrganisationType>, IEnumerable<Types.OrganisationType>>(
                    apiResponse);
        }

        public async Task<IEnumerable<DeliveryArea>> GetDeliveryAreas()
        {
            _logger.LogInformation($"Getting Delivery Areas from EPAO Register.");
            var apiResponse = await Get<List<DeliveryArea>>($"/api/ao/delivery-areas");

            return Mapper.Map<IEnumerable<DeliveryArea>, IEnumerable<DeliveryArea>>(apiResponse);
        }

        public async Task<IEnumerable<StandardCollation>> GetStandards()
        {
            _logger.LogInformation($"Gathering Standards from EPAO Register.");
            await Post($"/api/ao/update-standards", new { });
            var apiResponse =
                await Get<List<StandardCollation>>($"/api/ao/assessment-organisations/collated-standards");
            return apiResponse;
        }

        private string GetToken()
        {
            if (_hostingEnvironment.IsDevelopment())
                return string.Empty;

            var tenantId = _config.AssessorServiceApiAuthentication.TenantId;
            var clientId = _config.AssessorServiceApiAuthentication.ClientId;
            var clientSecret = _config.AssessorServiceApiAuthentication.ClientSecret;
            var resourceId = _config.AssessorServiceApiAuthentication.ResourceId;

            var authority = $"https://login.microsoftonline.com/{tenantId}";
            var clientCredential = new ClientCredential(clientId, clientSecret);
            var context = new AuthenticationContext(authority, true);
            var result = context.AcquireTokenAsync(resourceId, clientCredential).Result;

            return result.AccessToken;
        }
    }

}
