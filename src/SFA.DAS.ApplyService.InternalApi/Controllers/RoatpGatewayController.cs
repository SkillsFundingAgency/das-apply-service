using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply.Gateway;
using SFA.DAS.ApplyService.Application.Apply.Gateway.ExternalApiCheckDetails;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Services;
using SFA.DAS.ApplyService.InternalApi.Services.Files;
using SFA.DAS.ApplyService.InternalApi.Types;
using SFA.DAS.ApplyService.InternalApi.Types.Requests.Gateway;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class RoatpGatewayController: Controller
    {
        private readonly IGatewayApiChecksService _gatewayApiChecksService;
        private readonly ILogger<RoatpGatewayController> _logger;
        private readonly IMediator _mediator;
        private readonly IFileStorageService _fileStorageService;

        public RoatpGatewayController(ILogger<RoatpGatewayController> logger, IMediator mediator, IGatewayApiChecksService gatewayApiChecksService, IFileStorageService fileStorageService) 
        {
            _logger = logger;
            _gatewayApiChecksService = gatewayApiChecksService;
            _fileStorageService = fileStorageService;
            _mediator = mediator;
        }

        [Route("Gateway/Page/Submit")]
        [HttpPost]
         public async Task GatewayPageSubmit([FromBody] Types.Requests.UpdateGatewayPageAnswerRequest request)
        {
            _logger.LogInformation($"Submitting Gateway page submit for ApplicationId '{request.ApplicationId}' for PageId '{request.PageId}', Status '{request.Status}', " +
                                   $"Comments '{request.Comments}', Clarification answer '{request.ClarificationAnswer}'");

            await _mediator.Send(new UpdateGatewayPageAnswerRequest(request.ApplicationId,
                request.PageId,
                request.Status,
                request.Comments,
                request.UserId,
                request.UserName, request.ClarificationAnswer));
        }


         [Route("Gateway/Page/SubmitClarification")]
         [HttpPost]
         public async Task GatewayPageSubmitClarification([FromBody] Types.Requests.UpdateGatewayPageAnswerRequest request)
         {
             _logger.LogInformation($"Submitting Gateway page submit clarification for ApplicationId '{request.ApplicationId}' for PageId '{request.PageId}', Status '{request.Status}', " +
                                    $"Comments '{request.Comments}'");

             await _mediator.Send(new UpdateGatewayPageAnswerClarificationRequest(request.ApplicationId,
                 request.PageId,
                 request.Status,
                 request.Comments,
                 request.UserId,
                 request.UserName, request.ClarificationAnswer));
         }


         [Route("Gateway/Page/SubmitPostClarification")]
         [HttpPost]
         public async Task GatewayPageSubmitPostClarification([FromBody] Types.Requests.UpdateGatewayPageAnswerRequest request)
         {
             _logger.LogInformation($"Submitting Gateway page post clarification for ApplicationId '{request.ApplicationId}' for PageId '{request.PageId}', Status '{request.Status}', " +
                                    $"Comments '{request.Comments}', Clarification answer '{request.ClarificationAnswer}'");

             await _mediator.Send(new UpdateGatewayPageAnswerPostClarificationRequest(request.ApplicationId,
                 request.PageId,
                 request.Status,
                 request.Comments,
                 request.UserId,
                 request.UserName, request.ClarificationAnswer));
         }


        [HttpPost("Gateway/UpdateGatewayReviewStatusAndComment")]
        public async Task<ActionResult<bool>> UpdateGatewayReviewStatusAndComment([FromBody] UpdateGatewayReviewStatusAndCommentCommand request)
        {
            await _mediator.Send(request);
            return true;
        }

        [HttpPost("Gateway/UpdateGatewayClarification")]
        public async Task<ActionResult<bool>> UpdateGatewayClarification([FromBody] UpdateGatewayReviewStatusAsClarificationRequest request)
        {
            return await _mediator.Send(new UpdateGatewayReviewStatusAsClarificationRequest(request.ApplicationId, request.UserId, request.UserName));
        }

        [HttpPost("Gateway/{applicationId}/CommonDetails")]
        public async Task<ActionResult<GatewayCommonDetails>> GetGatewayCommonDetails(Guid applicationId, [FromBody] Types.Requests.GatewayCommonDetailsRequest request)
        {
            var application = await _mediator.Send(new GetApplicationRequest(applicationId));

            if (application?.ApplyData is null)
            {
                return NotFound();
            }
            
            if (application.GatewayReviewStatus == GatewayReviewStatus.New)
            {
                _logger.LogInformation($"Starting Gateway Review for application {application.ApplicationId}");
                await _mediator.Send(new StartGatewayReviewRequest(application.ApplicationId, request.UserId, request.UserName));

                _logger.LogInformation($"Getting external API checks data for application {application.ApplicationId}");
                var gatewayExternalApiCheckDetails = await _gatewayApiChecksService.GetExternalApiCheckDetails(application.ApplicationId);
                await _mediator.Send(new UpdateExternalApiCheckDetailsRequest(application.ApplicationId, gatewayExternalApiCheckDetails, request.UserId, request.UserName));

                // must refresh to get latest information
                application = await _mediator.Send(new GetApplicationRequest(application.ApplicationId));
            }

            var gatewayPage = await _mediator.Send(new GetGatewayPageAnswerRequest(application.ApplicationId, request.PageId));
            if(gatewayPage is null)
            {
                _logger.LogWarning($"Could not find page details for application {application.ApplicationId} | pageId {request.PageId}");
                gatewayPage = new GatewayPageAnswer
                {
                    ApplicationId = application.ApplicationId,
                    PageId = request.PageId
                };
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
                OutcomeMadeOn = gatewayPage.UpdatedAt,
                OutcomeMadeBy = gatewayPage.UpdatedBy,
                GatewaySubcontractorDeclarationClarificationUpload = application.ApplyData.GatewayReviewDetails?.GatewaySubcontractorDeclarationClarificationUpload,
                ClarificationComments = gatewayPage.ClarificationComments,
                ClarificationBy = gatewayPage.ClarificationBy,
                ClarificationDate = gatewayPage.ClarificationDate,
                ClarificationAnswer = gatewayPage.ClarificationAnswer
            };
        }

        [Route("Gateway/{applicationId}/Pages")]
        [HttpGet]
        public async Task<ActionResult<List<GatewayPageAnswerSummary>>> GetGatewayPages(Guid applicationId)
        {
            // here
            return await _mediator.Send(new GetGatewayPagesRequest(applicationId));
        }



        [HttpPost("/Gateway/SubcontractorDeclarationClarification/{applicationId}/Upload")]
        public async Task<IActionResult> UploadClarificationFile(Guid applicationId, [FromForm] SubcontractorDeclarationClarificationFileCommand command)
        {
            if (Request.Form.Files != null && Request.Form.Files.Any())
            {
                var clarificationFileName = Request.Form.Files.First().FileName;
                var uploadedSuccessfully = await _fileStorageService.UploadFiles(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations, RoatpClarificationUpload.SubcontractorDeclarationClarificationFile, Request.Form.Files, ContainerType.Gateway, new CancellationToken());

                if (!uploadedSuccessfully)
                {
                    _logger.LogError($"Unable to upload subcontractor declaration clarification for application: {applicationId} || File name {clarificationFileName}");
                    return BadRequest();
                }
                await _mediator.Send(new AddSubcontractorDeclarationFileUploadRequest(applicationId, clarificationFileName, command.UserId, command.UserName));
            }

            return Ok();
        }

        [HttpPost("/Gateway/SubcontractorDeclarationClarification/{applicationId}/Remove")]
        public async Task<IActionResult> RemoveClarificationFile(Guid applicationId, [FromBody] SubcontractorDeclarationClarificationFileCommand command)
        {
           
            var clarificationFileName = command.FileName;
            var uploadedSuccessfully = await _fileStorageService.DeleteFile(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations, RoatpClarificationUpload.SubcontractorDeclarationClarificationFile, clarificationFileName, ContainerType.Gateway, new CancellationToken());

            if (!uploadedSuccessfully)
            {
                _logger.LogError($"Unable to upload subcontractor declaration clarification for application: {applicationId} || File name {clarificationFileName}");
                return BadRequest();
            }
            await _mediator.Send(new RemoveSubcontractorDeclarationFileRequest(applicationId, clarificationFileName, command.UserId, command.UserName));
            
            return Ok();
        }
    }
}
