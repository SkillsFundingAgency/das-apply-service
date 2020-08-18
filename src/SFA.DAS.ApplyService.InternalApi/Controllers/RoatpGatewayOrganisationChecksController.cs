using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Ukrlp;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Models.Roatp;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class RoatpGatewayOrganisationChecksController : Controller
    {
        private readonly IInternalQnaApiClient _qnaApiClient;
        private readonly ILogger<RoatpGatewayOrganisationChecksController> _logger;

        public RoatpGatewayOrganisationChecksController(IInternalQnaApiClient qnaApiClient, ILogger<RoatpGatewayOrganisationChecksController> logger)
        {
            _qnaApiClient = qnaApiClient;
            _logger = logger;
        }


        [HttpGet("Gateway/{applicationId}/OrganisationAddress")]
        public async Task<ActionResult<ContactAddress>> GetOrganisationAddress(Guid applicationId)
        {
            _logger.LogInformation($"Getting Company Address from QnA API for application '{applicationId}'");
            var PreamblePage = await _qnaApiClient.GetPageBySectionNo(applicationId, 0, 1, RoatpWorkflowPageIds.Preamble);

            return Ok(new ContactAddress
            {
                Address1 = PreamblePage?.PageOfAnswers.SelectMany(a => a.Answers).Where(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine1).FirstOrDefault().Value,
                Address2 = PreamblePage?.PageOfAnswers.SelectMany(a => a.Answers).Where(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine2).FirstOrDefault().Value,
                Address3 = PreamblePage?.PageOfAnswers.SelectMany(a => a.Answers).Where(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine3).FirstOrDefault().Value,
                Address4 = PreamblePage?.PageOfAnswers.SelectMany(a => a.Answers).Where(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine4).FirstOrDefault().Value,
                Town = PreamblePage?.PageOfAnswers.SelectMany(a => a.Answers).Where(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressTown).FirstOrDefault().Value,
                PostCode = PreamblePage?.PageOfAnswers.SelectMany(a => a.Answers).Where(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressPostcode).FirstOrDefault().Value
            });
        }

        [HttpGet("Gateway/{applicationId}/IcoNumber")]
        public async Task<ActionResult<string>> GetIcoNumber(Guid applicationId)
        {
            _logger.LogInformation($"RoatpGatewayOrganisationChecksController-GetIcoNumber - applicationId - '{applicationId}'");
            var page = await _qnaApiClient.GetPageBySectionNo(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails, RoatpWorkflowPageIds.YourOrganisationIcoNumber);
            var icoNumber = page?.PageOfAnswers.SelectMany(a => a.Answers).Where(a => a.QuestionId == RoatpYourOrganisationQuestionIdConstants.IcoNumber).FirstOrDefault().Value;

            return Json(new IcoNumber { ApplicationId = applicationId, Value = icoNumber});
        }
    }
}