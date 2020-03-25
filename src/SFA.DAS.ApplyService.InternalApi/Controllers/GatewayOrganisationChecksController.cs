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

        public GatewayOrganisationChecksController(IInternalQnaApiClient qnaApiClient)
        {
            _qnaApiClient = qnaApiClient;
        }

        [HttpGet("/Gateway/{applicationId}/TradingName")]
        public async Task<string> GetTradingName(Guid applicationId)
        {
            return await _qnaApiClient.GetAnswerValue(applicationId, 
                RoatpWorkflowSequenceIds.Preamble, 
                RoatpWorkflowSectionIds.Preamble, 
                RoatpWorkflowPageIds.Preamble, 
                RoatpPreambleQuestionIdConstants.UkrlpTradingName);
        }

        [HttpGet("/Gateway/{applicationId}/WebsiteAddressSourcedFromUkrlp")]
        public async Task<string> GetWebsiteAddressFromUkrlp(Guid applicationId)
        {
            return await _qnaApiClient.GetAnswerValue(applicationId, 
                RoatpWorkflowSequenceIds.Preamble, 
                RoatpWorkflowSectionIds.Preamble, 
                RoatpWorkflowPageIds.Preamble, 
                RoatpPreambleQuestionIdConstants.UkrlpWebsite);
        }

        [HttpGet("/Gateway/{applicationId}/WebsiteAddressManuallyEntered")]
        public async Task<string> GetWebsiteAddressManuallyEntered(Guid applicationId)
        {
            return await _qnaApiClient.GetAnswerValue(applicationId,
                RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed,
                RoatpWorkflowPageIds.WebsiteManuallyEntered,
                RoatpYourOrganisationQuestionIdConstants.WebsiteManuallyEntered);
        }
    }
}
