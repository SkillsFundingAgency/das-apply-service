using System;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    public class GatewayOrganisationChecksControllerTests
    {
        private Mock<IInternalQnaApiClient> _qnaApiClient;
        private Mock<ILogger<GatewayOrganisationChecksController>> _logger;
        private GatewayOrganisationChecksController _controller;

        private const string ValueOfQuestion = "swordfish";
        private readonly Guid _applicationId = new Guid("742d2fc4-69bd-47b6-b93f-c059d59db0c0");

        [SetUp]
        public void Before_each_test()
        {
            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _logger = new Mock<ILogger<GatewayOrganisationChecksController>>();
            _controller = new GatewayOrganisationChecksController(_qnaApiClient.Object, _logger.Object);
        }


        [Test]
        public void get_trading_name_returns_expected_value_when_present()
        {
            _qnaApiClient
                .Setup(x => x.GetAnswerValue(_applicationId,
                    RoatpWorkflowSequenceIds.Preamble,
                    RoatpWorkflowSectionIds.Preamble,
                    RoatpWorkflowPageIds.Preamble,
                    RoatpPreambleQuestionIdConstants.UkrlpTradingName)).ReturnsAsync(ValueOfQuestion);
            var actualResult = _controller.GetTradingName(_applicationId).Result;

            Assert.AreEqual(ValueOfQuestion, actualResult);
        }


        [Test]
        public void get_trading_name_returns_no_value_when_not_present()
        {
            _qnaApiClient
                .Setup(x => x.GetAnswerValue(_applicationId,
                    RoatpWorkflowSequenceIds.Preamble,
                    RoatpWorkflowSectionIds.Preamble,
                    RoatpWorkflowPageIds.Preamble,
                    RoatpPreambleQuestionIdConstants.UkrlpTradingName)).ReturnsAsync((string)null);

            var actualResult = _controller.GetTradingName(_applicationId).Result;

            Assert.IsNull(actualResult);
        }


        [Test]
        public void get_website_address_from_ukrlp_returns_expected_value_when_present()
        {
            _qnaApiClient
                .Setup(x => x.GetAnswerValue(_applicationId,
                    RoatpWorkflowSequenceIds.Preamble,
                    RoatpWorkflowSectionIds.Preamble,
                    RoatpWorkflowPageIds.Preamble,
                    RoatpPreambleQuestionIdConstants.UkrlpWebsite)).ReturnsAsync(ValueOfQuestion);
            var actualResult = _controller.GetWebsiteAddressFromUkrlp(_applicationId).Result;

            Assert.AreEqual(ValueOfQuestion, actualResult);
        }


        [Test]
        public void get_website_address_from_ukrlp_returns_no_value_when_not_present()
        {
            _qnaApiClient
                .Setup(x => x.GetAnswerValue(_applicationId,
                    RoatpWorkflowSequenceIds.Preamble,
                    RoatpWorkflowSectionIds.Preamble,
                    RoatpWorkflowPageIds.Preamble,
                    RoatpPreambleQuestionIdConstants.UkrlpWebsite)).ReturnsAsync((string)null);

            var actualResult = _controller.GetWebsiteAddressFromUkrlp(_applicationId).Result;

            Assert.IsNull(actualResult);
        }



        [Test]
        public void get_website_address_manually_entered_returns_expected_value_when_present()
        {
            _qnaApiClient
                .Setup(x => x.GetAnswerValue(_applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed,
                    RoatpWorkflowPageIds.WebsiteManuallyEntered,
                    RoatpYourOrganisationQuestionIdConstants.WebsiteManuallyEntered)).ReturnsAsync(ValueOfQuestion);
            var actualResult = _controller.GetWebsiteAddressManuallyEntered(_applicationId).Result;

            Assert.AreEqual(ValueOfQuestion, actualResult);
        }


        [Test]
        public void get_website_address_manually_entered_returns_no_value_when_not_present()
        {
            _qnaApiClient
                .Setup(x => x.GetAnswerValue(_applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed,
                    RoatpWorkflowPageIds.WebsiteManuallyEntered,
                    RoatpYourOrganisationQuestionIdConstants.WebsiteManuallyEntered)).ReturnsAsync((string)null);

            var actualResult = _controller.GetWebsiteAddressManuallyEntered(_applicationId).Result;

            Assert.IsNull(actualResult);
        }

        public void SetupQnAClient(string ukrplWebsite, string applyWebsite)
        {
            _qnaApiClient
               .Setup(x => x.GetAnswerValue(_applicationId,
                                           RoatpWorkflowSequenceIds.Preamble,
                                           RoatpWorkflowSectionIds.Preamble,
                                           RoatpWorkflowPageIds.Preamble,
                                           RoatpPreambleQuestionIdConstants.UkrlpWebsite)).ReturnsAsync(ukrplWebsite);
            _qnaApiClient
               .Setup(x => x.GetAnswerValue(_applicationId,
                                           RoatpWorkflowSequenceIds.YourOrganisation,
                                           RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails,
                                           RoatpWorkflowPageIds.WebsiteManuallyEntered,
                                           RoatpYourOrganisationQuestionIdConstants.WebsiteManuallyEntered)).ReturnsAsync(applyWebsite);
        }

        [Test]
        public void Get_organisation_website_address_both_sources_returning()
        {
            var ukrlpWebsite = "www.UkrlpWebsite.co.uk";
            var applyWebsite = "www.ApplyWebsite.co.uk";
            SetupQnAClient(ukrlpWebsite, applyWebsite);
            var actualResult = _controller.GetOrganisationWebsiteAddress(_applicationId).Result;
            Assert.AreEqual(ukrlpWebsite, actualResult);
        }

        [Test]
        public void Get_organisation_website_from_WebsiteAddressSourcedFromUkrlp()
        {
            var ukrlpWebsite = "www.UkrlpWebsite.co.uk";
            var applyWebsite = string.Empty;
            SetupQnAClient(ukrlpWebsite, applyWebsite);
            var actualResult = _controller.GetOrganisationWebsiteAddress(_applicationId).Result;
            Assert.AreEqual(ukrlpWebsite, actualResult);
        }

        [Test]
        public void Get_organisation_website_address_from_WebsiteAddressManuallyEntered()
        {
            var ukrlpWebsite = string.Empty;
            var applyWebsite = "www.ApplyWebsite.co.uk";
            SetupQnAClient(ukrlpWebsite, applyWebsite);
            var actualResult = _controller.GetOrganisationWebsiteAddress(_applicationId).Result;
            Assert.AreEqual(applyWebsite, actualResult);
        }
        [Test]
        public void Get_organisation_website_address_NoWbsiteReturned()
        {
            var ukrlpWebsite = string.Empty; 
            var applyWebsite = string.Empty; 
            SetupQnAClient(ukrlpWebsite, applyWebsite);
            var actualResult = _controller.GetOrganisationWebsiteAddress(_applicationId).Result;
            Assert.AreEqual(string.Empty, actualResult);
        }

    }
}
