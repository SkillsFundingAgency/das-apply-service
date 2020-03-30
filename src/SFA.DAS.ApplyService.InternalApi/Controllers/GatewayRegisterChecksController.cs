using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class GatewayRegisterChecksController : Controller
    {
        private readonly IInternalQnaApiClient _qnaApiClient;
        private readonly IMediator _mediator;
        private readonly ILogger<GatewayRegisterChecksController> _logger;

        public GatewayRegisterChecksController(IInternalQnaApiClient qnaApiClient, IMediator mediator, ILogger<GatewayRegisterChecksController> logger)
        {
            _qnaApiClient = qnaApiClient;
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("/Gateway/{applicationId}/ProviderRoute")]
        public async Task<string> GetProviderRoute(Guid applicationId)
        {
            var providerRoute = await _qnaApiClient.GetAnswerValue(applicationId,
                RoatpWorkflowSequenceIds.Preamble,
                RoatpWorkflowSectionIds.Preamble,
                RoatpWorkflowPageIds.ProviderRoute,
                RoatpPreambleQuestionIdConstants.ApplyProviderRoute);

            _logger.LogInformation($"Getting ProviderRoute for application '{applicationId}' - ProviderRoute '{providerRoute}'");

            return providerRoute;
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
