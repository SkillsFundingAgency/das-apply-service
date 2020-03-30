using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private GatewayOrganisationChecksController _controller;

        private const string ValueOfQuestion = "swordfish";
        private readonly Guid _applicationId = new Guid("742d2fc4-69bd-47b6-b93f-c059d59db0c0");

        [SetUp]
        public void Before_each_test()
        {
            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _controller = new GatewayOrganisationChecksController(_qnaApiClient.Object);
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
    }
}
