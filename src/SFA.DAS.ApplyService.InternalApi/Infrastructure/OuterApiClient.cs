using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Configuration;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    using Newtonsoft.Json;
    using SFA.DAS.ApplyService.Infrastructure.ApiClients;
    using SFA.DAS.ApplyService.InternalApi.Types.CharityCommission;
    using System;
    using System.Net.Http;

    /// <summary>
    /// Charity Commission API docs are located at: http://apps.charitycommission.gov.uk/Showcharity/API/SearchCharitiesV1/Docs/DevGuideHome.aspx
    /// Charity Commission WSDL is located at: https://apps.charitycommission.gov.uk/Showcharity/API/SearchCharitiesV1/SearchCharitiesV1.asmx?WSDL
    /// There is a Web-Friendly version located at: http://beta.charitycommission.gov.uk/charity-search/
    /// </summary>
    public class OuterApiClient : ApiClientBase<OuterApiClient>, IOuterApiClient
    {
        public OuterApiClient(HttpClient httpClient, ILogger<OuterApiClient> logger, IConfigurationService configurationService) : base(httpClient, logger)
        {
            var config = configurationService.GetConfig().Result;

            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.BaseAddress = new Uri(config.OuterApiConfiguration.ApiBaseUrl);
        }

        public async virtual Task<Charity> GetCharity(int charityNumber)
        {
            try
            {
                return await GetCharityDetails(charityNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to retrieve details from Charity Commission API", ex);
                throw new ServiceUnavailableException("Unable to retrieve details from Charity Commission API");
            }
        }

        private async Task<Charity> GetCharityDetails(int charityNumber)
        {
            _logger.LogInformation($"Searching Charity Commission - Charity Details. Charity Number: {charityNumber}");
            var apiResponse = await _httpClient.GetAsync($"charities/{charityNumber}");
            string json = await apiResponse.Content.ReadAsStringAsync();
            var charityDetails = JsonConvert.DeserializeObject<Charity>(json);
            return charityDetails;
        }
    }
}
