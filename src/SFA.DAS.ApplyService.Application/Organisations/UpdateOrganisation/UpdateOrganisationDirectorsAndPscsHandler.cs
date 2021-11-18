using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Organisations.UpdateOrganisation
{
    public class UpdateOrganisationDirectorsAndPscsHandler : IRequestHandler<UpdateOrganisationDirectorsAndPscsRequest, bool>
    {
        private readonly IOrganisationRepository _organisationRepository;

        public UpdateOrganisationDirectorsAndPscsHandler(IOrganisationRepository organisationRepository)
        {
            _organisationRepository = organisationRepository;
        }

        public async Task<bool> Handle(UpdateOrganisationDirectorsAndPscsRequest request, CancellationToken cancellationToken)
        {
            var existingOrganisation = await _organisationRepository.GetOrganisationByUkprn(request.Ukprn);

            if (existingOrganisation != null)
            {
                existingOrganisation.OrganisationDetails.CompaniesHouseDetails.Directors = request.Directors;
                existingOrganisation.OrganisationDetails.CompaniesHouseDetails.PersonsSignificationControl =
                    request.PersonsSignificantControl;
            
                await _organisationRepository.UpdateOrganisation(existingOrganisation, request.UpdatedBy);
            
                return true;
            }

            return false;
        }

      
    }
}