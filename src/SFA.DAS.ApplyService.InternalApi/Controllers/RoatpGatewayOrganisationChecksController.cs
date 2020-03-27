using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Ukrlp;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class RoatpGatewayOrganisationChecksController : Controller
    {
        private readonly IInternalQnaApiClient _qnaApiClient;
        private readonly ILogger<RoatpGatewayOrganisationChecksController> _logger;

        public RoatpGatewayOrganisationChecksController(IInternalQnaApiClient qnaApiClient, ILogger<RoatpGatewayOrganisationChecksController> logger)
        {
            _qnaApiClient = qnaApiClient;
            _logger = logger;
        }


        [HttpGet("Gateway/{applicationId}/OrganisationAddress")]
        public async Task<ActionResult<ContactAddress>> GetOrganisationAddress(Guid applicationId)
        {
            _logger.LogInformation($"Getting Company Address from QnA API for application '{applicationId}'");
            var PreamblePage = await _qnaApiClient.GetPageBySectionNo(applicationId, 0, 1, RoatpWorkflowPageIds.Preamble);

            return Ok(new ContactAddress
            {
                Address1 = PreamblePage?.PageOfAnswers.SelectMany(a => a.Answers).Where(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine1).FirstOrDefault().Value,
                Address2 = PreamblePage?.PageOfAnswers.SelectMany(a => a.Answers).Where(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine2).FirstOrDefault().Value,
                Address3 = PreamblePage?.PageOfAnswers.SelectMany(a => a.Answers).Where(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine3).FirstOrDefault().Value,
                Address4 = PreamblePage?.PageOfAnswers.SelectMany(a => a.Answers).Where(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine4).FirstOrDefault().Value,
                Town = PreamblePage?.PageOfAnswers.SelectMany(a => a.Answers).Where(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressTown).FirstOrDefault().Value,
                PostCode = PreamblePage?.PageOfAnswers.SelectMany(a => a.Answers).Where(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressPostcode).FirstOrDefault().Value
            });
        }

        [HttpGet("Gateway/{applicationId}/IcoNumber")]
        public async Task<ActionResult<string>> GetIcoNumber(Guid applicationId)
        {
            _logger.LogInformation($"RoatpGatewayOrganisationChecksController-GetIcoNumber - applicationId - '{applicationId}'");
            var page = await _qnaApiClient.GetPageBySectionNo(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails, RoatpWorkflowPageIds.YourOrganisationIcoNumber);
            return Ok(page?.PageOfAnswers.SelectMany(a => a.Answers).Where(a => a.QuestionId == RoatpYourOrganisationQuestionIdConstants.IcoNumber).FirstOrDefault().Value);
        }

        [HttpGet("/Gateway/{applicationId}/OrganisationType")]
        public async Task<ActionResult<string>> GetOrganisationType(Guid applicationId)
        {
            _logger.LogInformation($"RoatpGatewayOrganisationChecksController-GetOrganisationType - applicationId - '{applicationId}'");

            var ukrlpVerificationCompany = await _qnaApiClient.GetQuestionTag(applicationId, RoatpWorkflowQuestionTags.UkrlpVerificationCompany);
            var ukrlpVerificationCharity = await _qnaApiClient.GetQuestionTag(applicationId, RoatpWorkflowQuestionTags.UkrlpVerificationCharity);

            // 1.) Company - if tag UKRLPVerificationCompany is "TRUE", UKRLPVerificationCharity is not "TRUE"
            if(ukrlpVerificationCompany.Equals(GatewayOrganisationTypesResponses.TRUE, StringComparison.InvariantCultureIgnoreCase) && !ukrlpVerificationCharity.Equals(GatewayOrganisationTypesResponses.TRUE, StringComparison.InvariantCultureIgnoreCase))
            {
                return Ok(GatewayOrganisationTypes.Company);
            }
            // 2.) Company and charity - if tag UKRLPVerificationCompany is "TRUE", UKRLPVerificationCharity is "TRUE"
            if (ukrlpVerificationCompany.Equals(GatewayOrganisationTypesResponses.TRUE, StringComparison.InvariantCultureIgnoreCase) && ukrlpVerificationCharity.Equals(GatewayOrganisationTypesResponses.TRUE, StringComparison.InvariantCultureIgnoreCase))
            {
                return Ok(GatewayOrganisationTypes.CompanyAndCharity);
            }
            // 3.) Charity - if tag UKRLPVerificationCompany is not "TRUE" and UKRLPVerificationCharity": "TRUE" 
            if (!ukrlpVerificationCompany.Equals(GatewayOrganisationTypesResponses.TRUE, StringComparison.InvariantCultureIgnoreCase) && ukrlpVerificationCharity.Equals(GatewayOrganisationTypesResponses.TRUE, StringComparison.InvariantCultureIgnoreCase))
            {
                return Ok(GatewayOrganisationTypes.Charity);
            }

            // 4.) Sole Trader or Partnership - if SoleTradeOrPartnership is ('Sole Trader' or 'Partnership')
            var soleTraderOrPartnership = await _qnaApiClient.GetQuestionTag(applicationId, RoatpWorkflowQuestionTags.SoleTraderOrPartnership);
            if (soleTraderOrPartnership.Equals(GatewayOrganisationTypes.SoleTrader, StringComparison.InvariantCultureIgnoreCase))
            {
                return Ok(GatewayOrganisationTypes.SoleTrader);
            }
            if (soleTraderOrPartnership.Equals(GatewayOrganisationTypes.Partnership, StringComparison.InvariantCultureIgnoreCase))
            {
                return Ok(GatewayOrganisationTypes.Partnership);
            }

            // 5.) Statutory institute - if "UKRLPPrimaryVerificationSource" is "Government Statute"
            var ukrlpPrimaryVerificationSource = await _qnaApiClient.GetQuestionTag(applicationId, RoatpWorkflowQuestionTags.UKRLPPrimaryVerificationSource);
            if (ukrlpPrimaryVerificationSource.Equals(GatewayOrganisationTypesResponses.GovernmentStatute, StringComparison.InvariantCultureIgnoreCase))
            {
                return Ok(GatewayOrganisationTypes.StatutoryInstitute);
            }
        
            return Ok(string.Empty);
        }
    }

    public class GatewayOrganisationTypes
    {
        public static string Company = "Company";
        public static string CompanyAndCharity = "Company and charity";
        public static string Charity = "Charity";
        public static string SoleTrader = "Sole Trader";
        public static string Partnership = "Partnership";
        public static string StatutoryInstitute = "Statutory institute";
    }
    public class GatewayOrganisationTypesResponses
    {
        public static string TRUE = "TRUE";
        public static string GovernmentStatute = "Government Statute";
    }
}