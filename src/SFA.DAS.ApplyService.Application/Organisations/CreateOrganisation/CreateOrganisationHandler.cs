using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Application.Interfaces;
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
            var result = await UpdateOrganisationIfExists(request) ?? await CreateNewOrganisation(request);

            if (result != null && !string.IsNullOrEmpty(request.PrimaryContactEmail))
            {
                await _emailService.SendPreAmbleEmail(request.PrimaryContactEmail, 2, new { OrganisationName = request.Name });
            }

            return result;
        }

        private async Task<Organisation> CreateNewOrganisation(CreateOrganisationRequest request)
        {
            var organisation = new Organisation
            {
                Status = "New",
                CreatedBy = request.CreatedBy,
                Name = request.Name,
                OrganisationDetails = JsonConvert.SerializeObject(request.OrganisationDetails),
                OrganisationType = request.OrganisationType,
                OrganisationUkprn = request.OrganisationUkprn,
                RoEPAOApproved = request.RoEPAOApproved,
                RoATPApproved = request.RoATPApproved
            };

            return await _organisationRepository.CreateOrganisation(organisation, request.CreatedByUserId);
        }

        private async Task<Organisation> UpdateOrganisationIfExists(CreateOrganisationRequest request)
        {
            var existingOrganisation = await _organisationRepository.GetOrganisationByName(request.Name);

            if (existingOrganisation != null)
            {
                existingOrganisation.OrganisationDetails = JsonConvert.SerializeObject(request.OrganisationDetails);
                existingOrganisation.OrganisationType = request.OrganisationType;
                existingOrganisation.OrganisationUkprn = request.OrganisationUkprn;
                existingOrganisation.UpdatedBy = request.CreatedBy;

                if (!existingOrganisation.RoEPAOApproved) existingOrganisation.RoEPAOApproved = request.RoEPAOApproved;
                if (!existingOrganisation.RoATPApproved) existingOrganisation.RoATPApproved = request.RoATPApproved;

                return await _organisationRepository.UpdateOrganisation(existingOrganisation, request.CreatedByUserId);
            }

            return null;
        }
    }
}
