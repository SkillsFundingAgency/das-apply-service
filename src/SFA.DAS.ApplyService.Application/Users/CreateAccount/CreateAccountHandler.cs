using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Interfaces;

namespace SFA.DAS.ApplyService.Application.Users.CreateAccount
{
    public class CreateAccountHandler : IRequestHandler<CreateAccountRequest, bool>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IDfeSignInService _dfeSignInService;
        private readonly IEmailService _emailServiceObject;

        public CreateAccountHandler(IContactRepository contactRepository, IDfeSignInService dfeSignInService,
            IEmailService emailServiceObject)
        {
            _contactRepository = contactRepository;
            _dfeSignInService = dfeSignInService;
            _emailServiceObject = emailServiceObject;
        }

        public async Task<bool> Handle(CreateAccountRequest request, CancellationToken cancellationToken)
        {
            var existingContact = await _contactRepository.GetContact(request.Email);
            if (existingContact == null)
            {
                var newContact = await _contactRepository.CreateContact(request.Email, request.GivenName, request.FamilyName, "DfESignIn");
                var invitationResult = await _dfeSignInService.InviteUser(request.Email, request.GivenName, request.FamilyName, newContact.Id);
                if (!invitationResult.IsSuccess)
                {
                    return false;
                }
            }
            else
            {
                if (existingContact.ApplyOrganisationId == null)
                {
                    // They have signed up in Apply, but we have yet to receive a DfE Sign In id from DfE.
                    // This is either because they haven't followed the link and signed up in DfE yet, or
                    // there was a problem with the Callback from DfE.
                    // If it was a problem with the Callback, then this extra call to InviteUser will kick off another one without
                    // sending them another Invite email.
                    var invitationResult = await _dfeSignInService.InviteUser(request.Email, request.GivenName, request.FamilyName, existingContact.Id);
                    if (!invitationResult.IsSuccess)
                    {
                        return false;
                    }
                }
                
                await _emailServiceObject.SendEmail(request.Email, 1,
                    new {GivenName = existingContact.GivenNames, FamilyName = existingContact.FamilyName});
            }
            
            return true;
        }
    }
}