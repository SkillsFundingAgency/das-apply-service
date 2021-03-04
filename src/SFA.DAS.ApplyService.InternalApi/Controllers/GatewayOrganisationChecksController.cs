using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class GatewayOrganisationChecksController : Controller
    {
        private readonly IInternalQnaApiClient _qnaApiClient;
        private readonly ILogger<GatewayOrganisationChecksController> _logger;

        public GatewayOrganisationChecksController(IInternalQnaApiClient qnaApiClient, ILogger<GatewayOrganisationChecksController> logger)
        {
            _qnaApiClient = qnaApiClient;
            _logger = logger;
        }

        [HttpGet("/Gateway/{applicationId}/OneInTwelveMonths")]
        public async Task<string> GetOneInTwelveMonths(Guid applicationId)
        {
            return await _qnaApiClient.GetAnswerValue(applicationId,
                RoatpWorkflowSequenceIds.Preamble,
                RoatpWorkflowSectionIds.Preamble,
                RoatpWorkflowPageIds.Preamble,
                RoatpPreambleQuestionIdConstants.OneInTwelveMonths);
        }

        [HttpGet("/Gateway/{applicationId}/TradingName")]
        public async Task<string> GetTradingName(Guid applicationId)
        {
            return await _qnaApiClient.GetAnswerValue(applicationId, 
                RoatpWorkflowSequenceIds.Preamble, 
                RoatpWorkflowSectionIds.Preamble, 
                RoatpWorkflowPageIds.Preamble, 
                RoatpPreambleQuestionIdConstants.UkrlpTradingName);
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
            var ukrlpWebsite = await _qnaApiClient.GetAnswerValue(applicationId,
                                                                    RoatpWorkflowSequenceIds.Preamble,
                                                                    RoatpWorkflowSectionIds.Preamble,
                                                                    RoatpWorkflowPageIds.Preamble,
                                                                    RoatpPreambleQuestionIdConstants.UkrlpWebsite);
            var applyWebsite = string.Empty;
            if (string.IsNullOrEmpty(ukrlpWebsite))
            {
                applyWebsite = await _qnaApiClient.GetAnswerValue(applicationId,
                                                                    RoatpWorkflowSequenceIds.YourOrganisation,
                                                                    RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails,
                                                                    RoatpWorkflowPageIds.WebsiteManuallyEntered,
                                                                    RoatpYourOrganisationQuestionIdConstants.WebsiteManuallyEntered);
                if (!string.IsNullOrEmpty(applyWebsite))
                {
                    organisationWebsite = applyWebsite;
                }
            }
            else
            {
                organisationWebsite = ukrlpWebsite;
            }

            var websiteMessage = string.IsNullOrEmpty(ukrlpWebsite) ? string.Empty : $"WebsiteAddressSourcedFromUkrlp - '" + ukrlpWebsite + "'";
            websiteMessage += string.IsNullOrEmpty(applyWebsite) ? string.Empty : $"WebsiteAddressManuallyEntered - '" + applyWebsite + "'";
            websiteMessage += string.IsNullOrEmpty(organisationWebsite) ? "No website found" : string.Empty;

            _logger.LogInformation($"Getting Organisation WebsiteAddress for application '{applicationId}' - Website '{websiteMessage}'");

            return organisationWebsite;
        }
    }
}
