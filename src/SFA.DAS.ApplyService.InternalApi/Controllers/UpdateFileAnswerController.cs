using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.UpdateFileAnswer;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    public class UpdateFileAnswerController : Controller
    {
        private readonly IMediator _mediator;

        public UpdateFileAnswerController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost("/UpdateFileAnswer")]
        public async Task<IActionResult> Update([FromBody] UpdateFileAnswerRequest request)
        {
            await _mediator.Send(request);
            return Ok();
        }
    }
}