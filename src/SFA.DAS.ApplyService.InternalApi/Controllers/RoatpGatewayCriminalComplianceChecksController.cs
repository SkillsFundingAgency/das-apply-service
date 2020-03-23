using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Models.Roatp;
using SFA.DAS.ApplyService.InternalApi.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class RoatpGatewayCriminalComplianceChecksController : Controller
    {

        private readonly IInternalQnaApiClient _qnaApiClient;
        private readonly ICriminalComplianceChecksQuestionLookupService _lookupService;

        public RoatpGatewayCriminalComplianceChecksController(IInternalQnaApiClient qnaApiClient, ICriminalComplianceChecksQuestionLookupService lookupService)
        {
            _qnaApiClient = qnaApiClient;
            _lookupService = lookupService;
        }

        [HttpGet("/Gateway/{applicationId}/CriminalCompliance/{gatewayPageId}")]
        public async Task<IActionResult> GetCriminalComplianceQuestionDetails(Guid applicationId, string gatewayPageId)
        {
            var qnaPageDetails = _lookupService.GetQuestionDetailsForGatewayPageId(gatewayPageId);

            var qnaPage = await _qnaApiClient.GetPageBySectionNo(applicationId, RoatpWorkflowSequenceIds.CriminalComplianceChecks,
                                                                 RoatpWorkflowSectionIds.CriminalComplianceChecks.ChecksOnYourOrganisation,
                                                                 qnaPageDetails.PageId);

            var complianceChecksQuestion = qnaPage.Questions.FirstOrDefault(x => x.QuestionId == qnaPageDetails.QuestionId);

            var criminalComplianceCheckDetails = new CriminalComplianceCheckDetails { PageId = qnaPage.PageId };

            if (complianceChecksQuestion != null)
            {
                criminalComplianceCheckDetails.QuestionText = complianceChecksQuestion.Label;
                var pageOfAnswers = qnaPage.PageOfAnswers.FirstOrDefault();
                if (pageOfAnswers != null)
                {
                    var answer = pageOfAnswers.Answers.FirstOrDefault(x => x.QuestionId == qnaPageDetails.QuestionId);
                    criminalComplianceCheckDetails.QuestionId = answer.QuestionId;
                    criminalComplianceCheckDetails.Answer = answer.Value;
                    PopulateFurtherQuestionIfAnswerMatches(complianceChecksQuestion, criminalComplianceCheckDetails, pageOfAnswers, answer);
                }
            }

            return Ok(criminalComplianceCheckDetails);
        }

        private static void PopulateFurtherQuestionIfAnswerMatches(Question complianceChecksQuestion, 
                                                             CriminalComplianceCheckDetails criminalComplianceCheckDetails, 
                                                             PageOfAnswers pageOfAnswers, Answer answer)
        {
            foreach (var option in complianceChecksQuestion.Input.Options)
            {
                if (option.FurtherQuestions != null && option.FurtherQuestions.Any() && option.Value == answer.Value)
                {
                    var furtherInformationAnswer = pageOfAnswers.Answers.FirstOrDefault(x => x.QuestionId == option.FurtherQuestions[0]?.QuestionId);
                    if (furtherInformationAnswer != null)
                    {
                        criminalComplianceCheckDetails.FurtherQuestionId = furtherInformationAnswer.QuestionId;
                        criminalComplianceCheckDetails.FurtherAnswer = furtherInformationAnswer.Value;
                        break;
                    }
                }
            }
        }
    }
}
