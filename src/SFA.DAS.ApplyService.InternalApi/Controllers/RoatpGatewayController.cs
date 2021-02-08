using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Gateway;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.InternalApi.Services;
using SFA.DAS.ApplyService.InternalApi.Services.Files;
using SFA.DAS.ApplyService.InternalApi.Types;
using SFA.DAS.ApplyService.Types;

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
        private readonly IFileStorageService _fileStorageService;
        private readonly IOversightReviewRepository _oversightReviewRepository;
        private readonly IAuditService _auditService;

        public RoatpGatewayController(IApplyRepository applyRepository, ILogger<RoatpGatewayController> logger, IMediator mediator, IGatewayApiChecksService gatewayApiChecksService, IFileStorageService fileStorageService, IOversightReviewRepository oversightReviewRepository, IAuditService auditService) 
        {
            _applyRepository = applyRepository;
            _logger = logger;
            _gatewayApiChecksService = gatewayApiChecksService;
            _fileStorageService = fileStorageService;
            _oversightReviewRepository = oversightReviewRepository;
            _auditService = auditService;
            _mediator = mediator;
        }

        [Route("Gateway/Page/Submit")]
        [HttpPost]
         public async Task GatewayPageSubmit([FromBody] Types.Requests.UpdateGatewayPageAnswerRequest request)
        {
            if(request.PageId == TwoInTwelveMonthsPageId)
            {
                await UpdateTwoInTwelveMonthApplyData(request);
            }

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
             if (request.PageId == TwoInTwelveMonthsPageId)
             {
                 await UpdateTwoInTwelveMonthApplyData(request);
             }

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
             if (request.PageId == TwoInTwelveMonthsPageId)
             {
                 await UpdateTwoInTwelveMonthApplyData(request);
             }

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
        public async Task<ActionResult<bool>> UpdateGatewayReviewStatusAndComment([FromBody] UpdateGatewayReviewStatusAndCommentRequest request)
        {
            var application = await _mediator.Send(new GetApplicationRequest(request.ApplicationId));

            if (application != null)
            {
                if (application.ApplyData.GatewayReviewDetails != null)
                {
                    application.ApplyData.GatewayReviewDetails.OutcomeDateTime = DateTime.UtcNow;
                    application.ApplyData.GatewayReviewDetails.Comments = request.GatewayReviewComment;
                    application.ApplyData.GatewayReviewDetails.ExternalComments = request.GatewayReviewExternalComment;
                }

                var result = await _applyRepository.UpdateGatewayReviewStatusAndComment(application.ApplicationId, application.ApplyData, request.GatewayReviewStatus, request.UserId, request.UserName);

                if (result && request.GatewayReviewStatus == GatewayReviewStatus.Reject)
                {
                    var oversightReview = new OversightReview
                    {
                        ApplicationId = request.ApplicationId,
                        Status = OversightReviewStatus.Rejected,
                        UserId = request.UserId,
                        UserName = request.UserName
                    };

                    _auditService.StartTracking(UserAction.UpdateGatewayReviewStatus, request.UserId, request.UserName);
                    _auditService.AuditInsert(oversightReview);
                    //await _oversightReviewRepository.Add(oversightReview);
                    //await _auditService.Save();
                }

                return result;
            }

            return false;
        }

        [HttpPost("Gateway/UpdateGatewayClarification")]
        public async Task<ActionResult<bool>> UpdateGatewayClarification([FromBody] UpdateGatewayReviewStatusAsClarificationRequest request)
        {
            return await _mediator.Send(new UpdateGatewayReviewStatusAsClarificationRequest(request.ApplicationId, request.UserId, request.UserName));
        }

        [Route("Gateway/{applicationId}/Pages/{pageId}/CommonDetails")]
        [HttpGet]
        public async Task<ActionResult<GatewayCommonDetails>> GetGatewayCommonDetails(Guid applicationId, string pageId)
        {
            var application = await _mediator.Send(new GetApplicationRequest(applicationId));

            if (application?.ApplyData is null)
            {
                return NotFound();
            }

            var gatewayPage = await _mediator.Send(new GetGatewayPageAnswerRequest(application.ApplicationId, pageId));
            if(gatewayPage is null)
            {
                gatewayPage = new GatewayPageAnswer
                {
                    ApplicationId = application.ApplicationId,
                    PageId = pageId
                };
            }

            var newDetails = new GatewayCommonDetails
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

            var logging =
                $"Getting common page details for applcationId [{applicationId}] for PageId [{pageId}]: {Newtonsoft.Json.JsonConvert.SerializeObject(newDetails)}";
            _logger.LogInformation(logging);

            return newDetails;
        }

        [Route("Gateway/{applicationId}/Pages")]
        [HttpGet]
        public async Task<ActionResult<List<GatewayPageAnswerSummary>>> GetGatewayPages(Guid applicationId)
        {
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


        private async Task UpdateTwoInTwelveMonthApplyData(Types.Requests.UpdateGatewayPageAnswerRequest request)
        {
            var application = await _mediator.Send(new GetApplicationRequest(request.ApplicationId));

            if (application.GatewayReviewStatus == GatewayReviewStatus.New)
            {
                _logger.LogInformation(
                    $"{TwoInTwelveMonthsPageId} - Starting Gateway Review for application {application.ApplicationId}");
                await _mediator.Send(new StartGatewayReviewRequest(application.ApplicationId, request.UserName));
            }

            if (request.Status == GatewayAnswerStatus.Pass)
            {
                _logger.LogInformation(
                    $"{TwoInTwelveMonthsPageId} - Getting external API checks data for application {application.ApplicationId}");
                var gatewayDetails = application.ApplyData.GatewayReviewDetails;
                var clarificationRequestedOn = gatewayDetails?.ClarificationRequestedOn;
                var clarificationRequestedBy = gatewayDetails?.ClarificationRequestedBy;
                application.ApplyData.GatewayReviewDetails =
                    await _gatewayApiChecksService.GetExternalApiCheckDetails(application.ApplicationId, request.UserName);
                application.ApplyData.GatewayReviewDetails.ClarificationRequestedBy = clarificationRequestedBy;
                application.ApplyData.GatewayReviewDetails.ClarificationRequestedOn = clarificationRequestedOn;

                await _applyRepository.UpdateApplyData(application.ApplicationId, application.ApplyData, request.UserName);
            }
        }

        public class SubcontractorDeclarationClarificationFileCommand
        {
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string FileName { get; set; }
        }
    }
}
