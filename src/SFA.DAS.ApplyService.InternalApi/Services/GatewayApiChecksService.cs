using AutoMapper;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.CharityCommission;
using SFA.DAS.ApplyService.Domain.CompaniesHouse;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Ukrlp;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Services
{
    public class GatewayApiChecksService : IGatewayApiChecksService
    {
        private readonly CompaniesHouseApiClient _companiesHouseApiClient;
        private readonly CharityCommissionApiClient _charityCommissionClient;
        private readonly RoatpApiClient _roatpApiClient;
        private readonly IInternalQnaApiClient _qnaApiClient;
        private readonly ILogger<GatewayApiChecksService> _logger;

        public GatewayApiChecksService(CompaniesHouseApiClient companiesHouseApiClient, CharityCommissionApiClient charityCommissionApiClient,
                                       RoatpApiClient roatpApiClient, IInternalQnaApiClient qnaApiClient,
                                       ILogger<GatewayApiChecksService> logger)
        {
            _companiesHouseApiClient = companiesHouseApiClient;
            _charityCommissionClient = charityCommissionApiClient;
            _roatpApiClient = roatpApiClient;
            _qnaApiClient = qnaApiClient;
            _logger = logger;
        }

        public async Task<ApplyGatewayDetails> GetExternalApiCheckDetails(Guid applicationId, string userRequestedChecks)
        {
            var applyGatewayDetails = new ApplyGatewayDetails();

            var ukprn = await _qnaApiClient.GetQuestionTag(applicationId, RoatpWorkflowQuestionTags.UKPRN);

            var ukrlpResults = await _roatpApiClient.GetUkrlpDetails(ukprn);
            if (ukrlpResults == null)
            {
                var message = $"Unable to retrieve UKRLP details for application {applicationId}";
                _logger.LogError(message);
                throw new ServiceUnavailableException(message);
            }

            var ukrlpDetails = ukrlpResults.Results.FirstOrDefault();
            applyGatewayDetails.UkrlpDetails = Mapper.Map<ProviderDetails>(ukrlpDetails);

            var roatpStatus = await _roatpApiClient.GetOrganisationRegisterStatus(ukprn);
            if (roatpStatus == null)
            {
                var message = $"Unable to retrieve RoATP register details for application {applicationId}";
                _logger.LogError(message);
                throw new ServiceUnavailableException(message);
            }

            applyGatewayDetails.RoatpRegisterDetails = roatpStatus;

            var companiesHouseVerification = ukrlpDetails.VerificationDetails
                                             .FirstOrDefault(x => x.VerificationAuthority == VerificationAuthorities.CompaniesHouseAuthority);
            if (companiesHouseVerification != null)
            {
                await LookupCompaniesHouseDetails(applyGatewayDetails, companiesHouseVerification.VerificationId);
            }
            var charityCommissionVerification = ukrlpDetails.VerificationDetails
                                                .FirstOrDefault(x => x.VerificationAuthority == VerificationAuthorities.CharityCommissionAuthority);
            if (charityCommissionVerification != null)
            {
                await LookupCharityCommissionDetails(applyGatewayDetails, charityCommissionVerification.VerificationId);
            }
            applyGatewayDetails.SourcesCheckedOn = DateTime.Now;

            return await Task.FromResult(applyGatewayDetails);
        }

        private async Task LookupCompaniesHouseDetails(ApplyGatewayDetails applyGatewayDetails, string companyNumber)
        {
            var companyDetails = await _companiesHouseApiClient.GetCompany(companyNumber);
            if (companyDetails == null)
            {
                var message = $"Unable to retrieve Companies House details for company number {companyNumber}";
                _logger.LogError(message);
                throw new ServiceUnavailableException(message);
            }
            applyGatewayDetails.CompaniesHouseDetails = Mapper.Map<CompaniesHouseSummary>(companyDetails.Response);
        }

        private async Task LookupCharityCommissionDetails(ApplyGatewayDetails applyGatewayDetails, string charityNumberFromUkrlp)
        {
            var charityNumber = 0;
            int.TryParse(charityNumberFromUkrlp, out charityNumber);
            if (charityNumber != 0)
            {
                var charityDetails = await _charityCommissionClient.GetCharity(charityNumber);
                if (charityDetails == null)
                {
                    var message = $"Unable to retrieve Charity Commission details for charity number {charityNumber}";
                    _logger.LogError(message);
                    throw new ServiceUnavailableException(message);
                }
                applyGatewayDetails.CharityCommissionDetails = Mapper.Map<CharityCommissionSummary>(charityDetails);
            }
        }
    }
}
