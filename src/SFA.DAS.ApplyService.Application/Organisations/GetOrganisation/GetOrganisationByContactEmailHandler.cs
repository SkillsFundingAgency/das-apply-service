using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Organisations.GetOrganisation
{
    public class GetOrganisationByContactEmailHandler : IRequestHandler<GetOrganisationByContactEmailRequest, Organisation>
    {
        private readonly IOrganisationRepository _organisationRepository;

        public GetOrganisationByContactEmailHandler(IOrganisationRepository organisationRepository)
        {
            _organisationRepository = organisationRepository;
        }

        public async Task<Organisation> Handle(GetOrganisationByContactEmailRequest request, CancellationToken cancellationToken)
        {
            return await _organisationRepository.GetOrganisationByContactEmail(request.Email);
        }
    }
}
