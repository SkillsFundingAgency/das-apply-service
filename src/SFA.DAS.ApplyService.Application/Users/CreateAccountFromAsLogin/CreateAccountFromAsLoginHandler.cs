using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Users.CreateAccountFromAsLogin
{
    public class CreateAccountFromAsLoginHandler : IRequestHandler<CreateAccountFromAsLoginRequest, bool>
    {
        private readonly IContactRepository _contactRepository;

        public CreateAccountFromAsLoginHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        
        public async Task<bool> Handle(CreateAccountFromAsLoginRequest request, CancellationToken cancellationToken)
        {
            var existingContact = await _contactRepository.GetContactByEmail(request.Email);

            if (existingContact is null)
            {
                var newContact = await _contactRepository.CreateContact(request.Email, request.GivenName, request.FamilyName);
                await _contactRepository.UpdateSignInId(newContact.Id, request.SignInId);
            }
            else
            {
                await _contactRepository.UpdateSignInId(existingContact.Id, request.SignInId);
            }
            
            return true;
        }
    }
}