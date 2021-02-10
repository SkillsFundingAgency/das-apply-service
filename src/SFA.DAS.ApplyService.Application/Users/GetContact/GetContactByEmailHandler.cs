using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Users.GetContact
{
    public class GetContactByEmailHandler : IRequestHandler<GetContactByEmailRequest, Contact>
    {
        private readonly IContactRepository _contactRepository;

        public GetContactByEmailHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        
        public async Task<Contact> Handle(GetContactByEmailRequest request, CancellationToken cancellationToken)
        {
            return await _contactRepository.GetContactByEmail(request.Email);
        }
    }
}