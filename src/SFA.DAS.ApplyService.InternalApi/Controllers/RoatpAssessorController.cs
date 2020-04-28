using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class RoatpAssessorController : Controller
    {
        private readonly ILogger<RoatpAssessorController> _logger;
        private readonly IApplyRepository _applyRepository;
        private readonly IInternalQnaApiClient _qnaApiClient;

        public RoatpAssessorController(ILogger<RoatpAssessorController> logger, IApplyRepository applyRepository, IInternalQnaApiClient qnaApiClient)
        {   
            _logger = logger;
            _applyRepository = applyRepository;
            _qnaApiClient = qnaApiClient;
        }

        [Route("Assessor/{applicationId}/Overview")]
        [HttpGet]
        public async Task<ActionResult<List<AssessorSequence>>> GetAssessorOverview(Guid applicationId)
        {
            var allQnaSections = await _qnaApiClient.GetSections(applicationId);

            return new List<AssessorSequence>
            {
                GetAssessorSequence(allQnaSections, RoatpWorkflowSequenceIds.ProtectingYourApprentices, "Protecting your apprentices checks"),
                GetAssessorSequence(allQnaSections, RoatpWorkflowSequenceIds.ReadinessToEngage, "Readiness to engage checks"),
                GetAssessorSequence(allQnaSections, RoatpWorkflowSequenceIds.PlanningApprenticeshipTraining, "Planning apprenticeship training checks"),
                GetAssessorSequence(allQnaSections, RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining, "Delivering apprenticeship training checks"),
                GetAssessorSequence(allQnaSections, RoatpWorkflowSequenceIds.EvaluatingApprenticeshipTraining, "Evaluating apprenticeship training checks")
            };
        }

        private AssessorSequence GetAssessorSequence(IEnumerable<ApplicationSection> qnaSections, int sequenceNumber, string sequenceTitle)
        {
            const int introductionSectionNumber = 1;

            return new AssessorSequence
            {
                SequenceNumber = sequenceNumber,
                SequenceTitle = sequenceTitle,
                Sections = qnaSections.Where(sec => sec.SequenceId == sequenceNumber && sec.SectionId != introductionSectionNumber)
                .Select(sec =>
                {
                    return new AssessorSection { SectionNumber = sec.SectionId, LinkTitle = sec.Title, Status = string.Empty };
                }).ToList()
            };
        }
    }
}
