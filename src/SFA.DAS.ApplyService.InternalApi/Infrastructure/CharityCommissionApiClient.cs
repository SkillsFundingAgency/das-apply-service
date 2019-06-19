using AutoMapper;
using CharityCommissionService;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Configuration;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
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

        public CharityCommissionApiClient(ISearchCharitiesV1SoapClient client, ILogger<CharityCommissionApiClient> logger, IConfigurationService configurationService)
        {
            _client = client;
            _logger = logger;
            _config = configurationService.GetConfig().Result;
        }

        public async Task<Types.CharityCommission.Charity> GetCharity(int charityNumber)
        {
            return await GetCharityDetails(charityNumber); 
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
