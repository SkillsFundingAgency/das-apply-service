using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.InternalApi.Types.CharityCommission;

namespace SFA.DAS.ApplyService.Infrastructure.ApiClients
{
    public class OuterApiClient : IOuterApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OuterApiClient> _logger;

        public OuterApiClient(HttpClient httpClient, ILogger<OuterApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<Charity> GetCharityDetails(int charityNumber)
        {
            var apiResponse = await _httpClient.GetAsync($"charities/{charityNumber}");

            apiResponse.EnsureSuccessStatusCode();

            return await apiResponse.Content.ReadAsAsync<Charity>();
        }
    }
}