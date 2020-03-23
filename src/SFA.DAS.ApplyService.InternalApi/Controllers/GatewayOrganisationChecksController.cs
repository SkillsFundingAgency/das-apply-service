using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class GatewayOrganisationChecksController : Controller
    {
        private readonly IInternalQnaApiClient _qnaApiClient;

        /// <summary>
        /// Returns trading name for an application
        /// </summary>
        /// <param name="qnaApiClient"></param>
        public GatewayOrganisationChecksController(IInternalQnaApiClient qnaApiClient)
        {
            _qnaApiClient = qnaApiClient;
        }

        [HttpGet("/Gateway/{applicationId}/TradingName")]
        public async Task<string> GetTradingName(Guid applicationId)
        {
            var tradingNamePage = await _qnaApiClient.GetPageBySectionNo(applicationId, RoatpWorkflowSequenceIds.Preamble, RoatpWorkflowSectionIds.Preamble, RoatpWorkflowPageIds.Preamble);
            return tradingNamePage?.PageOfAnswers?.SelectMany(a => a.Answers)?.FirstOrDefault(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpTradingName)?.Value;
        }

        // MFCMFC need to clear out the magic numbers
        [HttpGet("/Gateway/{applicationId}/WebsiteAddressSourcedFromUkrlp")]
        public async Task<string> GetWebsiteAddressFromUkrlp(Guid applicationId)
        {
            var websiteNamePage = await _qnaApiClient.GetPageBySectionNo(applicationId, RoatpWorkflowSequenceIds.Preamble, RoatpWorkflowSectionIds.Preamble, RoatpWorkflowPageIds.Preamble);
            return websiteNamePage?.PageOfAnswers?.SelectMany(a => a.Answers)?.FirstOrDefault(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpWebsite)?.Value;
        }

        [HttpGet("/Gateway/{applicationId}/WebsiteAddressManuallyEntered")]
        public async Task<string> GetWebsiteAddressManuallyEntered(Guid applicationId)
        {
            var websiteNamePage = await _qnaApiClient.GetPageBySectionNo(applicationId, 
                RoatpWorkflowSequenceIds.YourOrganisation, 
                RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed, 
                RoatpWorkflowPageIds.WebsiteManuallyEntered);
            return websiteNamePage?.PageOfAnswers?.SelectMany(a => a.Answers)?.FirstOrDefault(a => a.QuestionId == RoatpYourOrganisationQuestionIdConstants.WebsiteManuallyEntered)?.Value;
        }
    }
}
