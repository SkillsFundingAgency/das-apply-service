using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Users.CreateAccount
{
    public class CreateAccountHandler : IRequestHandler<CreateAccountRequest>
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

        public async Task<Unit> Handle(CreateAccountRequest request, CancellationToken cancellationToken)
        {
            var existingContact = await _contactRepository.GetContact(request.Email);
            if (existingContact == null)
            {
                var newContact = await _contactRepository.CreateContact(request.Email, request.GivenName, request.FamilyName, "DfESignIn");
                _dfeSignInService.InviteUser(request.Email, request.GivenName, request.FamilyName, newContact.Id);
            }
            else
            {
                await _emailServiceObject.SendEmail(request.Email, 1,
                    new {GivenName = existingContact.GivenNames, FamilyName = existingContact.FamilyName});
            }
            
            return Unit.Value;
        }
    }
}