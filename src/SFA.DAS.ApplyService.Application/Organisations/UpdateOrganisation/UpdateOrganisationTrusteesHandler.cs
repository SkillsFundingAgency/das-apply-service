using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.CharityCommission;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Organisations.UpdateOrganisation
{
    public class UpdateOrganisationTrusteesHandler : IRequestHandler<UpdateOrganisationTrusteesRequest, bool>
    {
        private readonly IOrganisationRepository _organisationRepository;

        public UpdateOrganisationTrusteesHandler(IOrganisationRepository organisationRepository)
        {
            _organisationRepository = organisationRepository;
        }

        public async Task<bool> Handle(UpdateOrganisationTrusteesRequest request, CancellationToken cancellationToken)
        {
            var existingOrganisation = await _organisationRepository.GetOrganisationByUkprn(request.Ukprn);
            if (existingOrganisation != null)
            {
                var trustees = request.Trustees.Select(trusteeValue => new Trustee { Name = trusteeValue.Name, Id = trusteeValue.Id.ToString() }).ToList();
                existingOrganisation.OrganisationDetails.CharityCommissionDetails.Trustees = trustees;
                
                await _organisationRepository.UpdateOrganisation(existingOrganisation, request.UpdatedBy);

                return true;
            }

            return false;
        }
    }
}