using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Users.RemoveContact
{
    public class RemoveContactFromOrganisationRequest : IRequest
    {
        public Guid ContactId { get; set; }
    }
    
    public class RemoveContactFromOrganisationHandler : IRequestHandler<RemoveContactFromOrganisationRequest>
    {
        private readonly IContactRepository _contactRepository;

        public RemoveContactFromOrganisationHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        
        public async Task<Unit> Handle(RemoveContactFromOrganisationRequest request, CancellationToken cancellationToken)
        {
            await _contactRepository.RemoveContactFromOrganisation(request.ContactId);
            return Unit.Value;
        }
    }
}