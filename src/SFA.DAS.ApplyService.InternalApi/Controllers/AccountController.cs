using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Users.CreateAccount;
using SFA.DAS.ApplyService.InternalApi.Types;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("/Account/")]
        public async Task<ActionResult> InviteUser([FromBody]User request)
        {
            await _mediator.Send(new CreateAccountRequest(request.Email, request.GivenName, request.FamilyName));
            
            return Ok();
        }
    }
}