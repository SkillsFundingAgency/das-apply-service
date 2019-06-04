using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Users.UpdateContactOrgId
{
    public class UpdateContactOrgRequestHandler : IRequestHandler<UpdateContactOrgdRequest>
    {
        private readonly IContactRepository _contactRepository;

        public UpdateContactOrgRequestHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        
        public async Task<Unit> Handle(UpdateContactOrgdRequest request, CancellationToken cancellationToken)
        {
            await _contactRepository.UpdateContactOrgId(request.ContactId, request.OrgId); 
            
            return Unit.Value;
        }
    }
}