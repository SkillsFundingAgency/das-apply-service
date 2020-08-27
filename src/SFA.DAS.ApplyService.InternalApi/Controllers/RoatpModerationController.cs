using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;
using SFA.DAS.ApplyService.Application.Apply.Moderator;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Services;
using SFA.DAS.ApplyService.InternalApi.Types.Moderator;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class RoatpModerationController : Controller
    {
        private static readonly List<int> _ModeratorSequences = new List<int>
        {
            RoatpWorkflowSequenceIds.ProtectingYourApprentices,
            RoatpWorkflowSequenceIds.ReadinessToEngage,
            RoatpWorkflowSequenceIds.PlanningApprenticeshipTraining,
            RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining,
            RoatpWorkflowSequenceIds.EvaluatingApprenticeshipTraining
        };

        private readonly ILogger<RoatpModerationController> _logger;
        private readonly IMediator _mediator;
        private readonly IInternalQnaApiClient _qnaApiClient;
        private readonly IAssessorLookupService _assessorLookupService;
        private readonly IGetAssessorPageService _getAssessorPageService;
        private readonly ISectorDetailsOrchestratorService _sectorDetailsOrchestratorService;

        public RoatpModerationController(ILogger<RoatpModerationController> logger, IMediator mediator, IInternalQnaApiClient qnaApiClient, IAssessorLookupService assessorLookupService,
            IGetAssessorPageService getAssessorPageService, ISectorDetailsOrchestratorService sectorDetailsOrchestratorService)
        {
            _logger = logger;
            _mediator = mediator;
            _qnaApiClient = qnaApiClient;
            _assessorLookupService = assessorLookupService;
            _getAssessorPageService = getAssessorPageService;
            _sectorDetailsOrchestratorService = sectorDetailsOrchestratorService;
        }

        [HttpGet("Moderator/Applications/{applicationId}/Overview")]
        public async Task<List<ModeratorSequence>> GetModeratorOverview(Guid applicationId)
        {
            var overviewSequences = new List<ModeratorSequence>();

            var application = await _mediator.Send(new GetApplicationRequest(applicationId));
            var allQnaSections = await _qnaApiClient.GetAllApplicationSections(applicationId);

            if (allQnaSections != null && application?.ApplyData != null)
            {
                foreach (var sequenceNumber in _ModeratorSequences)
                {
                    var applySequence = application.ApplyData.Sequences?.FirstOrDefault(seq => seq.SequenceNo == sequenceNumber);

                    var sequence = GetModeratorSequence(sequenceNumber, allQnaSections, applySequence);
                    overviewSequences.Add(sequence);
                }
            }

            return overviewSequences.OrderBy(seq => seq.SequenceNumber).ToList();
        }

        private ModeratorSequence GetModeratorSequence(int sequenceNumber, IEnumerable<ApplicationSection> qnaSections, ApplySequence applySequence)
        {
            ModeratorSequence sequence = null;

            if (_ModeratorSequences.Contains(sequenceNumber))
            {
                var sectionsToExclude = GetWhatYouWillNeedSectionsForSequence(sequenceNumber);

                sequence = new ModeratorSequence
                {
                    SequenceNumber = sequenceNumber,
                    SequenceTitle = _assessorLookupService.GetTitleForSequence(sequenceNumber),
                    Sections = qnaSections.Where(sec =>sec.SequenceId == sequenceNumber && !sectionsToExclude.Contains(sec.SectionId))
                        .Select(sec =>
                        {
                            return new ModeratorSection
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

        [HttpPost("Moderator/Applications/{applicationId}/Sectors")]
        public async Task<List<ModeratorSector>> GetSectors(Guid applicationId, [FromBody] GetSectorsRequest request)
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

            var sectors = sectionStartingPages?.Select(page => new ModeratorSector { Title = page.LinkTitle, PageId = page.PageId })
                .ToList();

            if (sectors == null || !sectors.Any()) return new List<ModeratorSector>();

            var sectionStatusesRequest = new GetModeratorPageReviewOutcomesForSectionRequest(applicationId, RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining, RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees, request.UserId);

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

        [HttpGet("Moderator/Applications/{applicationId}/SectorDetails/{pageId}")]
        public async Task<Types.Assessor.AssessorSectorDetails> GetSectorDetails(Guid applicationId, string pageId)
        {
            return await _sectorDetailsOrchestratorService.GetSectorDetails(applicationId, pageId);
        }

        [HttpGet("Moderator/Applications/{applicationId}/Sequences/{sequenceNumber}/Sections/{sectionNumber}/Page")]
        public async Task<Types.Assessor.AssessorPage> GetFirstModeratorPage(Guid applicationId, int sequenceNumber, int sectionNumber)
        {
            return await GetModeratorPage(applicationId, sequenceNumber, sectionNumber, null);
        }

        [HttpGet("Moderator/Applications/{applicationId}/Sequences/{sequenceNumber}/Sections/{sectionNumber}/Page/{pageId}")]
        public async Task<Types.Assessor.AssessorPage> GetModeratorPage(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId)
        {
            if (_ModeratorSequences.Contains(sequenceNumber))
            {
                return await _getAssessorPageService.GetPage(applicationId, sequenceNumber, sectionNumber, pageId);
            }

            return null;
        }

        [HttpGet("Moderator/Applications/{applicationId}/Sequences/{sequenceNumber}/Sections/{sectionNumber}/Page/{pageId}/BlindAssessmentOutcome")]
        public async Task<BlindAssessmentOutcome> GetBlindAssessmentOutcome(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId)
        {
            var blindAssessmentOutcome = await _mediator.Send(new GetBlindAssessmentOutcomeRequest(applicationId, sequenceNumber, sectionNumber, pageId));

            return blindAssessmentOutcome;
        }

        [HttpPost("Moderator/Applications/{applicationId}/SubmitPageReviewOutcome")]
        public async Task SubmitPageReviewOutcome(Guid applicationId, [FromBody] SubmitPageReviewOutcomeCommand request)
        {
            await _mediator.Send(new SubmitModeratorPageOutcomeRequest(applicationId, request.SequenceNumber, request.SectionNumber, request.PageId, request.UserId, request.Status, request.Comment, request.ExternalComment));
        }

        [HttpPost("Moderator/Applications/{applicationId}/GetPageReviewOutcome")]
        public async Task<ModeratorPageReviewOutcome> GetPageReviewOutcome(Guid applicationId, [FromBody] GetPageReviewOutcomeRequest request)
        {
            var pageReviewOutcome = await _mediator.Send(new GetModeratorPageReviewOutcomeRequest(applicationId, request.SequenceNumber, request.SectionNumber, request.PageId, request.UserId));

            return pageReviewOutcome;
        }

        [HttpPost("Moderator/Applications/{applicationId}/GetPageReviewOutcomesForSection")]
        public async Task<List<ModeratorPageReviewOutcome>> GetPageReviewOutcomesForSection(Guid applicationId, [FromBody] GetPageReviewOutcomesForSectionRequest request)
        {
            var moderatorReviewOutcomes = await _mediator.Send(new GetModeratorPageReviewOutcomesForSectionRequest(applicationId, request.SequenceNumber, request.SectionNumber, request.UserId));

            return moderatorReviewOutcomes;
        }

        [HttpPost("Moderator/Applications/{applicationId}/GetAllPageReviewOutcomes")]
        public async Task<List<ModeratorPageReviewOutcome>> GetAllPageReviewOutcomes(Guid applicationId, [FromBody] GetAllPageReviewOutcomesRequest request)
        {
            var moderatorReviewOutcomes = await _mediator.Send(new GetAllModeratorPageReviewOutcomesRequest(applicationId, request.UserId));

            return moderatorReviewOutcomes;
        }

        public class SubmitPageReviewOutcomeCommand
        {
            public int SequenceNumber { get; set; }
            public int SectionNumber { get; set; }
            public string PageId { get; set; }
            public string UserId { get; set; }
            public string Status { get; set; }
            public string Comment { get; set; }
            public string ExternalComment { get; set; }
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

        public class GetSectorsRequest
        {
            public string UserId { get; set; }
        }
    }
}
