using AutoMapper;
using CharityCommissionService;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Configuration;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    using System;

    /// <summary>
    /// Charity Commission API docs are located at: http://apps.charitycommission.gov.uk/Showcharity/API/SearchCharitiesV1/Docs/DevGuideHome.aspx
    /// Charity Commission WSDL is located at: https://apps.charitycommission.gov.uk/Showcharity/API/SearchCharitiesV1/SearchCharitiesV1.asmx?WSDL
    /// There is a Web-Friendly version located at: http://beta.charitycommission.gov.uk/charity-search/
    /// </summary>
    public class CharityCommissionApiClient
    {
        private readonly ISearchCharitiesV1SoapClient _client;
        private readonly ILogger<CharityCommissionApiClient> _logger;
        private readonly IApplyConfig _config;

        public CharityCommissionApiClient()
        {
            // Constructor used for Mocking CharityCommissionApiClient
        }


        public CharityCommissionApiClient(ISearchCharitiesV1SoapClient client, ILogger<CharityCommissionApiClient> logger, IConfigurationService configurationService)
        {
            _client = client;
            _logger = logger;
            _config = configurationService.GetConfig().Result;
        }

        public async virtual Task<Types.CharityCommission.Charity> GetCharity(int charityNumber)
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

        private async Task<Types.CharityCommission.Charity> GetCharityDetails(int charityNumber)
        {
            _logger.LogInformation($"Searching Charity Commission - Charity Details. Charity Number: {charityNumber}");
            var request = new GetCharityByRegisteredCharityNumberRequest(_config.CharityCommissionApiAuthentication.ApiKey, charityNumber);
            var apiResponse = await _client.GetCharityByRegisteredCharityNumberAsync(request);
            return Mapper.Map<Charity, Types.CharityCommission.Charity>(apiResponse.GetCharityByRegisteredCharityNumberResult);
        }
    }
}
