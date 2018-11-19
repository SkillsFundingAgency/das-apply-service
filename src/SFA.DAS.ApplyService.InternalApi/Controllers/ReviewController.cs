using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.Review;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IMediator _mediator;

        public ReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Review")]
        public async Task<ActionResult> Review()
        {
            var applications = await _mediator.Send(new ReviewRequest());
            return Ok(applications);
        }
    }

}