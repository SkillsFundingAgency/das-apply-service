using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Organisations.GetOrganisation
{
    public class GetOrganisationByUserIdHandler : IRequestHandler<GetOrganisationByUserIdRequest, Organisation>
    {
        private readonly IOrganisationRepository _organisationRepository;

        public GetOrganisationByUserIdHandler(IOrganisationRepository organisationRepository)
        {
            _organisationRepository = organisationRepository;
        }

        public async Task<Organisation> Handle(GetOrganisationByUserIdRequest request, CancellationToken cancellationToken)
        {
            return await _organisationRepository.GetOrganisationByUserId(request.UserId);
        }
    }
}
