namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Domain.Roatp;
    using Microsoft.Extensions.Logging;
    using SFA.DAS.ApplyService.Configuration;
    using SFA.DAS.ApplyService.Domain.Entities;

    public class RoatpApiClient : IRoatpApiClient
    {
        private readonly ILogger<RoatpApiClient> _logger;
        private readonly ITokenService _tokenService;
        private static readonly HttpClient _httpClient = new HttpClient();

        public RoatpApiClient(IConfigurationService configurationService, ILogger<RoatpApiClient> logger, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _logger = logger;
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(configurationService.GetConfig().Result.InternalApi.Uri);
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenService.GetToken());
        }

        public async Task<IEnumerable<ApplicationRoute>> GetApplicationRoutes()
        {
            return await (await _httpClient.GetAsync($"/all-roatp-routes")).Content
                .ReadAsAsync<IEnumerable<ApplicationRoute>>();
        }

        public async Task<OrganisationRegisterStatus> GetOrganisationRegisterStatus(long ukprn)
        {
            return await (await _httpClient.GetAsync($"/ukprn-on-register?ukprn={ukprn}")).Content
                .ReadAsAsync<OrganisationRegisterStatus>();
        }

        public async Task<string> GetNextRoatpApplicationReference()
        {
            // TODO: Remove this API call. Should be internal to Create Application
            var nextReferenceInSequence = await (await _httpClient.GetAsync("/next-application-reference")).Content.ReadAsAsync<NextApplicationReference>();
            return nextReferenceInSequence.ApplicationReference;
        }

        public async Task<bool> SubmitRoatpApplication(RoatpApplicationData applicationData)
        {
            // TODO: Remove this API call. Should be using the one in IApplicationApiClient
            return await (await _httpClient.PostAsJsonAsync($"/submit", applicationData)).Content.ReadAsAsync<bool>();
        }

        public async Task<RoatpApplicationData> GetApplicationData(Guid applicationId)
        {
            // TODO: Remove this API call. Should be using the one in IApplicationApiClient
            return await (await _httpClient.GetAsync($"/application-data?applicationId={applicationId}")).Content
                .ReadAsAsync<RoatpApplicationData>();
        }
    }

    public class NextApplicationReference
    {
        // TODO: Remove this class
        public string ApplicationReference { get; set; }
    }
}