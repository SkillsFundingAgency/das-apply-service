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

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class RoatpAssessorController : Controller
    {
        private readonly ILogger<RoatpAssessorController> _logger;
        private readonly IMediator _mediator;
        private readonly IApplyRepository _applyRepository;
        private readonly IInternalQnaApiClient _qnaApiClient;

        public RoatpAssessorController(ILogger<RoatpAssessorController> logger, IMediator mediator, IApplyRepository applyRepository, IInternalQnaApiClient qnaApiClient)
        {
            _logger = logger;
            _mediator = mediator;
            _applyRepository = applyRepository;
            _qnaApiClient = qnaApiClient;
        }

        [HttpGet("Assessor/Applications/{userId}")]
        public async Task<RoatpAssessorSummary> AssessorSummary(string userId)
        {
            var summary = await _mediator.Send(new AssessorSummaryRequest(userId));

            return summary;
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

        [Route("Assessor/{applicationId}/Overview")]
        [HttpGet]
        public async Task<List<AssessorSequence>> GetAssessorOverview(Guid applicationId)
        {
            var allQnaSections = await _qnaApiClient.GetSections(applicationId);

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
            var sectionsToExclude = RoatpWorkflowSectionIds.GetWhatYouWillNeedSectionsForSequence(sequenceNumber);

            return new AssessorSequence
            {
                SequenceNumber = sequenceNumber,
                SequenceTitle = GetSequenceTitle(sequenceNumber),
                Sections = qnaSections.Where(sec => sec.SequenceId == sequenceNumber && !sectionsToExclude.Contains(sec.SectionId))
                .Select(sec =>
                {
                    return new AssessorSection { SectionNumber = sec.SectionId, LinkTitle = sec.Title, Status = string.Empty };
                }).ToList()
            };
        }

        private static string GetSequenceTitle(int sequenceId)
        {
            switch (sequenceId)
            {
                case RoatpWorkflowSequenceIds.ProtectingYourApprentices:
                    return "Protecting your apprentices checks";
                case RoatpWorkflowSequenceIds.ReadinessToEngage:
                    return "Readiness to engage checks";
                case RoatpWorkflowSequenceIds.PlanningApprenticeshipTraining:
                    return "Planning apprenticeship training checks";
                case RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining:
                    return "Delivering apprenticeship training checks";
                case RoatpWorkflowSequenceIds.EvaluatingApprenticeshipTraining:
                    return "Evaluating apprenticeship training checks";
                default:
                    return null;
            }
        }
    }

    public class AssignAssessorApplicationRequest
    {
        public int AssessorNumber { get; set; }
        public string AssessorUserId { get; set; }
        public string AssessorName { get; set; }
    }
}
