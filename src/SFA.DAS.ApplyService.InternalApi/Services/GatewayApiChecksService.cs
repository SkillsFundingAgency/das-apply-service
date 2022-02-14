using AutoMapper;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.CharityCommission;
using SFA.DAS.ApplyService.Domain.CompaniesHouse;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Ukrlp;
using SFA.DAS.ApplyService.Infrastructure.ApiClients;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using SFA.DAS.ApplyService.InternalApi.Types;
using SFA.DAS.ApplyService.InternalApi.Types.CharityCommission;

//MFCMFC using ProviderDetails = SFA.DAS.ApplyService.Domain.Ukrlp.ProviderDetails;

namespace SFA.DAS.ApplyService.InternalApi.Services
{
    public class GatewayApiChecksService : IGatewayApiChecksService
    {
        private readonly CompaniesHouseApiClient _companiesHouseApiClient;
        private readonly IOuterApiClient _outerApiClient;
        private readonly IRoatpApiClient _roatpApiClient;
        private readonly IInternalQnaApiClient _qnaApiClient;
        private readonly ILogger<GatewayApiChecksService> _logger;
        private readonly AsyncRetryPolicy _retryPolicy;

        public GatewayApiChecksService(
            CompaniesHouseApiClient companiesHouseApiClient, 
            IOuterApiClient outerApiClient,
            IRoatpApiClient roatpApiClient, 
            IInternalQnaApiClient qnaApiClient,
            ILogger<GatewayApiChecksService> logger)
        {
            _companiesHouseApiClient = companiesHouseApiClient;
            _outerApiClient = outerApiClient;
            _roatpApiClient = roatpApiClient;
            _qnaApiClient = qnaApiClient;
            _logger = logger;
            _retryPolicy = GetRetryPolicy();
        }

        public async Task<ApplyGatewayDetails> GetExternalApiCheckDetails(Guid applicationId)
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

            return applyGatewayDetails;
        }

        private async Task LookupCompaniesHouseDetails(ApplyGatewayDetails applyGatewayDetails, string companyNumber)
        {
            ApiResponse<Types.CompaniesHouse.Company> companyDetails = null;
            try
            { 
                companyDetails =
                    await _retryPolicy.ExecuteAsync(context => _companiesHouseApiClient.GetCompany(companyNumber),
                        new Context());

            }
            catch(Exception ex)
            {
                var message = $"Unable to retrieve results from companies house for company number {companyNumber}";
                _logger.LogError(message, ex);
                throw new ServiceUnavailableException(message);
            }

            if (string.IsNullOrEmpty(companyDetails?.Response?.CompanyName))
            {
                var message = $"Unable to retrieve Companies House details for company number {companyNumber}";
                _logger.LogError(message);
                throw new ServiceUnavailableException(message);
            }
            applyGatewayDetails.CompaniesHouseDetails = Mapper.Map<CompaniesHouseSummary>(companyDetails.Response);
        }

        private async Task LookupCharityCommissionDetails(ApplyGatewayDetails applyGatewayDetails, string charityNumberFromUkrlp)
        {
            if (!int.TryParse(charityNumberFromUkrlp, out var charityNumber))
            {
                var message =
                    $"Unable to parse charity registration number from charity number in ukrlp: '{charityNumberFromUkrlp}'";
                _logger.LogError(message);
                throw new ServiceUnavailableException(message);
            }

            Charity charityDetails = null;
            try
            {
                charityDetails = await _retryPolicy.ExecuteAsync(context => _outerApiClient.GetCharityDetails(charityNumber), new Context());
                applyGatewayDetails.CharityCommissionDetails = Mapper.Map<CharityCommissionSummary>(charityDetails);
            }
            catch (HttpRequestException ex)
            {
                var message = $"Unable to retrieve Charity Commission details for charity number {charityNumber} based on charity number from uklrp: {charityNumberFromUkrlp}";
                _logger.LogError(ex, message);
                throw new ServiceUnavailableException(message);
            }

            
        }

        private AsyncRetryPolicy GetRetryPolicy()
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(4)
                }, (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning($"Error retrieving response from companies house or charity commission. Reason: {exception.Message}. Retrying in {timeSpan.Seconds} secs...attempt: {retryCount}");
                });
        }
    }
}
