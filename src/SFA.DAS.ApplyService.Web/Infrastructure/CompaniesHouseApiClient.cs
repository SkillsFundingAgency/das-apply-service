using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.InternalApi.Types.CompaniesHouse;
using AutoMapper;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using SFA.DAS.ApplyService.Configuration;
    using Microsoft.AspNetCore.Http;
    using SFA.DAS.ApplyService.Domain.CompaniesHouse;

    public class CompaniesHouseApiClient : ICompaniesHouseApiClient
    {
        private ILogger<CompaniesHouseApiClient> _logger;
        private readonly ITokenService _tokenService;
        private static readonly HttpClient _httpClient = new HttpClient();

        public CompaniesHouseApiClient(IConfigurationService configurationService,
            ILogger<CompaniesHouseApiClient> logger, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _logger = logger;
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(configurationService.GetConfig().Result.InternalApi.Uri);
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenService.GetToken());
        }

        public async Task<CompaniesHouseSummary> GetCompanyDetails(string companiesHouseNumber)
        {
            var requestMessage =
                await _httpClient.GetAsync($"companies-house-lookup?companyNumber={companiesHouseNumber}");

            if (requestMessage.IsSuccessStatusCode)
            {
                var companyDetails = await requestMessage.Content.ReadAsAsync<Company>();

                return Mapper.Map<CompaniesHouseSummary>(companyDetails);
            }

            if (requestMessage.StatusCode == HttpStatusCode.ServiceUnavailable || requestMessage.StatusCode == HttpStatusCode.InternalServerError)
            {
                return new CompaniesHouseSummary { Status = CompaniesHouseSummary.ServiceUnavailable };
            }

            return new CompaniesHouseSummary { Status = CompaniesHouseSummary.CompanyStatusNotFound };
        }
    }
}
