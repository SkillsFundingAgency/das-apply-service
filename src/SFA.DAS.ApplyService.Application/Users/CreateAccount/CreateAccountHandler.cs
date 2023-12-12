using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Users.CreateAccount
{
    public class CreateAccountHandler : IRequestHandler<CreateAccountRequest, bool>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IDfeSignInService _dfeSignInService;
        private readonly ILogger<CreateAccountHandler> _logger;

        public CreateAccountHandler(IContactRepository contactRepository, IDfeSignInService dfeSignInService, ILogger<CreateAccountHandler> logger)
        {
            _contactRepository = contactRepository;
            _dfeSignInService = dfeSignInService;
            _logger = logger;
        }

        public async Task<bool> Handle(CreateAccountRequest request, CancellationToken cancellationToken)
        {
            var existingContact = await _contactRepository.GetContactByEmail(request.Email);
            if (existingContact == null)
            {
                var newContact = await _contactRepository.CreateContact(request.Email, request.GivenName, request.FamilyName, request.GovUkIdentifier);

                return await InviteUser(newContact);
            }
            else
            {
                return await InviteUser(existingContact);
            }
        }

        private async Task<bool> InviteUser(Domain.Entities.Contact contact)
        {
            // NOTE: An appropriate email will be generated by the Sign In Service.
            var invitationResult = await _dfeSignInService.InviteUser(contact.Email, contact.GivenNames, contact.FamilyName, contact.Id);
            if (!invitationResult.IsSuccess)
            {
                if (invitationResult.UserExists)
                {
                    await _contactRepository.UpdateSignInId(contact.Id, invitationResult.ExistingUserId, null);
                    return true;
                }

                return false;
            }

            return true;
        }
    }
}