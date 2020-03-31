using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class GatewayRegisterChecksController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<GatewayRegisterChecksController> _logger;

        public GatewayRegisterChecksController(IMediator mediator, ILogger<GatewayRegisterChecksController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("/Gateway/{applicationId}/ProviderRouteName")]
        public async Task<string> GetProviderRouteName(Guid applicationId)
        {
            // NOTE: ProviderRouteName name isn't part of QnA. It's actually part of the Apply Details!
            var application = await _mediator.Send(new GetApplicationRequest(applicationId));

            var providerRouteName = application?.ApplyData?.ApplyDetails?.ProviderRouteName;

            _logger.LogInformation($"Getting ProviderRouteName for application '{applicationId}' - ProviderRouteName '{providerRouteName}'");

            return providerRouteName;
        }
    }
}
