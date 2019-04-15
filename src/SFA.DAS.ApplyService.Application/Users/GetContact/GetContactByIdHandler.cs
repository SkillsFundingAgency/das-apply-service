using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Users.GetContact
{
    public class GetContactByIdHandler : IRequestHandler<GetContactByIdRequest, Contact>
    {
        private readonly IContactRepository _contactRepository;

        public GetContactByIdHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        
        public async Task<Contact> Handle(GetContactByIdRequest request, CancellationToken cancellationToken)
        {
            return await _contactRepository.GetContact(request.Id);
        }
    }
}