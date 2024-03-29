﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Organisations.UpdateOrganisation;
using SFA.DAS.ApplyService.Domain.CompaniesHouse;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Infrastructure.ApiClients;
using SFA.DAS.ApplyService.InternalApi.Types;
using FHADetails = SFA.DAS.ApplyService.InternalApi.Types.FHADetails;
using OrganisationDetails = SFA.DAS.ApplyService.InternalApi.Types.OrganisationDetails;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class OrganisationApiClient : ApiClientBase<OrganisationApiClient>, IOrganisationApiClient
    {
        public OrganisationApiClient(HttpClient httpClient, ILogger<OrganisationApiClient> logger, ITokenService tokenService) : base(httpClient, logger)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.GetToken());
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
                RoATPApproved = organisation.RoATPApproved,
                OrganisationDetails = orgDetails,
                CreatedBy = userId,
                PrimaryContactEmail = organisation.Email
            };

            return await Create(request, userId);
        }

        public async Task<Organisation> Create(CreateOrganisationRequest request, Guid userId)
        {
            return await Post<CreateOrganisationRequest, Organisation>($"/Organisations", request);
        }

        public async Task<Organisation> GetByUser(Guid userId)
        {
            return await Get<Organisation>($"Organisations/UserId/{userId}");
        }

        public async Task<Organisation> GetByApplicationId(Guid applicationId)
        {
            return await Get<Organisation>($"Organisations/ApplicationId/{applicationId}");
        }

        public async Task<bool> UpdateDirectorsAndPscs(string ukprn, List<DirectorInformation> directors, List<PersonSignificantControlInformation> personsWithSignificantControl, Guid userId)
        {
            var request = new UpdateOrganisationDirectorsAndPscsRequest
            {
                Ukprn = ukprn,
                UpdatedBy = userId,
                Directors = directors,
                PersonsSignificantControl = personsWithSignificantControl
            };

            return await Put<UpdateOrganisationDirectorsAndPscsRequest, bool>($"/Organisations/DirectorsAndPscs", request);
        }

        public async Task<bool> UpdateTrustees(string ukprn, List<InternalApi.Types.CharityCommission.Trustee> trustees, Guid userId)
        {
            var request = new UpdateOrganisationTrusteesRequest
            {
                Ukprn = ukprn,
                UpdatedBy = userId,
                Trustees = trustees
            };

            return await Put<UpdateOrganisationTrusteesRequest, bool>($"/Organisations/Trustees", request);
        }
    }
}
