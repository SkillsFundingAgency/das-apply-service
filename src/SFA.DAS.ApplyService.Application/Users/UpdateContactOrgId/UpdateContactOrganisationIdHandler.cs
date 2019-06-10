using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Users.UpdateContactOrgId
{
    public class UpdateContactOrganisationIdHandler : IRequestHandler<UpdateContactOrganisationIdRequest>
    {
        private readonly IContactRepository _contactRepository;

        public UpdateContactOrganisationIdHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        
        public async Task<Unit> Handle(UpdateContactOrganisationIdRequest request, CancellationToken cancellationToken)
        {
            await _contactRepository.UpdateContactOrgId(request.ContactId, request.OrgId); 
            
            return Unit.Value;
        }
    }
}