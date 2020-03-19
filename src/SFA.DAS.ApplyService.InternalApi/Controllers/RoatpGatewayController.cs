using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Gateway;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Services;
using SFA.DAS.ApplyService.InternalApi.Types;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class RoatpGatewayController: Controller
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IInternalQnaApiClient _qnaApiClient;
        private readonly IGatewayApiChecksService _gatewayApiChecksService;
        private readonly ILogger<RoatpGatewayController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applyRepository"></param>
        public RoatpGatewayController(IApplyRepository applyRepository, ILogger<RoatpGatewayController> logger, IInternalQnaApiClient qnaApiClient, IGatewayApiChecksService gatewayApiChecksService) 
        {
            _applyRepository = applyRepository;
            _logger = logger;
            _qnaApiClient = qnaApiClient;
            _gatewayApiChecksService = gatewayApiChecksService;
        }

        [Route("Gateway/Page/Submit")]
        [HttpPost]
         public async Task GatewayPageSubmit([FromBody] UpsertGatewayPageAnswerRequest request)
        {
            await _applyRepository.SubmitGatewayPageAnswer(request.ApplicationId, request.PageId, request.UserName,
                request.Status, request.Comments);

        }

         [Route("Gateway/Page/CommonDetails/{applicationId}/{pageId}/{userName}")]
         [HttpGet]
         public async Task<ActionResult<GatewayCommonDetails>> GetGatewayCommonDetails(Guid applicationId, string pageId,
             string userName)
         {
             var applicationDetails = await _applyRepository.GetApplication(applicationId);
             if (applicationDetails?.ApplyData?.GatewayReviewDetails == null)
             {
                var applyGatewayDetails = await _gatewayApiChecksService.GetExternalApiCheckDetails(applicationId, userName);
                applicationDetails.ApplyData.GatewayReviewDetails = applyGatewayDetails;
             }

             var ukprn = applicationDetails.ApplyData.ApplyDetails.UKPRN;
            var organisationName = applicationDetails.ApplyData.ApplyDetails.OrganisationName;
            var checkedOn = applicationDetails?.ApplyData?.GatewayReviewDetails?.SourcesCheckedOn;
            var submittedOn = applicationDetails?.ApplyData?.ApplyDetails?.ApplicationSubmittedOn;
            var status = await _applyRepository.GetGatewayPageStatus(applicationId, pageId);
            var comments = await _applyRepository.GetGatewayPageComments(applicationId, pageId);
            var gatewayReviewStatus = applicationDetails.GatewayReviewStatus;

            var details = new GatewayCommonDetails
            {
                Ukprn = ukprn,
                ApplicationSubmittedOn = submittedOn,
                CheckedOn = checkedOn,
                LegalName = organisationName,
                Status = status,
                GatewayReviewStatus = gatewayReviewStatus,
                OptionPassText = status == GatewayAnswerStatus.Pass ? comments: null,
                OptionInProgressText = status == GatewayAnswerStatus.InProgress ? comments:null,
                OptionFailText =  status == GatewayAnswerStatus.Fail ? comments : null
            };

            return details;
         }


        [Route("Gateway/Pages")]
         [HttpGet]
         public async Task<ActionResult<List<GatewayPageAnswerSummary>>> GetGatewayPages(Guid applicationId)
        {
            var gatewayRecord = await _applyRepository.GetGatewayPageAnswers(applicationId);

            return gatewayRecord;
        }
    }
}
