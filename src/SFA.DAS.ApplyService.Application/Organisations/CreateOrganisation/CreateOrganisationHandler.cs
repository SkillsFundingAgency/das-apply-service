using MediatR;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

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
            if(request.OrganisationDetails?.FHADetails != null && IsOrganationTypeFinancialExempt(request.OrganisationType))
            {
                request.OrganisationDetails.FHADetails.FinancialExempt = true;
            }

            var result = await UpdateOrganisationIfExists(request) ?? await CreateNewOrganisation(request);

            if (result != null)
            {
                //await _emailService.SendEmail(EmailTemplateName.APPLY_EPAO_UPDATE, request.PrimaryContactEmail, 
                //    new { contactname = request.Name });
            }

            return result;
        }

        private async Task<Organisation> CreateNewOrganisation(CreateOrganisationRequest request)
        {
            var organisation = new Organisation
            {
                Status = "New",
                CreatedBy = request.CreatedBy.ToString(),
                Name = request.Name,
                OrganisationDetails = request.OrganisationDetails,
                OrganisationType = request.OrganisationType,
                OrganisationUkprn = request.OrganisationUkprn,
                RoEPAOApproved = request.RoEPAOApproved,
                RoATPApproved = request.RoATPApproved
            };

            organisation.Id = await _organisationRepository.CreateOrganisation(organisation, request.CreatedBy);
            return organisation;
        }

        private async Task<Organisation> UpdateOrganisationIfExists(CreateOrganisationRequest request)
        {
            var existingOrganisation = await _organisationRepository.GetOrganisationByName(request.Name);

            if (existingOrganisation != null)
            {
                existingOrganisation.OrganisationDetails = request.OrganisationDetails;
                existingOrganisation.OrganisationType = request.OrganisationType;
                existingOrganisation.OrganisationUkprn = request.OrganisationUkprn;
                existingOrganisation.UpdatedBy = request.CreatedBy.ToString();

                if (!existingOrganisation.RoEPAOApproved) existingOrganisation.RoEPAOApproved = request.RoEPAOApproved;
                if (!existingOrganisation.RoATPApproved) existingOrganisation.RoATPApproved = request.RoATPApproved;

                await _organisationRepository.UpdateOrganisation(existingOrganisation, request.CreatedBy);

                return existingOrganisation;
            }

            return null;
        }

        private static bool IsOrganationTypeFinancialExempt(string organisationType)
        {
            // This is unlikely to change. Hence, after a quick discussion, decided to hard-code these than cope with external dependencies
            return "HEI".Equals(organisationType, StringComparison.InvariantCultureIgnoreCase)
                || "College".Equals(organisationType, StringComparison.InvariantCultureIgnoreCase)
                || "Public Sector".Equals(organisationType, StringComparison.InvariantCultureIgnoreCase)
                || "Academy or Free School".Equals(organisationType, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
