using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Users.GetOrganisationContacts
{
    public class GetOrganisationContactsHandler : IRequestHandler<GetOrganisationContactsRequest, List<Contact>>
    {
        private readonly IContactRepository _contactRepository;

        public GetOrganisationContactsHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<List<Contact>> Handle(GetOrganisationContactsRequest request, CancellationToken cancellationToken)
        {
            return await _contactRepository.GetOrganisationContacts(request.Id);
        }
    }
}
