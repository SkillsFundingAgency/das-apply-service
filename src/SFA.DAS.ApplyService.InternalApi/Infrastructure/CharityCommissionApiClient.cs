using AutoMapper;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Infrastructure.ApiClients;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    /// <summary>
    /// APIM portal is located at: https://api-portal.charitycommission.gov.uk/
    /// Charity Commission API docs are located at: https://api-portal.charitycommission.gov.uk/api-details#api=register-of-charities-api
    /// There is a Web-Friendly version located at: https://register-of-charities.charitycommission.gov.uk/charity-search/
    /// </summary>
    public class CharityCommissionApiClient : ApiClientBase<CharityCommissionApiClient>, ICharityCommissionApiClient
    {
        private const string _apimSubscriptionKeyName = "Ocp-Apim-Subscription-Key";

        public CharityCommissionApiClient(HttpClient httpClient, ILogger<CharityCommissionApiClient> logger, IConfigurationService configurationService) : base(httpClient, logger)
        {
            var config = configurationService.GetConfig().GetAwaiter().GetResult();
            var subscriptionKey = config.CharityCommissionApiAuthentication.ApiKey;
            
            if (!_httpClient.DefaultRequestHeaders.Contains(_apimSubscriptionKeyName))
            {
                _httpClient.DefaultRequestHeaders.Add(_apimSubscriptionKeyName, subscriptionKey);
            }
        }

        public async Task<Types.CharityCommission.Charity> GetCharity(int charityNumber)
        {
            try
            {
                _logger.LogInformation($"Searching Charity Commission - Charity Details. Charity Number: {charityNumber}");

                const int mainCharitySuffix = 0;
                var apiResponse = await Get<CharityCommission.Entities.Charity>($"allcharitydetails/{charityNumber}/{mainCharitySuffix}");

                return Mapper.Map<CharityCommission.Entities.Charity, Types.CharityCommission.Charity>(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to retrieve details from Charity Commission API", ex);
                throw new ServiceUnavailableException("Unable to retrieve details from Charity Commission API");
            }
        }
    }

    public interface ICharityCommissionApiClient
    {
        Task<Types.CharityCommission.Charity> GetCharity(int charityNumber);
    }
}
