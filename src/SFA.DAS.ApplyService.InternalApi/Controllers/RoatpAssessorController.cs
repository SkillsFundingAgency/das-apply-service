using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Services;
using SFA.DAS.ApplyService.InternalApi.Types;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
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
            const int protectingYourApprenticesSequenceNumber = 4;
            const int readinessToEngageSequenceNumber = 5;
            const int planningApprenticeshipTrainingSequenceNumber = 6;
            const int deliveringApprenticeshipTrainingSequenceNumber = 7;
            const int evaluatingApprenticeshipTrainingSequenceNumber = 8;

            var allQnaSections = await _qnaApiClient.GetSections(applicationId);

            return new List<AssessorSequence>
            {
                GetAssessorSequence(allQnaSections, protectingYourApprenticesSequenceNumber, "Protecting your apprentices checks"),
                GetAssessorSequence(allQnaSections, readinessToEngageSequenceNumber, "Readiness to engage checks"),
                GetAssessorSequence(allQnaSections, planningApprenticeshipTrainingSequenceNumber, "Planning apprenticeship training checks"),
                GetAssessorSequence(allQnaSections, deliveringApprenticeshipTrainingSequenceNumber, "Delivering apprenticeship training checks"),
                GetAssessorSequence(allQnaSections, evaluatingApprenticeshipTrainingSequenceNumber, "Evaluating apprenticeship training checks")
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
