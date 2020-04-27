using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.Financial;
using SFA.DAS.ApplyService.Application.Apply.Financial.Applications;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class FinancialController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IInternalQnaApiClient _qnaApiClient;

        public FinancialController(IMediator mediator, IInternalQnaApiClient qnaApiClient)
        {
            _mediator = mediator;
            _qnaApiClient = qnaApiClient;
        }

        [HttpGet("/Financial/OpenApplications")]
        public async Task<ActionResult> OpenApplications()
        {
            var applications = await _mediator.Send(new OpenFinancialApplicationsRequest());
            return Ok(applications);
        }

        [HttpGet("/Financial/ClarificationApplications")]
        public async Task<ActionResult> ClarificationApplications()
        {
            var applications = await _mediator.Send(new ClarificationFinancialApplicationsRequest());
            return Ok(applications);
        }

        [HttpGet("/Financial/ClosedApplications")]
        public async Task<ActionResult> ClosedApplications()
        {
            var applications = await _mediator.Send(new ClosedFinancialApplicationsRequest());
            return Ok(applications);
        }

        [HttpGet("/Financial/StatusCounts")]
        public async Task<ActionResult> StatusCounts()
        {
            var statusCounts = await _mediator.Send(new FinancialApplicationsStatusCountsRequest());
            return Ok(statusCounts);
        }

        [HttpPost("/Financial/{applicationId}/Grade")]
        public async Task<IActionResult> RecordGrade(Guid applicationId, [FromBody] FinancialReviewDetails financialReviewDetails)
        {
            if(financialReviewDetails != null)
            {
                // Note: This gets around the WAF block
                financialReviewDetails.FinancialEvidences = await GetFinancialEvidence(applicationId);
            }

            await _mediator.Send(new RecordGradeRequest(applicationId, financialReviewDetails));
            return Ok();
        }

        [HttpPost("/Financial/{applicationId}/StartReview")]
        public async Task<ActionResult> StartReview(Guid applicationId, [FromBody] StartFinancialReviewApplicationRequest request)
        {
            await _mediator.Send(new StartFinancialReviewRequest(applicationId, request.Reviewer));
            return Ok();
        }


        private async Task<List<FinancialEvidence>> GetFinancialEvidence(Guid applicationId)
        {
            const int FinancialEvidenceSequence = 2;

            var listOfEvidence = new List<FinancialEvidence>();

            var financialSequence = await _qnaApiClient.GetSequenceBySequenceNo(applicationId, FinancialEvidenceSequence);
            var financialSections = await _qnaApiClient.GetSections(applicationId, financialSequence.Id);

            foreach (var financialSection in financialSections)
            {
                if (financialSection != null)
                {
                    var pagesContainingQuestionsWithFileupload = financialSection.QnAData.Pages.Where(x => x.Questions.Any(y => y.Input.Type == "FileUpload")).ToList();
                    foreach (var uploadPage in pagesContainingQuestionsWithFileupload)
                    {
                        foreach (var uploadQuestion in uploadPage.Questions)
                        {
                            foreach (var answer in financialSection.QnAData.Pages.SelectMany(p => p.PageOfAnswers).SelectMany(a => a.Answers).Where(a => a.QuestionId == uploadQuestion.QuestionId))
                            {
                                if (string.IsNullOrWhiteSpace(answer.ToString())) continue;
                                var filenameWithFullPath = $"{financialSequence.ApplicationId}/{financialSequence.Id}/{financialSection.Id}/{uploadPage.PageId}/{uploadQuestion.QuestionId}/{answer.Value}";
                                listOfEvidence.Add(new FinancialEvidence { Filename = filenameWithFullPath });
                            }
                        }
                    }
                }
            }

            return listOfEvidence;
        }
    }

    public class StartFinancialReviewApplicationRequest
    {
        public string Reviewer { get; set; }
    }
}