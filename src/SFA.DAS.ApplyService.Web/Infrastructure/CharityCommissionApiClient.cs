
﻿namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    using Microsoft.Extensions.Logging;
    using SFA.DAS.ApplyService.Configuration;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using SFA.DAS.ApplyService.InternalApi.Types.CharityCommission;
    using SFA.DAS.ApplyService.InternalApi.Types;
    using System.Net.Http.Headers;

    public class CharityCommissionApiClient : ICharityCommissionApiClient
    {
        private ILogger<CharityCommissionApiClient> _logger;
        private readonly ITokenService _tokenService;
        private static readonly HttpClient _httpClient = new HttpClient();

        public CharityCommissionApiClient(IConfigurationService configurationService,
            ILogger<CharityCommissionApiClient> logger, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _logger = logger;
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(configurationService.GetConfig().Result.InternalApi.Uri);
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenService.GetToken());
        }

        public async Task<ApiResponse<Charity>> GetCharityDetails(int charityNumber)
        {
            var responseMessage = await _httpClient.GetAsync($"charity-commission-lookup?charityNumber={charityNumber}");

            if (responseMessage.StatusCode == HttpStatusCode.InternalServerError ||
                responseMessage.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                return new ApiResponse<Charity> { Success = false };
            }

            return new ApiResponse<Charity>
            {
                Success = true,
                Response = await responseMessage.Content.ReadAsAsync<Charity>()
            };
        }
    }
}
