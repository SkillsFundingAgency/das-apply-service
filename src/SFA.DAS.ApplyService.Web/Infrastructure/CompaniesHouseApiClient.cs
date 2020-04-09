using AutoMapper;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using SFA.DAS.ApplyService.Configuration;
    using SFA.DAS.ApplyService.Domain.CompaniesHouse;
    using SFA.DAS.ApplyService.InternalApi.Types.CompaniesHouse;
    using SFA.DAS.ApplyService.Infrastructure.ApiClients;

    public class CompaniesHouseApiClient : ApiClientBase<CompaniesHouseApiClient>, ICompaniesHouseApiClient
    {
        public CompaniesHouseApiClient(ILogger<CompaniesHouseApiClient> logger, IConfigurationService configurationService, ITokenService tokenService) : base(logger)
        {
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(configurationService.GetConfig().Result.InternalApi.Uri);
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.GetToken());
        }

        public async Task<CompaniesHouseSummary> GetCompanyDetails(string companiesHouseNumber)
        {
            var requestMessage = await GetResponse($"companies-house-lookup?companyNumber={companiesHouseNumber}");

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
