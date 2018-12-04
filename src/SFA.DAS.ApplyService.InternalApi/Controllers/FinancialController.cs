using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.Financial;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    public class FinancialController : Controller
    {
        private readonly IMediator _mediator;

        public FinancialController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/Financial/New")]
        public async Task<ActionResult> NewApplications()
        {
            return Ok(await _mediator.Send(new NewApplicationsRequest()));
        }
           
            
    }
}