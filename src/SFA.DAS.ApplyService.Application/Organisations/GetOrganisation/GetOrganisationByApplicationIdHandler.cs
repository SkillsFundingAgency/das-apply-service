using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Organisations.GetOrganisation
{
    public class GetOrganisationByApplicationIdHandler : IRequestHandler<GetOrganisationByApplicationIdRequest, Organisation>
    {
        private readonly IOrganisationRepository _organisationRepository;

        public GetOrganisationByApplicationIdHandler(IOrganisationRepository organisationRepository)
        {
            _organisationRepository = organisationRepository;
        }

        public async Task<Organisation> Handle(GetOrganisationByApplicationIdRequest request, CancellationToken cancellationToken)
        {
            return await _organisationRepository.GetOrganisationByApplicationId(request.ApplicationId);
        }
    }
}