﻿using System;
using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Organisations.GetOrganisation;
using SFA.DAS.ApplyService.Application.Organisations.UpdateOrganisation;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.ApplyService.Application.Organisations.ManageOrganisation;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    [Route("organisations/")]
    public class OrganisationController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrganisationController> _logger;

        public OrganisationController(IMediator mediator, ILogger<OrganisationController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost()]
        [PerformValidation]
        public async Task<ActionResult<Organisation>> CreateOrganisation([FromBody]ManageOrganisationRequest request)
        {
            var org = await _mediator.Send(request);

            if (org is null)
            {
                return BadRequest();
            }

            return Ok(org);
        }

        [HttpPut("DirectorsAndPscs")]
        [PerformValidation]
        public async Task<ActionResult<bool>> UpdateOrganisationDirectorsAndPscs([FromBody] UpdateOrganisationDirectorsAndPscsRequest request)
        {
            var result = await _mediator.Send(request);

            if (result is false)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPut("Trustees")]
        [PerformValidation]
        public async Task<ActionResult<bool>> UpdateOrganisationTrustees([FromBody] UpdateOrganisationTrusteesRequest request)
        {
            var result = await _mediator.Send(request);

            if (result is false)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<Organisation>> GetOrganisationByName(string name)
        {
            var org = await _mediator.Send(new GetOrganisationByNameRequest { Name = WebUtility.UrlDecode(name) });

            if (org is null)
            {
                return NotFound();
            }

            return Ok(org);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<Organisation>> GetOrganisationByEmail(string email)
        {
            var org = await _mediator.Send(new GetOrganisationByContactEmailRequest { Email = email });

            if (org is null)
            {
                return NotFound();
            }

            return Ok(org);
        }
        
        [HttpGet("userid/{userId}")]

        public async Task<ActionResult<Organisation>> GetOrganisationByUserId(Guid userId)
        {
            var org = await _mediator.Send(new GetOrganisationByUserIdRequest { UserId = userId });

            if (org is null)
            {
                return NotFound();
            }

            return Ok(org);
        }

        [HttpGet("id/{organisationId}")]
        public async Task<ActionResult<Organisation>> GetOrganisationById(Guid organisationId)
        {
            var org = await _mediator.Send(new GetOrganisationByIdRequest(organisationId));

            if (org is null)
            {
                return NotFound();
            }

            return Ok(org);
        }

        [HttpGet("applicationId/{applicationId}")]
        public async Task<ActionResult<Organisation>> GetOrganisationByApplicationId(Guid applicationId)
        {
            var org = await _mediator.Send(new GetOrganisationByApplicationIdRequest(applicationId));

            if (org is null)
            {
                return NotFound();
            }

            return Ok(org);
        }

        [HttpGet("ukprn/{ukprn}")]
        public async Task<ActionResult<Organisation>> GetOrganisationByUkprn(string ukprn)
        {
            var org = await _mediator.Send(new GetOrganisationByUkprnRequest { Ukprn = ukprn });

            if (org is null)
            {
                return NotFound();
            }

            return Ok(org);
        }
    }
}