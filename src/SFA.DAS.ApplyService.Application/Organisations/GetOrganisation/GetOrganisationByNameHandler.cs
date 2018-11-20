using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Organisations.GetOrganisation
{
    public class GetOrganisationByNameHandler : IRequestHandler<GetOrganisationByNameRequest, Organisation>
    {
        private readonly IOrganisationRepository _organisationRepository;

        public GetOrganisationByNameHandler(IOrganisationRepository organisationRepository)
        {
            _organisationRepository = organisationRepository;
        }

        public async Task<Organisation> Handle(GetOrganisationByNameRequest request, CancellationToken cancellationToken)
        {
            return await _organisationRepository.GetOrganisationByName(request.Name);
        }
    }
}
