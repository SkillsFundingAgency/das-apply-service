using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Gateway;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.Web.Infrastructure;
using CharityCommissionApiClient = SFA.DAS.ApplyService.InternalApi.Infrastructure.CharityCommissionApiClient;
using CompaniesHouseApiClient = SFA.DAS.ApplyService.InternalApi.Infrastructure.CompaniesHouseApiClient;
using IRoatpApiClient = SFA.DAS.ApplyService.InternalApi.Infrastructure.IRoatpApiClient;


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
        private readonly IRoatpApiClient _roatpApiClient;
        private readonly CompaniesHouseApiClient _companiesHouseApiClient;

        private readonly CharityCommissionApiClient _charityCommissionApiClient;
        private readonly ILogger<RoatpGatewayController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applyRepository"></param>
        public RoatpGatewayController(IApplyRepository applyRepository, ILogger<RoatpGatewayController> logger, IInternalQnaApiClient qnaApiClient, IRoatpApiClient roatpApiClient, CompaniesHouseApiClient companiesHouseApiClient, CharityCommissionApiClient charityCommissionApiApiClient) 
        {
            _applyRepository = applyRepository;
            _logger = logger;
            _qnaApiClient = qnaApiClient;
            _roatpApiClient = roatpApiClient;
            _companiesHouseApiClient = companiesHouseApiClient;
            _charityCommissionApiClient = charityCommissionApiApiClient;
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


         [Route("Gateway/Page/Value")]
         [HttpGet]
         public async Task<ActionResult<string>> GetGatewayPageItemValue(Guid applicationId, string pageId,
             string userName, string fieldName)
         {
             var fieldValue = await _applyRepository.GetGatewayPageAnswerValue(applicationId, pageId, fieldName);

             if (!string.IsNullOrEmpty(fieldValue))
                 return fieldValue;

             if (fieldName == "OrganisationName")
             {
                 // get it from apply data
                 var applicationDetails = await _applyRepository.GetApplication(applicationId);
                 fieldValue = applicationDetails.ApplyData.ApplyDetails.OrganisationName;
             }

             if (fieldName == "ApplicationSubmittedOn")
             {
                 // get it from apply data
                 var applicationDetails = await _applyRepository.GetApplication(applicationId);
                 fieldValue = applicationDetails?.ApplyData?.ApplyDetails?.ApplicationSubmittedOn.ToString();
             }


             if (fieldName == "GatewayReviewStatus")
             {
                 // get it from apply data
                 //MFCMFC Not sure we want to store this.... should be get it fresh each time???
                 var applicationDetails = await _applyRepository.GetApplication(applicationId);
                 fieldValue = applicationDetails.GatewayReviewStatus;
             }

            // will come from Apply.ApplyGatewayDetails.GatewayDetailsGatheredOn
            if (fieldName == "SourcesCheckedOn")
             {
                 fieldValue = DateTime.Now.ToString();
             }

             if (fieldName == "UKPRN")
             {
                 fieldValue = await _qnaApiClient.GetQuestionTag(applicationId, "UKPRN");
             }

            // UkrlpLegalName
            if (fieldName == "UkrlpLegalName")
            {
                var ukprn = await GetGatewayPageItemValue(applicationId, pageId, userName, "UKPRN");
                var ukrlpData = await _roatpApiClient.GetUkrlpDetails(ukprn.Value);
                if (ukrlpData!=null && ukrlpData.Results.Any())
                {
                    var ukrlpDetail = ukrlpData.Results.First();
                    fieldValue = ukrlpDetail.ProviderName;
                }
            }

            //    // CompaniesHouseLegalName
            if (fieldName == "CompaniesHouseName")
            {
                var companyNumber = await _qnaApiClient.GetQuestionTag(applicationId, "UKRLPVerificationCompanyNumber");
                if (!string.IsNullOrEmpty(companyNumber))
                {
                    var companyDetails = await _companiesHouseApiClient.GetCompany(companyNumber);

                    if (companyDetails != null && !string.IsNullOrEmpty(companyDetails?.Response?.CompanyName))
                       fieldValue = companyDetails.Response.CompanyName;
                }
            }
            //    // CharityCommissionLegalName
            if (fieldName == "CharityCommissionName")
            {
                var charityNumber = await _qnaApiClient.GetQuestionTag(applicationId, "UKRLPVerificationCharityRegNumber");
                if (!string.IsNullOrEmpty(charityNumber) && int.TryParse(charityNumber, out var charityNumberNumeric))
                {
                    var charityDetails = await _charityCommissionApiClient.GetCharity(charityNumberNumeric);

                    if (!string.IsNullOrEmpty(charityDetails?.Name))
                        fieldValue = charityDetails.Name;
                }
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
