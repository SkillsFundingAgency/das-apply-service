using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Users.GetContact
{
    public class GetContactBySignInIdHandler : IRequestHandler<GetContactBySignInIdRequest, Contact>
    {
        private readonly IContactRepository _contactRepository;

        public GetContactBySignInIdHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        
        public async Task<Contact> Handle(GetContactBySignInIdRequest request, CancellationToken cancellationToken)
        {
            return await _contactRepository.GetContactBySignInId(request.SignInId);
        }
    }
}