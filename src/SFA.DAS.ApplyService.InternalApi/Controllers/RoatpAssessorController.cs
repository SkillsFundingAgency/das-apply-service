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
        private readonly IInternalQnaApiClient _qnaApiClient;
        private readonly IAssessorLookupService _assessorLookupService;
        private readonly IGetAssessorPageService _getAssessorPageService;
        private readonly ISectorDetailsOrchestratorService _sectorDetailsOrchestratorService;

        public RoatpAssessorController(ILogger<RoatpAssessorController> logger, IMediator mediator,
            IInternalQnaApiClient qnaApiClient, IAssessorLookupService assessorLookupService,
            IGetAssessorPageService getAssessorPageService,
            ISectorDetailsOrchestratorService sectorDetailsOrchestratorService)
        {
            _logger = logger;
            _mediator = mediator;
            _qnaApiClient = qnaApiClient;
            _assessorLookupService = assessorLookupService;
            _getAssessorPageService = getAssessorPageService;
            _sectorDetailsOrchestratorService = sectorDetailsOrchestratorService;
        }

        [HttpGet("Assessor/Applications/{userId}")]
        public async Task<AssessorApplicationCounts> GetApplicationCounts(string userId)
        {
            var summary = await _mediator.Send(new AssessorApplicationCountsRequest(userId));

            return summary;
        }

        [HttpGet("Assessor/Applications/{userId}/New")]
        public async Task<List<AssessorApplicationSummary>> NewApplications(string userId)
        {
            var applications = await _mediator.Send(new NewAssessorApplicationsRequest(userId));

            return applications;
        }

        [HttpGet("Assessor/Applications/{userId}/InProgress")]
        public async Task<List<AssessorApplicationSummary>> InProgressApplications(string userId)
        {
            var applications = await _mediator.Send(new InProgressAssessorApplicationsRequest(userId));

            return applications;
        }

        [HttpGet("Assessor/Applications/{userId}/InModeration")]
        public async Task<List<ModerationApplicationSummary>> InModerationApplications(string userId)
        {
            var applications = await _mediator.Send(new ApplicationsInModerationRequest(userId));

            return applications;
        }


        [HttpPost("Assessor/Applications/{applicationId}/Assign")]
        public async Task AssignApplication(Guid applicationId, [FromBody] Controllers.AssignAssessorCommand request)
        {
            await _mediator.Send(new AssignAssessorRequest(applicationId, request.AssessorNumber,
                request.AssessorUserId, request.AssessorName));
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
                    var applySequence =
                        application.ApplyData.Sequences?.FirstOrDefault(seq => seq.SequenceNo == sequenceNumber);

                    var sequence = GetAssessorSequence(sequenceNumber, allQnaSections, applySequence);
                    overviewSequences.Add(sequence);
                }
            }

            return overviewSequences.OrderBy(seq => seq.SequenceNumber).ToList();
        }

        private AssessorSequence GetAssessorSequence(int sequenceNumber, IEnumerable<ApplicationSection> qnaSections,
            ApplySequence applySequence)
        {
            AssessorSequence sequence = null;

            if (_AssessorSequences.Contains(sequenceNumber))
            {
                var sectionsToExclude = GetWhatYouWillNeedSectionsForSequence(sequenceNumber);

                sequence = new AssessorSequence
                {
                    SequenceNumber = sequenceNumber,
                    SequenceTitle = _assessorLookupService.GetTitleForSequence(sequenceNumber),
                    Sections = qnaSections.Where(sec =>
                            sec.SequenceId == sequenceNumber && !sectionsToExclude.Contains(sec.SectionId))
                        .Select(sec =>
                        {
                            return new AssessorSection
                            { SectionNumber = sec.SectionId, LinkTitle = sec.Title, Status = string.Empty };
                        })
                        .OrderBy(sec => sec.SectionNumber).ToList()
                };

                if (applySequence != null)
                {
                    foreach (var section in sequence.Sections)
                    {
                        var applySection =
                            applySequence?.Sections?.FirstOrDefault(sec => sec.SectionNo == section.SectionNumber);

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

        [HttpPost("Assessor/Applications/{applicationId}/ChosenSectors")]
        public async Task<List<AssessorSector>> GetChosenSectors(Guid applicationId, [FromBody] Controllers.GetChosenSectorsRequest request)
        {
            var qnaSection = await _qnaApiClient.GetSectionBySectionNo(
                applicationId,
                RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining,
                RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees);

            var sectionStartingPages = qnaSection?.QnAData?.Pages.Where(x =>
                x.DisplayType == SectionDisplayType.PagesWithSections
                && x.PageId != RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ChooseYourOrganisationsSectors
                && x.Active
                && x.Complete
                && !x.NotRequired);

            var sectors = sectionStartingPages?.Select(page => new AssessorSector { Title = page.LinkTitle, PageId = page.PageId })
                .ToList();

            if (sectors == null || !sectors.Any()) return new List<AssessorSector>();

            var sectionStatusesRequest = new GetAssessorPageReviewOutcomesForSectionRequest(applicationId, RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining, RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees, request.UserId);

            var sectionStatuses = await _mediator.Send(sectionStatusesRequest);

            if (sectionStatuses == null || !sectionStatuses.Any()) return sectors;

            foreach (var sector in sectors)
            {
                foreach (var sectorStatus in sectionStatuses.Where(sectorStatus => sector.PageId == sectorStatus.PageId))
                {
                    sector.Status = sectorStatus.Status;
                }
            }

            return sectors;
        }

        [HttpGet("Assessor/Applications/{applicationId}/SectorDetails/{pageId}")]
        public async Task<AssessorSectorDetails> GetSectorDetails(Guid applicationId, string pageId)
        {
            return await _sectorDetailsOrchestratorService.GetSectorDetails(applicationId, pageId);
        }

        [HttpGet("Assessor/Applications/{applicationId}/Sequences/{sequenceNumber}/Sections/{sectionNumber}/Page")]
        public async Task<AssessorPage> GetFirstAssessorPage(Guid applicationId, int sequenceNumber, int sectionNumber)
        {
            return await GetAssessorPage(applicationId, sequenceNumber, sectionNumber, null);
        }

        [HttpGet("Assessor/Applications/{applicationId}/Sequences/{sequenceNumber}/Sections/{sectionNumber}/Page/{pageId}")]
        public async Task<AssessorPage> GetAssessorPage(Guid applicationId, int sequenceNumber, int sectionNumber,
            string pageId)
        {
            if (_AssessorSequences.Contains(sequenceNumber))
            {
                return await _getAssessorPageService.GetAssessorPage(applicationId, sequenceNumber, sectionNumber,
                    pageId);
            }

            return null;
        }

        [HttpGet("Assessor/Applications/{applicationId}/Sequences/{sequenceNumber}/Sections/{sectionNumber}/Page/{pageId}/questions/{questionId}/download/{filename}")]
        public async Task<FileStreamResult> DownloadFile(Guid applicationId, int sequenceNumber, int sectionNumber,
            string pageId, string questionId, string filename)
        {
            return await _qnaApiClient.DownloadSpecifiedFile(applicationId, sequenceNumber, sectionNumber, pageId, questionId, filename);
        }


        [HttpPost("Assessor/Applications/{applicationId}/SubmitPageReviewOutcome")]
        public async Task SubmitPageReviewOutcome(Guid applicationId, [FromBody] Controllers.SubmitPageReviewOutcomeCommand request)
        {
            await _mediator.Send(new SubmitAssessorPageOutcomeRequest(applicationId, request.SequenceNumber, request.SectionNumber, request.PageId, request.UserId, request.Status, request.Comment));
        }

        [HttpPost("Assessor/Applications/{applicationId}/GetPageReviewOutcome")]
        public async Task<AssessorPageReviewOutcome> GetPageReviewOutcome(Guid applicationId, [FromBody] Controllers.GetPageReviewOutcomeRequest request)
        {
            var pageReviewOutcome = await _mediator.Send(new Application.Apply.Assessor.GetAssessorPageReviewOutcomeRequest(applicationId, request.SequenceNumber, request.SectionNumber, request.PageId, request.UserId));

            return pageReviewOutcome;
        }

        [HttpPost("Assessor/Applications/{applicationId}/GetPageReviewOutcomesForSection")]
        public async Task<List<AssessorPageReviewOutcome>> GetPageReviewOutcomesForSection(Guid applicationId, [FromBody] Controllers.GetPageReviewOutcomesForSectionRequest request)
        {
            var assessorReviewOutcomes = await _mediator.Send(new GetAssessorPageReviewOutcomesForSectionRequest(applicationId, request.SequenceNumber, request.SectionNumber, request.UserId));

            return assessorReviewOutcomes;
        }

        [HttpPost("Assessor/Applications/{applicationId}/GetAllPageReviewOutcomes")]
        public async Task<List<AssessorPageReviewOutcome>> GetAllPageReviewOutcomes(Guid applicationId, [FromBody] Controllers.GetAllPageReviewOutcomesRequest request)
        {
            var assessorReviewOutcomes = await _mediator.Send(new GetAllAssessorPageReviewOutcomesRequest(applicationId, request.UserId));

            return assessorReviewOutcomes;
        }

        [HttpPost("Assessor/Applications/{applicationId}/UpdateAssessorReviewStatus")]
        public async Task UpdateAssessorReviewStatus(Guid applicationId, [FromBody] Controllers.UpdateAssessorReviewStatusCommand request)
        {
            await _mediator.Send(new UpdateAssessorReviewStatusRequest(applicationId, request.UserId, request.Status));
        }

    }

    public class AssignAssessorCommand
    {
        public int AssessorNumber { get; set; }
        public string AssessorUserId { get; set; }
        public string AssessorName { get; set; }
    }

    public class SubmitPageReviewOutcomeCommand
    {
        public int SequenceNumber { get; set; }
        public int SectionNumber { get; set; }
        public string PageId { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
    }

    public class GetPageReviewOutcomeRequest
    {
        public int SequenceNumber { get; set; }
        public int SectionNumber { get; set; }
        public string PageId { get; set; }
        public string UserId { get; set; }
    }

    public class GetPageReviewOutcomesForSectionRequest
    {
        public int SequenceNumber { get; set; }
        public int SectionNumber { get; set; }
        public string UserId { get; set; }
    }

    public class GetAllPageReviewOutcomesRequest
    {
        public string UserId { get; set; }
    }

    public class UpdateAssessorReviewStatusCommand
    {
        public string UserId { get; set; }
        public string Status { get; set; }
    }

    public class GetChosenSectorsRequest
    {
        public string UserId { get; set; }
    }
}