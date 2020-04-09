namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using SFA.DAS.ApplyService.Configuration;
    using SFA.DAS.ApplyService.Domain.Ukrlp;
    using SFA.DAS.ApplyService.Infrastructure.ApiClients;

    public class UkrlpApiClient : ApiClientBase<UkrlpApiClient>, IUkrlpApiClient
    {
        public UkrlpApiClient(ILogger<UkrlpApiClient> logger, IConfigurationService configurationService, ITokenService tokenService) : base(logger)
        {
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(configurationService.GetConfig().Result.InternalApi.Uri);
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.GetToken());
        }

        public async Task<UkrlpLookupResults> GetTrainingProviderByUkprn(long ukprn)
        {
            return await Get<UkrlpLookupResults>($"/ukrlp-lookup?ukprn={ukprn}");
        }
    }
}