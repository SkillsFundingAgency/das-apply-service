using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using SFA.DAS.ApplyService.Application.Users.ApproveContact;
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
        private readonly ILogger<AccountController> _logger;

        public AccountController(IMediator mediator, ILogger<AccountController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("/Account/")]
        [PerformValidation]
        public async Task<ActionResult> InviteUser([FromBody]NewContact newContact)
        {
            _logger.LogInformation($"Inviting user with params: {newContact.Email} , {newContact.GivenName} , {newContact.FamilyName}");
            var successful = await _mediator.Send(new CreateAccountRequest(newContact.Email, newContact.GivenName, newContact.FamilyName));

            if (!successful)
            {
                return BadRequest();
            }

            return Ok();
        }
        
        [HttpPost("/Account/Validate")]
        [PerformValidation]
        public ActionResult InviteUserValidate([FromBody]NewContact newContact)
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
            _logger.LogInformation($"Received callback from DfE: Sub: {callback.Sub} SourceId: {callback.SourceId}");
            await _mediator.Send(new UpdateSignInIdRequest(Guid.Parse(callback.Sub), Guid.Parse(callback.SourceId)));
            return Ok();
        }

        [HttpPost("/Account/{contactId:Guid}/approve")]
        public async Task<ActionResult> ApproveContact(Guid contactId)
        {
            var successful = await _mediator.Send(new ApproveContactRequest(contactId));

            if (!successful)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}