using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Users.CreateAccount;
using SFA.DAS.ApplyService.Application.Users.GetContact;
using SFA.DAS.ApplyService.Application.Users.UpdateSignInId;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
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
        [PerformValidation]
        public async Task<ActionResult> InviteUser([FromBody]NewContact newContact)
        {
            var successful = await _mediator.Send(new CreateAccountRequest(newContact.Email, newContact.GivenName, newContact.FamilyName));

            if (!successful)
            {
                return BadRequest();
            }

            return Ok();
        }
        
        [HttpPost("/Account/Validate")]
        [PerformValidation]
        public async Task<ActionResult> InviteUserValidate([FromBody]NewContact newContact)
        {
            return Ok();
        }

        [HttpGet("/Account/{signInId:Guid}")]
        public async Task<ActionResult<Contact>> GetBySignInId(Guid signInId)
        {
            return await _mediator.Send(new GetContactBySignInIdRequest(signInId));
        }

        [PerformValidation]
        [HttpPost("/Account/Callback")]
        public async Task<ActionResult> Callback([FromBody] DfeSignInCallback callback)
        {
            await _mediator.Send(new UpdateSignInIdRequest(Guid.Parse(callback.Sub), Guid.Parse(callback.SourceId)));
            return Ok();
        }
    }
}