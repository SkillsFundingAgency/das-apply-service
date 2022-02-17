using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.Domain.Ukrlp;
using SFA.DAS.ApplyService.InternalApi.Models.Roatp;
using SFA.DAS.ApplyService.InternalApi.Services;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class GatewayChecksController : Controller
    {
        private readonly IInternalQnaApiClient _qnaApiClient;
        private readonly ILogger<GatewayChecksController> _logger;
        private readonly ICriminalComplianceChecksQuestionLookupService _lookupService;

        public GatewayChecksController(IInternalQnaApiClient qnaApiClient,
            ILogger<GatewayChecksController> logger, ICriminalComplianceChecksQuestionLookupService lookupService)
        {
            _qnaApiClient = qnaApiClient;
            _logger = logger;
            _lookupService = lookupService;
        }

        [HttpGet("/Gateway/{applicationId}/TradingName")]
        public async Task<string> GetTradingName(Guid applicationId)
        {
            try {
                return await _qnaApiClient.GetAnswerValue(applicationId,
                    RoatpWorkflowSequenceIds.Preamble,
                    RoatpWorkflowSectionIds.Preamble,
                    RoatpWorkflowPageIds.Preamble,
                    RoatpPreambleQuestionIdConstants.UkrlpTradingName);
                }
            catch (Exception ex)
                {
                    throw new ServiceUnavailableException(
                        $"Qna not available for application {applicationId} for trading name", ex);
                }
        }

        [HttpGet("/Gateway/{applicationId}/WebsiteAddressSourcedFromUkrlp")]
        public async Task<string> GetWebsiteAddressFromUkrlp(Guid applicationId)
        {
            return await _qnaApiClient.GetAnswerValue(applicationId,
                RoatpWorkflowSequenceIds.Preamble,
                RoatpWorkflowSectionIds.Preamble,
                RoatpWorkflowPageIds.Preamble,
                RoatpPreambleQuestionIdConstants.UkrlpWebsite);
        }

        [HttpGet("/Gateway/{applicationId}/WebsiteAddressManuallyEntered")]
        public async Task<string> GetWebsiteAddressManuallyEntered(Guid applicationId)
        {
            return await _qnaApiClient.GetAnswerValue(applicationId,
                RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed,
                RoatpWorkflowPageIds.WebsiteManuallyEntered,
                RoatpYourOrganisationQuestionIdConstants.WebsiteManuallyEntered);
        }

        [HttpGet("/Gateway/{applicationId}/OrganisationWebsiteAddress")]
        public async Task<string> GetOrganisationWebsiteAddress(Guid applicationId)
        {
            var organisationWebsite = string.Empty;
            string ukrlpWebsite;

            try
            {
                ukrlpWebsite = await _qnaApiClient.GetAnswerValue(applicationId,
                    RoatpWorkflowSequenceIds.Preamble,
                    RoatpWorkflowSectionIds.Preamble,
                    RoatpWorkflowPageIds.Preamble,
                    RoatpPreambleQuestionIdConstants.UkrlpWebsite);
            }
            catch (Exception ex)
            {
                throw new ServiceUnavailableException(
                    $"Qna not available for application {applicationId} for OrganisationWebsiteAddress",ex);
            }

            var applyWebsite = string.Empty;

            if (string.IsNullOrEmpty(ukrlpWebsite))
            {
                try
                {
                    applyWebsite = await _qnaApiClient.GetAnswerValue(applicationId,
                        RoatpWorkflowSequenceIds.YourOrganisation,
                        RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails,
                        RoatpWorkflowPageIds.WebsiteManuallyEntered,
                        RoatpYourOrganisationQuestionIdConstants.WebsiteManuallyEntered);
                }
                catch (Exception ex)
                {
                    throw new ServiceUnavailableException(
                        $"Qna not available for application {applicationId} for OrganisationWebsiteAddress manually entered", ex);
                }

                if (!string.IsNullOrEmpty(applyWebsite))
                {
                    organisationWebsite = applyWebsite;
                }
            }
            else
            {
                organisationWebsite = ukrlpWebsite;
            }

            var websiteMessage = string.IsNullOrEmpty(ukrlpWebsite)
                ? string.Empty
                : $"WebsiteAddressSourcedFromUkrlp - '" + ukrlpWebsite + "'";
            websiteMessage += string.IsNullOrEmpty(applyWebsite)
                ? string.Empty
                : $"WebsiteAddressManuallyEntered - '" + applyWebsite + "'";
            websiteMessage += string.IsNullOrEmpty(organisationWebsite) ? "No website found" : string.Empty;

            _logger.LogInformation(
                $"Getting Organisation WebsiteAddress for application '{applicationId}' - Website '{websiteMessage}'");

            return organisationWebsite;
        }


        [HttpGet("Gateway/{applicationId}/OrganisationAddress")]
        public async Task<ActionResult<ContactAddress>> GetOrganisationAddress(Guid applicationId)
        {
            _logger.LogInformation($"Getting Company Address from QnA API for application '{applicationId}'");
            var PreamblePage =
                await _qnaApiClient.GetPageBySectionNo(applicationId, 0, 1, RoatpWorkflowPageIds.Preamble);

            return Ok(new ContactAddress
            {
                Address1 = PreamblePage?.PageOfAnswers.SelectMany(a => a.Answers)
                    .Where(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine1)
                    .FirstOrDefault().Value,
                Address2 = PreamblePage?.PageOfAnswers.SelectMany(a => a.Answers)
                    .Where(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine2)
                    .FirstOrDefault().Value,
                Address3 = PreamblePage?.PageOfAnswers.SelectMany(a => a.Answers)
                    .Where(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine3)
                    .FirstOrDefault().Value,
                Address4 = PreamblePage?.PageOfAnswers.SelectMany(a => a.Answers)
                    .Where(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine4)
                    .FirstOrDefault().Value,
                Town = PreamblePage?.PageOfAnswers.SelectMany(a => a.Answers)
                    .Where(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressTown).FirstOrDefault()
                    .Value,
                PostCode = PreamblePage?.PageOfAnswers.SelectMany(a => a.Answers)
                    .Where(a => a.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressPostcode)
                    .FirstOrDefault().Value
            });
        }

        [HttpGet("Gateway/{applicationId}/IcoNumber")]
        public async Task<ActionResult<string>> GetIcoNumber(Guid applicationId)
        {
            _logger.LogInformation(
                $"GatewayChecksController-GetIcoNumber - applicationId - '{applicationId}'");
            var page = await _qnaApiClient.GetPageBySectionNo(applicationId, RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails,
                RoatpWorkflowPageIds.YourOrganisationIcoNumber);
            var icoNumber = page?.PageOfAnswers.SelectMany(a => a.Answers)
                .Where(a => a.QuestionId == RoatpYourOrganisationQuestionIdConstants.IcoNumber).FirstOrDefault().Value;

            return Json(new IcoNumber {ApplicationId = applicationId, Value = icoNumber});
        }



        [HttpGet("/Gateway/{applicationId}/CriminalCompliance/{gatewayPageId}")]
        public async Task<IActionResult> GetCriminalComplianceQuestionDetails(Guid applicationId, string gatewayPageId)
        {
            var qnaPageDetails = _lookupService.GetQuestionDetailsForGatewayPageId(applicationId, gatewayPageId);

            var qnaPage = await _qnaApiClient.GetPageBySectionNo(applicationId, RoatpWorkflowSequenceIds.CriminalComplianceChecks,
                                                                 qnaPageDetails.SectionId,
                                                                 qnaPageDetails.PageId);

            var complianceChecksQuestion = qnaPage.Questions.FirstOrDefault(x => x.QuestionId == qnaPageDetails.QuestionId);

            var criminalComplianceCheckDetails = new CriminalComplianceCheckDetails { PageId = qnaPage.PageId };

            if (complianceChecksQuestion != null)
            {
                criminalComplianceCheckDetails.QuestionText = complianceChecksQuestion.Label;
                var pageOfAnswers = qnaPage.PageOfAnswers.FirstOrDefault();
                if (pageOfAnswers != null)
                {
                    var answer = pageOfAnswers.Answers.FirstOrDefault(x => x.QuestionId == qnaPageDetails.QuestionId);
                    criminalComplianceCheckDetails.QuestionId = answer.QuestionId;
                    criminalComplianceCheckDetails.Answer = answer.Value;
                    PopulateFurtherQuestionIfAnswerMatches(complianceChecksQuestion, criminalComplianceCheckDetails, pageOfAnswers, answer);
                }
            }

            return Ok(criminalComplianceCheckDetails);
        }

        private static void PopulateFurtherQuestionIfAnswerMatches(Question complianceChecksQuestion,
                                                             CriminalComplianceCheckDetails criminalComplianceCheckDetails,
                                                             PageOfAnswers pageOfAnswers, Answer answer)
        {
            foreach (var option in complianceChecksQuestion.Input.Options)
            {
                if (option.FurtherQuestions != null && option.FurtherQuestions.Any() && option.Value == answer.Value)
                {
                    var furtherInformationAnswer = pageOfAnswers.Answers.FirstOrDefault(x => x.QuestionId == option.FurtherQuestions[0]?.QuestionId);
                    if (furtherInformationAnswer != null)
                    {
                        criminalComplianceCheckDetails.FurtherQuestionId = furtherInformationAnswer.QuestionId;
                        criminalComplianceCheckDetails.FurtherAnswer = furtherInformationAnswer.Value;
                        break;
                    }
                }
            }
        }
    }
}