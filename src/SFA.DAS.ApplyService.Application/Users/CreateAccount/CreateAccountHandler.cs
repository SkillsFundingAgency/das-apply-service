using System.Threading;
using System.Threading.Tasks;
using MediatR;

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
                await _emailServiceObject.SendEmail(request.Email, 1,
                    new {GivenName = existingContact.GivenNames, FamilyName = existingContact.FamilyName});
            }
            
            return true;
        }
    }
}