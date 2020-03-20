using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;

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
        
        [Route("Gateway/Page/QnaCompanyAddress/{applicationId}")]
        [HttpGet]
        public async Task<ActionResult<GetQnaCompanyAddressResult>> GetQnaCompanyAddress(Guid applicationId)
        {
            _logger.LogInformation($"Getting Company Address from QnA API for application '{applicationId}'");
            var PreamblePage = await _qnaApiClient.GetPageBySectionNo(applicationId, 0, 1, RoatpWorkflowPageIds.Preamble);
            var applyAddressLine1 = PreamblePage.PageOfAnswers.SelectMany(a => a.Answers).Where(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine1).FirstOrDefault().Value;
            var applyAddressLine2 = PreamblePage.PageOfAnswers.SelectMany(a => a.Answers).Where(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine2).FirstOrDefault().Value;
            var applyAddressLine3 = PreamblePage.PageOfAnswers.SelectMany(a => a.Answers).Where(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine3).FirstOrDefault().Value;
            var applyAddressLine4 = PreamblePage.PageOfAnswers.SelectMany(a => a.Answers).Where(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine4).FirstOrDefault().Value;
            var applyTown = PreamblePage.PageOfAnswers.SelectMany(a => a.Answers).Where(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressTown).FirstOrDefault().Value;
            var applyPostcode = PreamblePage.PageOfAnswers.SelectMany(a => a.Answers).Where(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressPostcode).FirstOrDefault().Value;

            var applyAarray = new[] { applyAddressLine1, applyAddressLine2, applyAddressLine3, applyAddressLine4, applyTown, applyPostcode };
            var applyAddress = string.Join(", ", applyAarray.Where(s => !string.IsNullOrEmpty(s)));
            _logger.LogInformation($"Getting Company Address from QnA API for application '{applicationId}', which is '{applyAddress}'");

            var returnAddress = new GetQnaCompanyAddressResult { Address = applyAddress };


            return Ok(returnAddress);
        }
    }

    public class GetQnaCompanyAddressResult
    {
        public string Address { get; set; }
    }
}