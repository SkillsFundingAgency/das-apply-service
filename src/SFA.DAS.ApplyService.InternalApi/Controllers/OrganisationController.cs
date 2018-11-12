using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Organisations.CreateOrganisation;
using SFA.DAS.ApplyService.Application.Organisations.GetOrganisation;
using SFA.DAS.ApplyService.Application.Organisations.UpdateOrganisation;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
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
        public async Task<ActionResult<Organisation>> CreateOrganisation([FromBody]Organisation organisation)
        {
            var request = new CreateOrganisationRequest { CreatedBy = organisation.CreatedBy, Name = organisation.Name, OrganisationDetails = organisation.OrganisationDetails, OrganisationType = organisation.OrganisationType, OrganisationUkprn = organisation.OrganisationUkprn };

            var org = await _mediator.Send(request);

            if (org is null)
            {
                return BadRequest();
            }

            return Ok(org);
        }

        [HttpPut()]
        [PerformValidation]
        public async Task<ActionResult<Organisation>> UpdateOrganisation([FromBody]Organisation organisation)
        {
            var request = new UpdateOrganisationRequest { UpdatedBy = organisation.UpdatedBy, Name = organisation.Name, OrganisationDetails = organisation.OrganisationDetails, OrganisationType = organisation.OrganisationType, OrganisationUkprn = organisation.OrganisationUkprn };

            var org = await _mediator.Send(request);

            if (org is null)
            {
                return BadRequest();
            }

            return Ok(org);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<Organisation>> GetOrganisationByName(string name)
        {
            var org = await _mediator.Send(new GetOrganisationByNameRequest { Name = name });

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
    }
}
