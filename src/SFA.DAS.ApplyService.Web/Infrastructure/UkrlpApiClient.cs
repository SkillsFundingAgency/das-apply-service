namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    using Microsoft.Extensions.Logging;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using SFA.DAS.ApplyService.Domain.Ukrlp;
    using SFA.DAS.ApplyService.Infrastructure.ApiClients;

    public class UkrlpApiClient : ApiClientBase<UkrlpApiClient>, IUkrlpApiClient
    {
        public UkrlpApiClient(HttpClient httpClient, ILogger<UkrlpApiClient> logger, ITokenService tokenService) : base(httpClient, logger)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.GetToken());
        }

        public async Task<UkrlpLookupResults> GetTrainingProviderByUkprn(int ukprn)
        {
            return await Get<UkrlpLookupResults>($"/ukrlp-lookup?ukprn={ukprn}");
        }
    }
}