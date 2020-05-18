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
using SFA.DAS.ApplyService.Application.Apply.GetApplications;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class RoatpAssessorController : Controller
    {
        private static readonly List<int> _AssessorSequences = new List<int>
        {
            RoatpWorkflowSequenceIds.ProtectingYourApprentices,
            RoatpWorkflowSequenceIds.ReadinessToEngage,
            RoatpWorkflowSequenceIds.PlanningApprenticeshipTraining,
            RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining,
            RoatpWorkflowSequenceIds.EvaluatingApprenticeshipTraining
        };

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


        [HttpGet("Assessor/Applications/ChosenSectors/{applicationId}")]
        public async Task<List<Sector>> GetChosenSectors(Guid applicationId)
        {
            var qnaSection = await _qnaApiClient.GetSectionBySectionNo(
                applicationId,
                RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining,
                RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees);

            var sectionStartingPages = qnaSection?.QnAData?.Pages.Where(x=>x.DisplayType == SectionDisplayType.PagesWithSections 
                                                                           && x.PageId != RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ChooseYourOrganisationsSectors
                                                                           && x.Active
                                                                           && x.Complete 
                                                                           && !x.NotRequired);

            return sectionStartingPages?.Select(page => new Sector {Title = page.LinkTitle, PageId = page.PageId}).ToList();
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
            var overviewSequences = new List<AssessorSequence>();

            var application = await _mediator.Send(new GetApplicationRequest(applicationId));
            var allQnaSections = await _qnaApiClient.GetAllApplicationSections(applicationId);            

            if (allQnaSections != null && application?.ApplyData != null)
            {
                foreach (var sequenceNumber in _AssessorSequences)
                {
                    var applySequence = application.ApplyData.Sequences?.FirstOrDefault(seq => seq.SequenceNo == sequenceNumber);

                    var sequence = GetAssessorSequence(sequenceNumber, allQnaSections, applySequence);
                    overviewSequences.Add(sequence);
                }
            }

            return overviewSequences.OrderBy(seq => seq.SequenceNumber).ToList();
        }

        private AssessorSequence GetAssessorSequence(int sequenceNumber, IEnumerable<ApplicationSection> qnaSections, ApplySequence applySequence)
        {
            AssessorSequence sequence = null;

            if (_AssessorSequences.Contains(sequenceNumber))
            {
                var sectionsToExclude = GetWhatYouWillNeedSectionsForSequence(sequenceNumber);               

                sequence = new AssessorSequence
                {
                    SequenceNumber = sequenceNumber,
                    SequenceTitle = _assessorLookupService.GetTitleForSequence(sequenceNumber),
                    Sections = qnaSections.Where(sec => sec.SequenceId == sequenceNumber && !sectionsToExclude.Contains(sec.SectionId))
                    .Select(sec =>
                    {
                        return new AssessorSection { SectionNumber = sec.SectionId, LinkTitle = sec.Title, Status = string.Empty };
                    })
                    .OrderBy(sec => sec.SectionNumber).ToList()
                };

                if(applySequence != null)
                {
                    foreach (var section in sequence.Sections)
                    {
                        var applySection = applySequence?.Sections?.FirstOrDefault(sec => sec.SectionNo == section.SectionNumber);

                        if (applySequence.NotRequired || applySection?.NotRequired == true)
                        {
                            section.Status = "Not Required";
                        }
                    }
                }
            }

            return sequence;
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

            if (_AssessorSequences.Contains(sequenceNumber))
            {
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

                    if (nextPageAction != null && "NextPage".Equals(nextPageAction.NextAction, StringComparison.InvariantCultureIgnoreCase))
                    {
                        page.NextPageId = nextPageAction.NextActionId;
                    }
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