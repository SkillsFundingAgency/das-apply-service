namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using SFA.DAS.ApplyService.InternalApi.Infrastructure;
    using System.Threading.Tasks;
    using Polly;
    using Polly.Retry;
    using System;
    using System.Net;
    using Microsoft.AspNetCore.Http;

    public class CompaniesHouseController : Controller
    {
        private ILogger<CompaniesHouseController> _logger;

        private CompaniesHouseApiClient _apiClient;

        private AsyncRetryPolicy _retryPolicy;

        public CompaniesHouseController(ILogger<CompaniesHouseController> logger, CompaniesHouseApiClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
            _retryPolicy = GetRetryPolicy();
        }

        [Route("companies-house-lookup")]
        public async Task<IActionResult> CompanyDetails(string companyNumber)
        {
            var companyDetails = await _retryPolicy.ExecuteAsync(context => _apiClient.GetCompany(companyNumber), new Context());
            
            if (!companyDetails.Success)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            if (companyDetails.Response == null)
            {
                return NotFound();
            }

            return Ok(companyDetails.Response);
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
                },
                (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning(
                        $"Error retrieving response from Companies House API. Reason: {exception.Message}. Retrying in {timeSpan.Seconds} secs...attempt: {retryCount}");
                });
        }
    }
}
