using AutoMapper;
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
        private readonly IRoatpApiClient _roatpApiClient;
        private readonly IInternalQnaApiClient _qnaApiClient;

        public GatewayApiChecksService(CompaniesHouseApiClient companiesHouseApiClient, CharityCommissionApiClient charityCommissionApiClient,
                                          IRoatpApiClient roatpApiClient, IInternalQnaApiClient qnaApiClient)
        {
            _companiesHouseApiClient = companiesHouseApiClient;
            _charityCommissionClient = charityCommissionApiClient;
            _roatpApiClient = roatpApiClient;
            _qnaApiClient = qnaApiClient;
        }

        public async Task<ApplyGatewayDetails> GetExternalApiCheckDetails(Guid applicationId, string userRequestedChecks)
        {
            var applyGatewayDetails = new ApplyGatewayDetails();

            var ukprn = await _qnaApiClient.GetQuestionTag(applicationId, RoatpWorkflowQuestionTags.UKPRN);

            var ukrlpResults = await _roatpApiClient.GetUkrlpDetails(ukprn);

            var ukrlpDetails = ukrlpResults.Results.FirstOrDefault();
            applyGatewayDetails.UkrlpDetails = Mapper.Map<ProviderDetails>(ukrlpDetails);

            var roatpStatus = await _roatpApiClient.GetOrganisationRegisterStatus(ukprn);

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
            applyGatewayDetails.CompaniesHouseDetails = Mapper.Map<CompaniesHouseSummary>(companyDetails.Response);
        }

        private async Task LookupCharityCommissionDetails(ApplyGatewayDetails applyGatewayDetails, string charityNumberFromUkrlp)
        {
            var charityNumber = 0;
            int.TryParse(charityNumberFromUkrlp, out charityNumber);
            if (charityNumber != 0)
            {
                var charityDetails = await _charityCommissionClient.GetCharity(charityNumber);
                applyGatewayDetails.CharityCommissionDetails = Mapper.Map<CharityCommissionSummary>(charityDetails);
            }
        }
    }
}
