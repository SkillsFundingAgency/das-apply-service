using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Organisations.UpdateOrganisation
{
    public class UpdateOrganisationHandler : IRequestHandler<UpdateOrganisationRequest, Organisation>
    {
        private readonly IOrganisationRepository _organisationRepository;
        private readonly IOrganisationAddressesRepository _organisationAddressesRepository;

        public UpdateOrganisationHandler(IOrganisationRepository organisationRepository, IOrganisationAddressesRepository organisationAddressesRepository)
        {
            _organisationRepository = organisationRepository;
            _organisationAddressesRepository = organisationAddressesRepository;
        }

        public async Task<Organisation> Handle(UpdateOrganisationRequest request, CancellationToken cancellationToken)
        {
             var result = await UpdateOrganisationIfExists(request);

            if (result != null)
            {
                await UpdateOrganisationAddressesIfExists(result);
            }
            return result;
        }

        private async Task<Organisation> UpdateOrganisationIfExists(UpdateOrganisationRequest request)
        {
            var existingOrganisation = await _organisationRepository.GetOrganisationByName(request.Name);

            if (existingOrganisation != null)
            {
                existingOrganisation.OrganisationDetails = request.OrganisationDetails;
                existingOrganisation.OrganisationType = request.OrganisationType;
                existingOrganisation.OrganisationUkprn = request.OrganisationUkprn;
                existingOrganisation.UpdatedBy = request.UpdatedBy.ToString();
                existingOrganisation.RoATPApproved = request.RoATPApproved;

                await _organisationRepository.UpdateOrganisation(existingOrganisation, request.UpdatedBy);

                return existingOrganisation;
            }

            return null;
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
    }
}
