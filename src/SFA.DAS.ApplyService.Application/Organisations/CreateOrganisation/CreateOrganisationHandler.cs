using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Organisations.CreateOrganisation
{
    public class CreateOrganisationHandler : IRequestHandler<CreateOrganisationRequest, Organisation>
    {
        private readonly IOrganisationRepository _organisationRepository;
        private readonly IEmailService _emailService;

        public CreateOrganisationHandler(IOrganisationRepository organisationRepository, IEmailService emailService)
        {
            _organisationRepository = organisationRepository;
            _emailService = emailService;
        }

        public async Task<Organisation> Handle(CreateOrganisationRequest request, CancellationToken cancellationToken)
        {
            return await UpdateOrganisationIfExists(request) ?? await CreateNewOrganisation(request);
        }

        private async Task<Organisation> CreateNewOrganisation(CreateOrganisationRequest request)
        {
            var organisation = new Organisation { CreatedBy = request.CreatedBy, Name = request.Name, OrganisationDetails = request.OrganisationDetails, OrganisationType = request.OrganisationType, OrganisationUkprn = request.OrganisationUkprn };
            organisation.Status = "New";

            var result = await _organisationRepository.CreateOrganisation(organisation);

            if (result != null)
            {
                await _emailService.SendPreAmbleEmail(request.PrimaryContactEmail, 2, new { OrganisationName = request.Name });
            }

            return result;
        }

        private async Task<Organisation> UpdateOrganisationIfExists(CreateOrganisationRequest request)
        {
            var existingOrganisation = await _organisationRepository.GetOrganisationByName(request.Name);

            if (existingOrganisation != null)
            {
                bool roEPAOApproved = "RoEPAO".Equals(request.OrganisationDetails?.OrganisationReferenceType, StringComparison.InvariantCultureIgnoreCase);
                bool roATPApproved = "RoATP".Equals(request.OrganisationDetails?.OrganisationReferenceType, StringComparison.InvariantCultureIgnoreCase);

                existingOrganisation.OrganisationDetails = request.OrganisationDetails;
                existingOrganisation.OrganisationType = request.OrganisationType;
                existingOrganisation.OrganisationUkprn = request.OrganisationUkprn;
                existingOrganisation.UpdatedBy = request.CreatedBy;

                existingOrganisation.RoEPAOApproved = roEPAOApproved ? roEPAOApproved : existingOrganisation.RoEPAOApproved;
                existingOrganisation.RoATPApproved = roATPApproved ? roATPApproved : existingOrganisation.RoATPApproved;

                return await _organisationRepository.UpdateOrganisation(existingOrganisation);
            }

            return null;
        }
    }
}
