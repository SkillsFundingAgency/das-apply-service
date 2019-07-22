using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Types;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using FHADetails = SFA.DAS.ApplyService.InternalApi.Types.FHADetails;
using OrganisationDetails = SFA.DAS.ApplyService.InternalApi.Types.OrganisationDetails;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class OrganisationApiClient : ApiClientBase
    {
        private readonly ILogger<OrganisationApiClient> _logger;

        public OrganisationApiClient(ILogger<OrganisationApiClient> logger, IConfigurationService configurationService) : base(logger, configurationService)
        {
            _logger = logger;
        }

        public async Task<Organisation> Create(OrganisationSearchResult organisation, Guid userId)
        {
            var orgDetails = new OrganisationDetails
            {
                OrganisationReferenceType = organisation.OrganisationReferenceType,
                OrganisationReferenceId = organisation.OrganisationReferenceId,
                LegalName = organisation.LegalName,
                TradingName = organisation.TradingName,
                ProviderName = organisation.ProviderName,
                CompanyNumber = organisation.CompanyNumber,
                CharityNumber = organisation.CharityNumber,
                Address1 = organisation.Address?.Address1,
                Address2 = organisation.Address?.Address2,
                Address3 = organisation.Address?.Address3,
                City = organisation.Address?.City,
                Postcode = organisation.Address?.Postcode, 
                FHADetails = new FHADetails()
                {
                    FinancialDueDate = organisation.FinancialDueDate, 
                    FinancialExempt = organisation.FinancialExempt   
                }
            };

            var request = new CreateOrganisationRequest
            {
                Name = organisation.Name,
                OrganisationType = organisation.OrganisationType,
                OrganisationUkprn = organisation.Ukprn,
                RoEPAOApproved = organisation.RoEPAOApproved,
                RoATPApproved = organisation.RoATPApproved,
                OrganisationDetails = orgDetails,
                CreatedBy = userId,
                PrimaryContactEmail = organisation.Email
            };

            return await Post<Organisation>($"/Organisations", request);
        }
    }
}
