using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Organisations.CreateOrganisation
{
    public class ManageOrganisationHandler : IRequestHandler<ManageOrganisationRequest, Organisation>
    {
        private readonly IOrganisationRepository _organisationRepository;
        private readonly IOrganisationAddressesRepository _organisationAddressesRepository;

        public ManageOrganisationHandler(IOrganisationRepository organisationRepository, IOrganisationAddressesRepository organisationAddressesRepository)
        {
            _organisationRepository = organisationRepository;
            _organisationAddressesRepository = organisationAddressesRepository;
        }

        public async Task<Organisation> Handle(ManageOrganisationRequest request, CancellationToken cancellationToken)
        {
            if(request.OrganisationDetails?.FHADetails != null && IsOrganationTypeFinancialExempt(request.OrganisationType))
            {
                request.OrganisationDetails.FHADetails.FinancialExempt = true;
            }

            var result = await UpdateOrganisationIfExists(request) ?? await CreateNewOrganisation(request);

            if (result != null)
            {
                _ = await UpdateOrganisationAddressesIfExists(result) ?? await CreateOrganisationAddresses(result);
            }
            return result;
        }

        private async Task<Organisation> CreateNewOrganisation(ManageOrganisationRequest request)
        {
            var organisation = new Organisation
            {
                Status = "New",
                CreatedBy = request.CreatedBy.ToString(),
                Name = request.Name,
                OrganisationDetails = request.OrganisationDetails,
                OrganisationType = request.OrganisationType,
                OrganisationUkprn = request.OrganisationUkprn,
                RoATPApproved = request.RoATPApproved
            };

            organisation.Id = await _organisationRepository.CreateOrganisation(organisation, request.CreatedBy);
            return organisation;
        }

        private async Task<Organisation> UpdateOrganisationIfExists(ManageOrganisationRequest request)
        {
            var existingOrganisation = await _organisationRepository.GetOrganisationByName(request.Name);

            if (existingOrganisation != null)
            {
                existingOrganisation.OrganisationDetails = request.OrganisationDetails;
                existingOrganisation.OrganisationType = request.OrganisationType;
                existingOrganisation.OrganisationUkprn = request.OrganisationUkprn;
                existingOrganisation.UpdatedBy = request.CreatedBy.ToString();

                if (!existingOrganisation.RoATPApproved) existingOrganisation.RoATPApproved = request.RoATPApproved;

                await _organisationRepository.UpdateOrganisation(existingOrganisation, request.CreatedBy);

                return existingOrganisation;
            }

            return null;
        }

        private async Task<OrganisationAddresses> CreateOrganisationAddresses(Organisation request)
        {
            var organisationAddresses = new OrganisationAddresses
            {
                OrganisationId = request.Id,
                AddressType = (int)OrganisationAddressType.LegalAddress,
                AddressLine1 = request.OrganisationDetails.Address1,
                AddressLine2 = request.OrganisationDetails.Address2,
                AddressLine3 = request.OrganisationDetails.Address3,
                City = request.OrganisationDetails.City,
                Postcode = request.OrganisationDetails.Postcode
            };

            organisationAddresses.Id = await _organisationAddressesRepository.CreateOrganisationAddresses(organisationAddresses);
            return organisationAddresses;
        }

        private async Task<OrganisationAddresses> UpdateOrganisationAddressesIfExists(Organisation request)
        {
            var existingOrganisationAddresses = await _organisationAddressesRepository.GetOrganisationAddressesByOrganisationId(request.Id);

            if (existingOrganisationAddresses != null)
            {
                existingOrganisationAddresses.AddressType = (int)OrganisationAddressType.LegalAddress;
                existingOrganisationAddresses.AddressLine1 = request.OrganisationDetails.Address1;
                existingOrganisationAddresses.AddressLine2 = request.OrganisationDetails.Address2;
                existingOrganisationAddresses.AddressLine3 = request.OrganisationDetails.Address3;
                existingOrganisationAddresses.City = request.OrganisationDetails.City;
                existingOrganisationAddresses.Postcode = request.OrganisationDetails.Postcode;

                await _organisationAddressesRepository.UpdateOrganisationAddresses(existingOrganisationAddresses);

                return existingOrganisationAddresses;
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
