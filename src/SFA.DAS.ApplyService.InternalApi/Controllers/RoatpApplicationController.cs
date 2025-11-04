using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using global::AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Services;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class RoatpApplicationController : Controller
    {
        private readonly ILogger<RoatpApplicationController> _logger;
        private readonly IRoatpApiClient _apiClient;
        private readonly AsyncRetryPolicy _retryPolicy;
        private readonly List<RoatpSequences> _roatpSequences;
        private readonly IRoatpService _roatpService;

        public RoatpApplicationController(ILogger<RoatpApplicationController> logger, IRoatpApiClient apiClient, IOptions<List<RoatpSequences>> roatpSequences, IRoatpService roatpService)
        {
            _logger = logger;
            _apiClient = apiClient;
            _roatpService = roatpService;
            _retryPolicy = GetRetryPolicy();
            _roatpSequences = roatpSequences.Value;
        }

        [Route("all-roatp-routes")]
        [HttpGet]
        public async Task<IActionResult> GetApplicationRoutes()
        {
            var providerTypes = await _retryPolicy.ExecuteAsync(context => _apiClient.GetProviderTypes(), new Context());

            var applicationRoutes = Mapper.Map<IEnumerable<ApplicationRoute>>(providerTypes);

            return Ok(applicationRoutes);
        }

        [Route("ukprn-on-register")]
        [HttpGet]
        public async Task<IActionResult> UkprnOnRegister(int ukprn)
        {
            var registerStatus = await _retryPolicy.ExecuteAsync(
                context => _roatpService.GetRegisterStatus(ukprn),
                new Context());

            return Ok(registerStatus);

        }

        [Route("roatp-sequences")]
        [HttpGet]
        public IActionResult RoatpSequences()
        {
            return Ok(_roatpSequences);
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
                    _logger.LogWarning($"Error retrieving response from RoATP API. Reason: {exception.Message}. Retrying in {timeSpan.Seconds} secs...attempt: {retryCount}");
                });
        }
    }
}
