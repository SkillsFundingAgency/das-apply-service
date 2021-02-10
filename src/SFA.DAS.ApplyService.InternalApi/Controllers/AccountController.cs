using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using SFA.DAS.ApplyService.Application.Users;
using SFA.DAS.ApplyService.Application.Users.ApproveContact;
using SFA.DAS.ApplyService.Application.Users.CreateAccount;
using SFA.DAS.ApplyService.Application.Users.CreateNewContact;
using SFA.DAS.ApplyService.Application.Users.GetContact;
using SFA.DAS.ApplyService.Application.Users.GetOrganisationContacts;
using SFA.DAS.ApplyService.Application.Users.RemoveContact;
using SFA.DAS.ApplyService.Application.Users.UpdateContactIdAndSignInId;
using SFA.DAS.ApplyService.Application.Users.UpdateContactOrgId;
using SFA.DAS.ApplyService.Application.Users.UpdateSignInId;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Types;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AccountController> _logger;
        private readonly IContactRepository _contactRepository;
        private readonly IConfigurationService _configService;

        public AccountController(IMediator mediator, ILogger<AccountController> logger, IContactRepository contactRepository, IConfigurationService configService)
        {
            _mediator = mediator;
            _logger = logger;
            _contactRepository = contactRepository;
            _configService = configService;
        }

        [HttpPost("/Account/RemoveFromOrganisation")]
        public async Task<ActionResult> RemoveContactFromOrganisation([FromBody] RemoveContactFromOrganisationRequest request)
        {
            await _mediator.Send(request, CancellationToken.None);
            return Ok();
        }
        
        [HttpPost("/Account/CreateNewContact")]
        public async Task<ActionResult> CreateNewContact([FromBody] CreateNewContactRequest request)
        {
            await _mediator.Send(request, CancellationToken.None);
            return Ok();
        }
        
        [HttpPost("/Account/")]
        [PerformValidation]
        public async Task<ActionResult> InviteUser([FromBody]NewContact newContact)
        {
            var successful = await _mediator.Send(new CreateAccountRequest(newContact.Email, newContact.GivenName, newContact.FamilyName, newContact.FromAssessor));

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
        public async Task<ActionResult> Callback([FromBody] SignInCallback callback)
        {
            _logger.LogInformation($"Received callback from ASLogin: Sub: {callback.Sub} SourceId: {callback.SourceId}");
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

        [HttpPut("/Account/")]
        public async Task UpdateContactWithSignInId([FromBody] AddContactSignInId addContactSignInId)
        {
            await _mediator.Send(new UpdateContactIdAndSignInIdRequest(Guid.Parse(addContactSignInId.SignInId),
                Guid.Parse(addContactSignInId.ContactId), addContactSignInId.Email, addContactSignInId.UpdatedBy));
        }

        [HttpPut("/Account/UpdateContactWithOrgId")]
        public async Task UpdateContactWithOrgId([FromBody] UpdateContactOrgId updateContactOrgId)
        {
            await _mediator.Send(new UpdateContactOrganisationIdRequest(updateContactOrgId.ContactId,
                updateContactOrgId.OrganisationId));
        }

        [HttpPost("/Account/MigrateUsers")]
        public async Task<ActionResult> MigrateUsers()
        {
            var config = await _configService.GetConfig();
            
            var endpoint = new Uri(new Uri(config.DfeSignIn.MetadataAddress), "/Migrate"); 
            using (var httpClient = new HttpClient())
            {
                var usersToMigrate = await _contactRepository.GetUsersToMigrate();
                foreach (var user in usersToMigrate)
                {
                    var result = await httpClient.PostAsJsonAsync(endpoint, new
                    {
                        ClientId = config.DfeSignIn.ClientId, 
                        GivenName = user.GivenNames, 
                        FamilyName = user.FamilyName, 
                        Email = user.Email
                    });

                    var migrateResult = await result.Content.ReadAsAsync<MigrateUserResult>();

                    await _contactRepository.UpdateMigratedContact(user.Id, migrateResult.NewUserId);
                }
            }

            return Ok();
        }

        [HttpGet("/Account/Contact/{contactId:Guid}")]
        public async Task<ActionResult<Contact>> GetByContactId(Guid contactId)
        {
           return  await _mediator.Send(new GetContactByIdRequest(contactId));
        }

        [HttpGet("/Account/Organisation/{organisationId:Guid}/Contacts")]
        public async Task<ActionResult<List<Contact>>> GetOrganisationContacts(Guid organisationId)
        {
            return await _mediator.Send(new GetOrganisationContactsRequest(organisationId));
        }

        [HttpPost("/Account/MigrateContactAndOrgs")]
        public async Task<ActionResult> MigrateContactAndOrgs([FromBody]
            MigrateContactOrganisation migrateContactOrganisation)
        {
            await _mediator.Send(new MigrateContactOrganisationRequest(migrateContactOrganisation.contact,
                migrateContactOrganisation.organisation));

            return Ok();
        }

        public class MigrateUserResult
        {
            public Guid NewUserId { get; set; }
        }
    }
}