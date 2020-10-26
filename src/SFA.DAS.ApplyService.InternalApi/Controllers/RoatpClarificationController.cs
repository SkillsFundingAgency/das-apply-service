using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.Clarification;
using SFA.DAS.ApplyService.Domain.Apply.Clarification;
using SFA.DAS.ApplyService.InternalApi.Services.Assessor;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class RoatpClarificationController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IAssessorSequenceService _sequenceService;

        public RoatpClarificationController(IMediator mediator,
            IAssessorSequenceService assessorSequenceService)
        {
            _mediator = mediator;
            _sequenceService = assessorSequenceService;
        }

        [HttpGet("Clarification/Applications/{applicationId}/Overview")]
        public async Task<List<AssessorSequence>> GetClarificationOverview(Guid applicationId)
        {
            var overviewSequences = await _sequenceService.GetSequences(applicationId);

            return overviewSequences.OrderBy(seq => seq.SequenceNumber).ToList();
        }

        [HttpPost("Clarification/Applications/{applicationId}/GetAllPageReviewOutcomes")]
        public async Task<List<ClarificationPageReviewOutcome>> GetAllPageReviewOutcomes(Guid applicationId, [FromBody] GetAllPageReviewOutcomesRequest request)
        {
            var reviewOutcomes = await _mediator.Send(new GetAllClarificationPageReviewOutcomesRequest(applicationId, request.UserId));

            return reviewOutcomes;
        }


        public class GetAllPageReviewOutcomesRequest
        {
            public string UserId { get; set; }
        }
    }
}
