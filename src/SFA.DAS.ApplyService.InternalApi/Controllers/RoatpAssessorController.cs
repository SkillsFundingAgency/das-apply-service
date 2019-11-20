using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Review.GetGatewayCounts;
using SFA.DAS.ApplyService.Application.Review.GetSubmittedApplications;
using SFA.DAS.ApplyService.Domain.Review.Gateway;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    [Route("roatp-assessor")]
    public class RoatpAssessorController : Controller
    {
        private readonly IMediator _mediator;

        public RoatpAssessorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("applications/submitted")]
        public async Task<ActionResult<List<Domain.Entities.Application>>> GetSubmittedApplications(Guid applicationId, string questionIdentifier)
        {
            return await _mediator.Send(new GetSubmittedApplicationsRequest());
        }

        [HttpGet("gateway/counts")]
        public async Task<ActionResult<GatewayCounts>> GetGatewayCounts(Guid applicationId, string questionIdentifier)
        {
            return await _mediator.Send(new GetGatewayCountsRequest());
        }
    }
}