using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Gateway;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Services;
using SFA.DAS.ApplyService.InternalApi.Types;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class RoatpGatewayController: Controller
    {
        private const string TwoInTwelveMonthsPageId = "TwoInTwelveMonths";

        private readonly IApplyRepository _applyRepository;
        private readonly IGatewayApiChecksService _gatewayApiChecksService;
        private readonly ILogger<RoatpGatewayController> _logger;
        private readonly IMediator _mediator;

        public RoatpGatewayController(IApplyRepository applyRepository, ILogger<RoatpGatewayController> logger, IMediator mediator, IGatewayApiChecksService gatewayApiChecksService) 
        {
            _applyRepository = applyRepository;
            _logger = logger;
            _gatewayApiChecksService = gatewayApiChecksService;
            _mediator = mediator;
        }

        [Route("Gateway/Page/Submit")]
        [HttpPost]
         public async Task GatewayPageSubmit([FromBody] UpsertGatewayPageAnswerRequest request)
        {
            if(request.PageId == TwoInTwelveMonthsPageId)
            {
                var application = await _mediator.Send(new GetApplicationRequest(request.ApplicationId));

                if(application.GatewayReviewStatus == GatewayReviewStatus.New)
                {
                    _logger.LogInformation($"{TwoInTwelveMonthsPageId} - Starting Gateway Review for application {application.ApplicationId}");
                    await _mediator.Send(new StartGatewayReviewRequest(application.ApplicationId, request.UserName));
                }

                if(request.Status == GatewayAnswerStatus.Pass && application.ApplyData.GatewayReviewDetails is null)
                {
                    _logger.LogInformation($"{TwoInTwelveMonthsPageId} - Getting external API checks data for application {application.ApplicationId}");
                    application.ApplyData.GatewayReviewDetails = await _gatewayApiChecksService.GetExternalApiCheckDetails(application.ApplicationId, request.UserName);
                    await _applyRepository.UpdateApplyData(application.ApplicationId, application.ApplyData, request.UserName);
                }
            }

            await _applyRepository.SubmitGatewayPageAnswer(request.ApplicationId, request.PageId, request.UserId, request.UserName,
                request.Status, request.Comments);
        }

        [HttpPost("Gateway/UpdateGatewayReviewStatusAndComment")]
        public async Task<ActionResult<bool>> UpdateGatewayReviewStatusAndComment([FromBody] UpdateGatewayReviewStatusAndCommentRequest request)
        {
            var application = await _mediator.Send(new GetApplicationRequest(request.ApplicationId));

            if (application != null)
            {
                if(application.ApplyData.GatewayReviewDetails != null)
                {
                    application.ApplyData.GatewayReviewDetails.OutcomeDateTime = DateTime.UtcNow;
                    application.ApplyData.GatewayReviewDetails.Comments = request.GatewayReviewComment;
                }
                return await _applyRepository.UpdateGatewayReviewStatusAndComment(application.ApplicationId, application.ApplyData, request.GatewayReviewStatus, request.UserId, request.UserName);
            }

            return false;
        }

        [Route("Gateway/{applicationId}/Pages/{pageId}/CommonDetails")]
        [HttpGet]
        public async Task<ActionResult<GatewayCommonDetails>> GetGatewayCommonDetails(Guid applicationId, string pageId)
        {
            var application = await _mediator.Send(new GetApplicationRequest(applicationId));
            var gatewayPage = await _mediator.Send(new GetGatewayPageAnswerRequest(applicationId, pageId));

            if (application?.ApplyData is null || gatewayPage is null)
            {
                return NotFound();
            }

            return new GatewayCommonDetails
            {
                ApplicationId = gatewayPage.ApplicationId,
                Ukprn = application.ApplyData.ApplyDetails.UKPRN,
                ApplicationSubmittedOn = application.ApplyData.ApplyDetails.ApplicationSubmittedOn,
                GatewayOutcomeMadeOn = application.ApplyData.GatewayReviewDetails?.OutcomeDateTime,
                GatewayOutcomeMadeBy = application.GatewayUserName,
                SourcesCheckedOn = application.ApplyData.GatewayReviewDetails?.SourcesCheckedOn,
                LegalName = application.ApplyData.ApplyDetails.OrganisationName,
                ProviderRouteName = application.ApplyData.ApplyDetails.ProviderRouteName,
                ApplicationStatus = application.ApplicationStatus,
                GatewayReviewStatus = application.GatewayReviewStatus,
                PageId = gatewayPage.PageId,
                Status = gatewayPage.Status,
                Comments = gatewayPage.Comments,
                OutcomeMadeOn = gatewayPage.UpdatedAt.HasValue ? gatewayPage.UpdatedAt : gatewayPage.CreatedAt,
                OutcomeMadeBy = !string.IsNullOrEmpty(gatewayPage.UpdatedBy) ? gatewayPage.UpdatedBy : gatewayPage.CreatedBy
            };
        }

        [Route("Gateway/{applicationId}/Pages")]
        [HttpGet]
        public async Task<ActionResult<List<GatewayPageAnswerSummary>>> GetGatewayPages(Guid applicationId)
        {
            return await _mediator.Send(new GetGatewayPagesRequest(applicationId));
        }
    }
}
