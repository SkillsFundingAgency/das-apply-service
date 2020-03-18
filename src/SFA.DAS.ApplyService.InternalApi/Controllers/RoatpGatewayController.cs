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
            var optionPassText = string.Empty;
            var optionFailText = string.Empty;
            var optionInProgressText = string.Empty;

            switch (request.Status)
            {
                case "Pass":
                    optionPassText = request.Comments;
                    break;
                case "Fail":
                    optionFailText = request.Comments;
                    break;
                case "In progress":
                    optionInProgressText = request.Comments;
                    break;
            }

            await _applyRepository.SubmitGatewayPageAnswer(request.ApplicationId, request.PageId, request.UserName,
                request.Status);

            await _applyRepository.SubmitGatewayPageDetail(request.ApplicationId, request.PageId, request.UserName, "OptionPassText",
                optionPassText);

            await _applyRepository.SubmitGatewayPageDetail(request.ApplicationId, request.PageId, request.UserName, "OptionFailText",
                optionFailText);

            await _applyRepository.SubmitGatewayPageDetail(request.ApplicationId, request.PageId, request.UserName, "OptionInProgressText",
                optionInProgressText);
        }

         [Route("Gateway/Page")]
         [HttpGet]
         public async Task<ActionResult<GatewayPageAnswer>> GetGatewayPage(Guid applicationId, string pageId)
         {
             return await _applyRepository.GetGatewayPageAnswer(applicationId, pageId);
         }


         [Route("Gateway/Page/HeaderDetails")]
         [HttpGet]
         public async Task<ActionResult<GatewayCommonDetails>> GetGatewayPageHeader(Guid applicationId, string pageId,
             string userName)
         {
             var ukprn = await GetGatewayPageItemValue(applicationId, pageId, userName, GatewayFields.UKPRN);
            var organisationName = await GetGatewayPageItemValue(applicationId, pageId, userName, GatewayFields.OrganisationName);
            var checkedOn = await GetGatewayPageItemValue(applicationId, pageId, userName, GatewayFields.SourcesCheckedOn);
            var submittedOn = await GetGatewayPageItemValue(applicationId, pageId, userName, GatewayFields.ApplicationSubmittedOn);
            var status = await GetGatewayPageItemValue(applicationId, pageId, userName, GatewayFields.Status);
            var passText = await GetGatewayPageItemValue(applicationId, pageId, userName, GatewayFields.OptionPassText);
            var failText = await GetGatewayPageItemValue(applicationId, pageId, userName, GatewayFields.OptionFailText);
            var inProgressText = await GetGatewayPageItemValue(applicationId, pageId, userName, GatewayFields.OptionInProgressText);

            var details = new GatewayCommonDetails
            {
                Ukprn = ukprn?.Value,
                ApplicationSubmittedOn = submittedOn?.Value,
                CheckedOn = checkedOn?.Value,
                LegalName = organisationName?.Value,
                Status = status?.Value,
                OptionPassText = passText?.Value,
                OptionInProgressText = inProgressText?.Value,
                OptionFailText =  failText?.Value
            };

            return details;
         }

         [Route("Gateway/Page/Value")]
         [HttpGet]
         public async Task<ActionResult<string>> GetGatewayPageItemValue(Guid applicationId, string pageId,
             string userName, string fieldName)
         {
             if (fieldName == GatewayFields.Status)
             {
                 return await _applyRepository.GetGatewayPageStatus(applicationId, pageId);
             }

             var fieldValue = await _applyRepository.GetGatewayPageAnswerValue(applicationId, pageId, fieldName);

             if (!string.IsNullOrEmpty(fieldValue))
                 return fieldValue;

             var applicationDetails = await _applyRepository.GetApplication(applicationId);

            if (applicationDetails?.ApplyData == null)
            {
                // this should never happen - so shutter page if it does
                return string.Empty;
            }

            if (applicationDetails.ApplyData?.GatewayReviewDetails == null)
            {
                var applyDetails = await _gatewayApiChecksService.GetExternalApiCheckDetails(applicationId, userName);
                applicationDetails.ApplyData.GatewayReviewDetails = applyDetails;
            }

             switch (fieldName)
             {
                 case GatewayFields.OrganisationName:
                     fieldValue = applicationDetails.ApplyData.ApplyDetails.OrganisationName;
                     break;
                 case GatewayFields.ApplicationSubmittedOn:
                     fieldValue = applicationDetails?.ApplyData?.ApplyDetails?.ApplicationSubmittedOn.ToString();
                     break;
                case GatewayFields.GatewayReviewStatus:
                    // This will need to be fresh each time
                    fieldValue = applicationDetails.GatewayReviewStatus;
                    return fieldValue;
                case GatewayFields.SourcesCheckedOn:
                     fieldValue = applicationDetails?.ApplyData?.GatewayReviewDetails?.SourcesCheckedOn?.ToString();
                     break;
                 case GatewayFields.UKPRN:
                     fieldValue = applicationDetails.ApplyData.ApplyDetails.UKPRN;
                     break;
                 case GatewayFields.UkrlpLegalName:
                     fieldValue = applicationDetails.ApplyData?.GatewayReviewDetails?.UkrlpDetails?.ProviderName;
                     break;
                case GatewayFields.CompaniesHouseName:
                     fieldValue = applicationDetails.ApplyData?.GatewayReviewDetails?.CompaniesHouseDetails?.CompanyName;
                     break;
                 case GatewayFields.CharityCommissionName:
                     fieldValue = applicationDetails.ApplyData?.GatewayReviewDetails?.CharityCommissionDetails?.CharityName;
                     break;
             }

             if (!string.IsNullOrEmpty(fieldValue))
                await _applyRepository.SubmitGatewayPageDetail(applicationId, pageId, userName, fieldName,
                    fieldValue);

             return fieldValue;
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
