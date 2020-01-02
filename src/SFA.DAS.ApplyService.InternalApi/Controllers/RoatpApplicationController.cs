namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::AutoMapper;
    using Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using SFA.DAS.ApplyService.Domain.Roatp;
    using Polly;
    using Polly.Retry;
    using Microsoft.AspNetCore.Authorization;
    using MediatR;
    using SFA.DAS.ApplyService.Application.Apply.Roatp;
    using SFA.DAS.ApplyService.Domain.Entities;

    [Authorize]
    public class RoatpApplicationController : Controller
    {
        private ILogger<RoatpApplicationController> _logger;

        private RoatpApiClient _apiClient;
        
        private AsyncRetryPolicy _retryPolicy;

        private readonly IMediator _mediator;

        public RoatpApplicationController(ILogger<RoatpApplicationController> logger, RoatpApiClient apiClient, IMediator mediator)
        {
            _logger = logger;
            _apiClient = apiClient;
            _retryPolicy = GetRetryPolicy();
            _mediator = mediator;
        }

        [Route("all-roatp-routes")]
        public async Task<IActionResult> GetApplicationRoutes()
        {
            var providerTypes = await _retryPolicy.ExecuteAsync(context => _apiClient.GetProviderTypes(), new Context());
            
            var applicationRoutes = Mapper.Map<IEnumerable<ApplicationRoute>>(providerTypes);

            return Ok(applicationRoutes);
        }

        [Route("ukprn-on-register")]
        public async Task<IActionResult> UkprnOnRegister(long ukprn)
        {
            var registerStatus = await _retryPolicy.ExecuteAsync(
                context => _apiClient.GetOrganisationRegisterStatus(ukprn.ToString()),
                new Context());
     
            return Ok(registerStatus);
        
        }

        [HttpPost]
        [Route("submit")]
        public async Task<IActionResult> SubmitApplication([FromBody] RoatpApplicationData applicationData)
        {
            // TODO: Remove this end point and mediator handers. Use ApplicationController.SubmitApplication
            var submitResult = await _mediator.Send(new SubmitApplicationRequest(applicationData));

            return Ok(submitResult);
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
