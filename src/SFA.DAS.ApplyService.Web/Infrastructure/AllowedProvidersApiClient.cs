using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Infrastructure.ApiClients;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class AllowedProvidersApiClient : ApiClientBase<AllowedProvidersApiClient>, IAllowedProvidersApiClient
    {
        public AllowedProvidersApiClient(HttpClient httpClient, ILogger<AllowedProvidersApiClient> logger, ITokenService tokenService) : base(httpClient, logger)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.GetToken());
        }

        public async Task<bool> IsUkprnOnAllowedList(int ukprn)
        {
            return await Get<bool>($"/AllowedProviders/ukprn/{ukprn}");
        }
    }
}
