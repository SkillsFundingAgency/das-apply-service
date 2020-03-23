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
            var tradingNameAndWebsitePage = await _qnaApiClient.GetPageBySectionNo(applicationId, 0, 1, RoatpWorkflowPageIds.Preamble);
            var tradingName= tradingNameAndWebsitePage?.PageOfAnswers?.SelectMany(a => a.Answers)?.FirstOrDefault(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpTradingName)?.Value;
            return tradingName;
        }
    }
}
