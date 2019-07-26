namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Polly;
    using Polly.Retry;
    using SFA.DAS.ApplyService.InternalApi.Models.Ukrlp;

    [Authorize]
    public class UkrlpLookupController : Controller
    {
        private ILogger<UkrlpLookupController> _logger;

        private IRoatpApiClient _apiClient;

        private AsyncRetryPolicy _retryPolicy;

        public UkrlpLookupController(ILogger<UkrlpLookupController> logger, IRoatpApiClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
            _retryPolicy = GetRetryPolicy();
        }

        [Route("ukrlp-lookup")]
        public async Task<IActionResult> UkrlpLookup(string ukprn)
        {
            UkprnLookupResponse providerData;

            try
            {
                providerData = await _retryPolicy.ExecuteAsync(context => _apiClient.GetUkrlpDetails(ukprn), new Context());
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to retrieve results from UKRLP", ex);
                providerData = new UkprnLookupResponse
                {
                    Success = false,
                    Results = new List<ProviderDetails>()
                };
            }
            return Ok(providerData);
        }

        private AsyncRetryPolicy GetRetryPolicy()
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(4)
                }, (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning($"Error retrieving response from UKRLP. Reason: {exception.Message}. Retrying in {timeSpan.Seconds} secs...attempt: {retryCount}");
                });
        }
    }
}
