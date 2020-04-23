namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    using Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;
    using Polly;
    using Polly.Retry;
    using System;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;

    [Authorize]
    public class CharityCommissionController : Controller
    {
        private ILogger<CharityCommissionController> _logger;

        private CharityCommissionApiClient _apiClient;

        private AsyncRetryPolicy _retryPolicy;

        public CharityCommissionController(ILogger<CharityCommissionController> logger, CharityCommissionApiClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
            _retryPolicy = GetRetryPolicy();
        }

        [HttpGet]
        [Route("charity-commission-lookup")]
        public async Task<IActionResult> CharityDetails(int charityNumber)
        {
            try
            {
                var charityDetails =
                    await _retryPolicy.ExecuteAsync(context => _apiClient.GetCharity(charityNumber), new Context());

                if (charityDetails == null)
                {
                    return NotFound();
                }

                return Ok(charityDetails);
            }
            catch (ServiceUnavailableException)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }
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
                    _logger.LogWarning($"Error retrieving response from Charity Commission API. Reason: {exception.Message}. Retrying in {timeSpan.Seconds} secs...attempt: {retryCount}");
                });
        }
    }
}
