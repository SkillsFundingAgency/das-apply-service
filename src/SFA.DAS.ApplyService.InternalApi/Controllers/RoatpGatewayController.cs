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
        //private readonly IRoatpApiClient _roatpApiClient;
        private readonly ILogger<RoatpGatewayController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applyRepository"></param>
        public RoatpGatewayController(IApplyRepository applyRepository, ILogger<RoatpGatewayController> logger, IInternalQnaApiClient qnaApiClient) //, IQnaApiClient qnaApiClient, IRoatpApiClient roatpApiClient)
        {
            _applyRepository = applyRepository;
            _logger = logger;
            _qnaApiClient = qnaApiClient;
            //_roatpApiClient = roatpApiClient;
        }

        [Route("Gateway/Page/Submit")]
        [HttpPost]
         public async Task GatewayPageSubmit([FromBody] UpsertGatewayPageAnswerRequest request)
        {
            _logger.LogInformation($"RoatpGatewayController-GatewayPageSubmit - ApplicationId '{request.ApplicationId}' - PageId '{request.PageId}' - Status '{request.Status}' - UserName '{request.UserName}' - PageData '{request.GatewayPageData}'");
            try
            {
                await _applyRepository.SubmitGatewayPageAnswer(request.ApplicationId, request.PageId, request.Status, request.UserName, request.GatewayPageData);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "RoatpGatewayController-GatewayPageSubmit - Error: '" + ex.Message + "'");
            }
            
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

             // will come from Apply.ApplyGatewayDetails.GatewayDetailsGatheredOn
             if (fieldName == "SourcesCheckedOn")
             {
                 fieldValue = DateTime.Now.ToString();
             }

             if (fieldName == "UKPRN")
             {
                 fieldValue = await _qnaApiClient.GetQuestionTag(applicationId, "UKPRN");
             }

             //// UkrlpLegalName
             //if (fieldName == "UkrlpLegalName")
             //{
             //    var ukprn = await GetGatewayPageItemValue(applicationId, pageId, userName, "UKPRN");
             //    var ukrlpData = await _roatpApiClient.GetUkrlpProviderDetails(ukprn.Result.ToString());
             //    if (ukrlpData.Any())
             //    {
             //        var ukrlpDetail = ukrlpData.First();
             //        fieldValue = ukrlpDetail.ProviderName;
             //    }


             //    // CompaniesHouseLegalName

             //    // CharityCommissionLegalName
         
         if (!string.IsNullOrEmpty(fieldValue))
                await _applyRepository.SubmitGatewayPageDetail(applicationId, pageId, userName, fieldName,
                    fieldValue);
        

            return fieldValue;
                
         }


        [Route("Gate way/Pages")]
         [HttpGet]
         public async Task<ActionResult<List<GatewayPageAnswerSummary>>> GetGatewayPages(Guid applicationId)
        {
            var gatewayRecord = await _applyRepository.GetGatewayPageAnswers(applicationId);

            return gatewayRecord;
        }
    }
}
