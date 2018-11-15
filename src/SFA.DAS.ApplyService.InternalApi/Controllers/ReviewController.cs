using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IMediator _mediator;

        public ReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        //public IActionResult 
    }
}