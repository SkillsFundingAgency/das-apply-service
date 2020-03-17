using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using System;
using System.Threading.Tasks;
using AutoMapper;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Ukrlp;
using System.Linq;
using SFA.DAS.ApplyService.Domain.CompaniesHouse;
using SFA.DAS.ApplyService.Domain.CharityCommission;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    public class GatewayApiChecksController : Controller
    {
        private readonly IApplyRepository _applyRepository;
        private readonly ILogger<GatewayApiChecksController> _logger;
        private readonly CompaniesHouseApiClient _companiesHouseApiClient;
        private readonly CharityCommissionApiClient _charityCommissionClient;
        private readonly IRoatpApiClient _roatpApiClient;
        private readonly IInternalQnaApiClient _qnaApiClient;

        public GatewayApiChecksController(IApplyRepository applyRepository, ILogger<GatewayApiChecksController> logger,
                                          CompaniesHouseApiClient companiesHouseApiClient, CharityCommissionApiClient charityCommissionApiClient,
                                          IRoatpApiClient roatpApiClient, IInternalQnaApiClient qnaApiClient)
        {
            _applyRepository = applyRepository;
            _logger = logger;
            _companiesHouseApiClient = companiesHouseApiClient;
            _charityCommissionClient = charityCommissionApiClient;
            _roatpApiClient = roatpApiClient;
            _qnaApiClient = qnaApiClient;
        }

        [HttpGet]
        [Route("Gateway/ApiChecks/{applicationId}/{userRequestedChecks}")]
        public async Task<IActionResult> ExternalApiChecks(Guid applicationId, string userRequestedChecks)
        {
            var applyData = await GetApplyData(applicationId);

            if (applyData.GatewayReviewDetails == null)
            {
                applyData.GatewayReviewDetails = new ApplyGatewayDetails();

                var ukprn = await _qnaApiClient.GetQuestionTag(applicationId, RoatpWorkflowQuestionTags.UKPRN);

                var ukrlpResults = await _roatpApiClient.GetUkrlpDetails(ukprn);

                var ukrlpDetails = ukrlpResults.Results.FirstOrDefault();
                applyData.GatewayReviewDetails.UkrlpDetails = Mapper.Map<ProviderDetails>(ukrlpDetails);

                var roatpStatus = await _roatpApiClient.GetOrganisationRegisterStatus(ukprn);

                applyData.GatewayReviewDetails.RoatpRegisterDetails = roatpStatus;

                var companiesHouseVerification = ukrlpDetails.VerificationDetails
                                                 .FirstOrDefault(x => x.VerificationAuthority == VerificationAuthorities.CompaniesHouseAuthority);
                if (companiesHouseVerification != null)
                {
                    await LookupCompaniesHouseDetails(applyData, companiesHouseVerification.VerificationId);
                }
                var charityCommissionVerification = ukrlpDetails.VerificationDetails
                                                    .FirstOrDefault(x => x.VerificationAuthority == VerificationAuthorities.CharityCommissionAuthority);
                if (charityCommissionVerification != null)
                {
                    await LookupCharityCommissionDetails(applyData, charityCommissionVerification.VerificationId);
                }
                applyData.GatewayReviewDetails.SourcesCheckedOn = DateTime.Now;

                await _applyRepository.UpdateApplyData(applicationId, applyData, userRequestedChecks);
            }
            return Ok(applyData.GatewayReviewDetails);
        }             


        private async Task<ApplyData> GetApplyData(Guid applicationId)
        {
            return await _applyRepository.GetApplyData(applicationId);
        }

        private async Task LookupCompaniesHouseDetails(ApplyData applyData, string companyNumber)
        {
            var companyDetails = await _companiesHouseApiClient.GetCompany(companyNumber);
            applyData.GatewayReviewDetails.CompaniesHouseDetails = Mapper.Map<CompaniesHouseSummary>(companyDetails.Response);
        }

        private async Task LookupCharityCommissionDetails(ApplyData applyData, string charityNumberFromUkrlp)
        {
            var charityNumber = 0;
            int.TryParse(charityNumberFromUkrlp, out charityNumber);
            if (charityNumber != 0)
            {
                var charityDetails = await _charityCommissionClient.GetCharity(charityNumber);
                applyData.GatewayReviewDetails.CharityCommissionDetails = Mapper.Map<CharityCommissionSummary>(charityDetails);
            }
        }
    }
}
