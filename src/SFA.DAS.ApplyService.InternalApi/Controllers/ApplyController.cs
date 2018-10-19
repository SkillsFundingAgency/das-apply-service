using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.InternalApi.Types;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    public class ApplyController : Controller
    {
        private readonly IMediator _mediator;

        public ApplyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Apply/Start")]
        public async Task Start([FromBody]StartApplyRequest request)
        {
            await _mediator.Send(new StartApplicationRequest(request.ApplicationType, request.ApplyingOrganisationId, request.Username));
        }
    }
}