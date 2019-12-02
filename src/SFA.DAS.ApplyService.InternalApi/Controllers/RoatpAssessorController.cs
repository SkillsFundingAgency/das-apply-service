﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Review.CreateGateway;
using SFA.DAS.ApplyService.Application.Review.GetGatewayCounts;
using SFA.DAS.ApplyService.Application.Review.GetGatewayInProgress;
using SFA.DAS.ApplyService.Application.Review.GetGatewayReview;
using SFA.DAS.ApplyService.Application.Review.GetRejectedOutcomes;
using SFA.DAS.ApplyService.Application.Review.GetSubmittedApplications;
using SFA.DAS.ApplyService.Application.Review.UpdateGatewayOutcomes;
using SFA.DAS.ApplyService.Domain.Entities.Review;
using SFA.DAS.ApplyService.Domain.Review;
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

        [HttpGet("gateway/in-progress")]
        public async Task<ActionResult<List<Gateway>>> GetGatewayInProgress()
        {
            return await _mediator.Send(new GetGatewayInProgressRequest());
        }

        [HttpPost("gateway/create")]
        public async Task<ActionResult> CreateGateway([FromBody] CreateGatewayRequest command)
        {
            await _mediator.Send(command);

            return Ok();
        }

        [HttpGet("gateway/counts")]
        public async Task<ActionResult<GatewayCounts>> GetGatewayCounts()
        {
            return await _mediator.Send(new GetGatewayCountsRequest());
        }

        [HttpPost("gateway/outcomes")]
        public async Task<ActionResult> PostGatewayOutcomes([FromBody] UpdateGatewayOutcomesCommand command)
        {
            await _mediator.Send(command);

            return Ok();
        }

        [HttpGet("review/rejected-outcomes/{applicationId:Guid}/{sectionId}/{pageId}")]
        public async Task<ActionResult<List<Outcome>>> GetRejectedOutcomes([FromRoute] Guid applicationId, [FromRoute]string sectionId, [FromRoute]string pageId)
        {
            var request = new GetRejectedOutcomesRequest(applicationId, sectionId, pageId);

            return await _mediator.Send(request);
        }

        [HttpGet("gateway/{applicationId:guid}")]
        public async Task<ActionResult<Gateway>> GetGatewayReview([FromRoute] Guid applicationId)
        {
            var request = new GetGatewayReviewRequest(applicationId);

            return await _mediator.Send(request);
        }
    }
}