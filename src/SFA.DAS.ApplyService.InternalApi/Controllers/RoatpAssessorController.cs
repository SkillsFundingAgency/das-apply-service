using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.Assessor;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class RoatpAssessorController : Controller
    {
        private readonly IMediator _mediator;

        public RoatpAssessorController(IMediator mediator)
        {
            _mediator = mediator;
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

        [HttpPost("Assessor/SubmitPageOutcome")]
        public async Task SubmitAssessorPageOutcome([FromBody] SubmitAssessorPageOutcomeRequest request)
        {
            await _mediator.Send(new SubmitAssessorPageOutcomeHandlerRequest(request.ApplicationId, 
                                                                                request.SequenceNumber, 
                                                                                request.SectionNumber, 
                                                                                request.PageId,
                                                                                request.AssessorType,
                                                                                request.UserId,
                                                                                request.Status,
                                                                                request.Comment));
        }
    }

    public class AssignAssessorApplicationRequest
    {
        public int AssessorNumber { get; set; }
        public string AssessorUserId { get; set; }
        public string AssessorName { get; set; }
    }

    public class SubmitAssessorPageOutcomeRequest
    {
        public Guid ApplicationId { get; set; }
        public int SequenceNumber { get; set; }
        public int SectionNumber { get; set; }
        public string PageId { get; set; }
        public int AssessorType { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
    }
}