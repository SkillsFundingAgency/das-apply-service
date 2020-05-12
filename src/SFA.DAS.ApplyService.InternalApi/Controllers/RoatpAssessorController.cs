using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Application.Apply.Assessor;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.InternalApi.Services;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class RoatpAssessorController : Controller
    {
        private readonly ILogger<RoatpAssessorController> _logger;
        private readonly IMediator _mediator;
        private readonly IApplyRepository _applyRepository;
        private readonly IInternalQnaApiClient _qnaApiClient;
        private readonly IAssessorLookupService _assessorLookupService;

        public RoatpAssessorController(ILogger<RoatpAssessorController> logger, IMediator mediator, IApplyRepository applyRepository, IInternalQnaApiClient qnaApiClient, IAssessorLookupService assessorLookupService)
        {
            _logger = logger;
            _mediator = mediator;
            _applyRepository = applyRepository;
            _qnaApiClient = qnaApiClient;
            _assessorLookupService = assessorLookupService;
        }

        [HttpGet("Assessor/Applications/{userId}")]
        public async Task<RoatpAssessorSummary> AssessorSummary(string userId)
        {
            var summary = await _mediator.Send(new AssessorSummaryRequest(userId));

            return summary ;
        }

        [HttpGet("Assessor/Applications/{userId}/New")]
        public async Task<List<RoatpAssessorApplicationSummary>> NewApplications(string userId)
        {
            var applications = await _mediator.Send(new NewAssessorApplicationsRequest(userId));

            return applications;
        }

        [HttpPost("Assessor/Applications/{applicationId}/Assign")]
        public async Task AssignApplication(Guid applicationId, [FromBody] AssignAssessorApplicationRequest request)
        {
            await _mediator.Send(new AssignAssessorRequest(applicationId, request.AssessorNumber, request.AssessorUserId, request.AssessorName));
        }

        [HttpGet("Assessor/Applications/{userId}/InProgress")]
        public async Task<List<RoatpAssessorApplicationSummary>> InProgressApplications(string userId)
        {
            var applications = await _mediator.Send(new InProgressAssessorApplicationsRequest(userId));

            return applications;
        }

        [HttpGet("Assessor/Applications/{applicationId}/Overview")]
        public async Task<List<AssessorSequence>> GetAssessorOverview(Guid applicationId)
        {
            var allQnaSections = await _qnaApiClient.GetAllApplicationSections(applicationId);

            return new List<AssessorSequence>
            {
                GetAssessorSequence(allQnaSections, RoatpWorkflowSequenceIds.ProtectingYourApprentices),
                GetAssessorSequence(allQnaSections, RoatpWorkflowSequenceIds.ReadinessToEngage),
                GetAssessorSequence(allQnaSections, RoatpWorkflowSequenceIds.PlanningApprenticeshipTraining),
                GetAssessorSequence(allQnaSections, RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining),
                GetAssessorSequence(allQnaSections, RoatpWorkflowSequenceIds.EvaluatingApprenticeshipTraining)
            };
        }

        private AssessorSequence GetAssessorSequence(IEnumerable<ApplicationSection> qnaSections, int sequenceNumber)
        {
            var sectionsToExclude = GetWhatYouWillNeedSectionsForSequence(sequenceNumber);

            return new AssessorSequence
            {
                SequenceNumber = sequenceNumber,
                SequenceTitle = _assessorLookupService.GetTitleForSequence(sequenceNumber),
                Sections = qnaSections.Where(sec => sec.SequenceId == sequenceNumber && !sectionsToExclude.Contains(sec.SectionId))
                .Select(sec =>
                {
                    return new AssessorSection { SectionNumber = sec.SectionId, LinkTitle = sec.Title, Status = string.Empty };
                }).ToList()
            };
        }

        private static List<int> GetWhatYouWillNeedSectionsForSequence(int sequenceNumber)
        {
            var sections = new List<int>();

            switch (sequenceNumber)
            {
                case RoatpWorkflowSequenceIds.YourOrganisation:
                    sections.Add(RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed);
                    break;
                case RoatpWorkflowSequenceIds.FinancialEvidence:
                    sections.Add(RoatpWorkflowSectionIds.FinancialEvidence.WhatYouWillNeed);
                    break;
                case RoatpWorkflowSequenceIds.CriminalComplianceChecks:
                    sections.Add(RoatpWorkflowSectionIds.CriminalComplianceChecks.WhatYouWillNeed);
                    sections.Add(RoatpWorkflowSectionIds.CriminalComplianceChecks.WhatYouWillNeed_CheckOnWhosInControl);
                    break;
                case RoatpWorkflowSequenceIds.ProtectingYourApprentices:
                    sections.Add(RoatpWorkflowSectionIds.ProtectingYourApprentices.WhatYouWillNeed);
                    break;
                case RoatpWorkflowSequenceIds.ReadinessToEngage:
                    sections.Add(RoatpWorkflowSectionIds.ReadinessToEngage.WhatYouWillNeed);
                    break;
                case RoatpWorkflowSequenceIds.PlanningApprenticeshipTraining:
                    sections.Add(RoatpWorkflowSectionIds.PlanningApprenticeshipTraining.WhatYouWillNeed);
                    break;
                case RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining:
                    sections.Add(RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.WhatYouWillNeed);
                    break;
                case RoatpWorkflowSequenceIds.EvaluatingApprenticeshipTraining:
                    sections.Add(RoatpWorkflowSectionIds.EvaluatingApprenticeshipTraining.WhatYouWillNeed);
                    break;
                default:
                    break;
            }

            return sections;
        }


        [HttpGet("Assessor/Applications/{applicationId}/Sequences/{sequenceNumber}/Sections/{sectionNumber}/Page")]
        public async Task<AssessorPage> GetFirstAssessorPage(Guid applicationId, int sequenceNumber, int sectionNumber)
        {
            return await GetAssessorPage(applicationId, sequenceNumber, sectionNumber, null);
        }

        [HttpGet("Assessor/Applications/{applicationId}/Sequences/{sequenceNumber}/Sections/{sectionNumber}/Page/{pageId}")]
        public async Task<AssessorPage> GetAssessorPage(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId)
        {
            AssessorPage page = null;

            var qnaSection = await _qnaApiClient.GetSectionBySectionNo(applicationId, sequenceNumber, sectionNumber);
            var qnaPage = qnaSection?.QnAData.Pages.FirstOrDefault(p => p.PageId == pageId || string.IsNullOrEmpty(pageId));

            if (qnaPage != null)
            {
                page = new AssessorPage
                {
                    ApplicationId = applicationId,
                    SequenceNumber = sequenceNumber,
                    SectionNumber = sectionNumber,
                    PageId = qnaPage.PageId,

                    DisplayType = qnaPage.DisplayType,
                    Caption = _assessorLookupService.GetTitleForSequence(sequenceNumber) ?? qnaSection.LinkTitle,
                    Heading = _assessorLookupService.GetTitleForPage(qnaPage.PageId) ?? qnaPage.LinkTitle,
                    Title = qnaPage.Title,
                    BodyText = qnaPage.BodyText,

                    Questions = qnaPage.Questions.Select(qnaQuestion =>
                    {
                        return new AssessorQuestion
                        {
                            QuestionId = qnaQuestion.QuestionId,
                            Label = qnaQuestion.Label,
                            QuestionBodyText = qnaQuestion.QuestionBodyText,
                            InputType = qnaQuestion.Input?.Type,
                            InputPrefix = qnaQuestion.Input?.InputPrefix,
                            InputSuffix = qnaQuestion.Input?.InputSuffix
                        };
                    }).ToList(),

                    Answers = qnaPage.PageOfAnswers.SelectMany(pao => pao.Answers).ToLookup(a => a.QuestionId).Select(group =>
                    {
                        return new AssessorAnswer { QuestionId = group.Key, Value = group.FirstOrDefault()?.Value };
                    }).ToList()
                };

                var nextPageAction = await _qnaApiClient.SkipPageBySectionNo(page.ApplicationId, page.SequenceNumber, page.SectionNumber, page.PageId);

                if ("NextPage".Equals(nextPageAction?.NextAction, StringComparison.InvariantCultureIgnoreCase))
                {
                    page.NextPageId = nextPageAction.NextActionId;
                }
            }

            return page;
        }


        [HttpGet("Assessor/Applications/{applicationId}/Sequences/{sequenceNumber}/Sections/{sectionNumber}/Page/{pageId}/questions/{questionId}/download/{filename}")]
        public async Task<FileStreamResult> DownloadFile(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId, string questionId, string filename)
        {
            return await _qnaApiClient.DownloadSpecifiedFile(applicationId, sequenceNumber, sectionNumber, pageId, questionId, filename);
        }

        [HttpPost("Assessor/SubmitPageOutcome")]
        public async Task SubmitAssessorPageOutcome([FromBody] SubmitAssessorPageOutcomeRequest request)
        {
            await _mediator.Send(request);
        }

        [HttpPost("Assessor/GetPageReviewOutcome")]
        public async Task<PageReviewOutcome> GetPageReviewOutcome([FromBody] GetPageReviewOutcomeRequest request)
        {
            var pageReviewOutcome = await _mediator.Send(request);

            return pageReviewOutcome;
        }

        [HttpPost("Assessor/GetAssessorReviewOutcomesPerSection")]
        public async Task<List<PageReviewOutcome>> GetAssessorReviewOutcomesPerSection([FromBody] GetAssessorReviewOutcomesPerSectionRequest request)
        {
            var assessorReviewOutcomes = await _mediator.Send(request);

            return assessorReviewOutcomes;
        }

        [HttpPost("Assessor/GetAllAssessorReviewOutcomes")]
        public async Task<List<PageReviewOutcome>> GetAllAssessorReviewOutcomes([FromBody] GetAllAssessorReviewOutcomesRequest request)
        {
            var assessorReviewOutcomes = await _mediator.Send(request);

            return assessorReviewOutcomes;
        }
    }


    public class AssignAssessorApplicationRequest
    {
        public int AssessorNumber { get; set; }
        public string AssessorUserId { get; set; }
        public string AssessorName { get; set; }
    }


}