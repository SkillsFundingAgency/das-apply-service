using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Organisations.UpdateOrganisation
{
    public class UpdateOrganisationHandler : IRequestHandler<UpdateOrganisationRequest, Organisation>
    {
        private readonly IOrganisationRepository _organisationRepository;

        public UpdateOrganisationHandler(IOrganisationRepository organisationRepository)
        {
            _organisationRepository = organisationRepository;
        }

        public async Task<Organisation> Handle(UpdateOrganisationRequest request, CancellationToken cancellationToken)
        {
            return await UpdateOrganisationIfExists(request);
        }

        private async Task<Organisation> UpdateOrganisationIfExists(UpdateOrganisationRequest request)
        {
            var existingOrganisation = await _organisationRepository.GetOrganisationByName(request.Name);

            if (existingOrganisation != null)
            {
                existingOrganisation.OrganisationDetails = JsonConvert.SerializeObject(request.OrganisationDetails);
                existingOrganisation.OrganisationType = request.OrganisationType;
                existingOrganisation.OrganisationUkprn = request.OrganisationUkprn;
                existingOrganisation.UpdatedBy = request.UpdatedBy;
                existingOrganisation.RoEPAOApproved = request.RoEPAOApproved;
                existingOrganisation.RoATPApproved = request.RoATPApproved;

                return await _organisationRepository.UpdateOrganisation(existingOrganisation);
            }

            return null;
        }
    }
}
