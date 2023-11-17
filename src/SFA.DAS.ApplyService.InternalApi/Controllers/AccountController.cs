using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Users.CreateAccount;
using SFA.DAS.ApplyService.Application.Users.CreateAccountFromAsLogin;
using SFA.DAS.ApplyService.Application.Users.GetContact;
using SFA.DAS.ApplyService.Application.Users.UpdateSignInId;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Types;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
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
        public async Task<ActionResult> InviteUser([FromBody] NewContact newContact)
        {
            var successful = await _mediator.Send(new CreateAccountRequest(newContact.Email, newContact.GivenName, newContact.FamilyName, newContact.GovUkIdentifier));

            if (!successful)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("/Account/{signInId:Guid}")]
        [PerformValidation]
        public async Task<ActionResult> CreateAccountFromAsLogin(Guid signInId, [FromBody] NewContact contact)
        {
            var successful = await _mediator.Send(new CreateAccountFromAsLoginRequest(signInId, contact.Email, contact.GivenName, contact.FamilyName, contact.GovUkIdentifier, contact.UserId));

            if (!successful)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet("/Account/{signInId:Guid}")]
        public async Task<ActionResult<Contact>> GetBySignInId(Guid signInId)
        {
            return await _mediator.Send(new GetContactBySignInIdRequest(signInId));
        }

        [HttpPost("/Account/Callback")]
        [PerformValidation]
        public async Task<ActionResult> Callback([FromBody] SignInCallback callback)
        {
            _logger.LogInformation($"Received callback from ASLogin: Sub: {callback.Sub} SourceId: {callback.SourceId}");
            await _mediator.Send(new UpdateSignInIdRequest(Guid.Parse(callback.Sub), Guid.Parse(callback.SourceId), null));
            return Ok();
        }

        [HttpGet("/Account/Contact/{contactId:Guid}")]
        public async Task<ActionResult<Contact>> GetByContactId(Guid contactId)
        {
           return  await _mediator.Send(new GetContactByIdRequest(contactId));
        }

        /// <summary>
        /// Api endpoint to get the contact by given email address parameter.
        /// </summary>
        /// <param name="email">Email address.</param>
        /// <returns>Contact</returns>
        [HttpGet("/Account/Contact/{email}")]
        public async Task<ActionResult<Contact>> GetByContactEmail(string email)
        {
            return await _mediator.Send(new GetContactByEmailRequest(email));
        }
    }
}