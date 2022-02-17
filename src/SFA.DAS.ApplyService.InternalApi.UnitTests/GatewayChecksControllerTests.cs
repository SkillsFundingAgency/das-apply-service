using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Ukrlp;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Models.Roatp;
using SFA.DAS.ApplyService.InternalApi.Services;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    public class GatewayOrganisationChecksControllerTests
    {
        private Mock<IInternalQnaApiClient> _qnaApiClient;
        private Mock<ILogger<GatewayChecksController>> _logger;
        private Mock<ICriminalComplianceChecksQuestionLookupService> _lookupService;
        private GatewayChecksController _controller;

        private const string ValueOfQuestion = "swordfish";
        private readonly Guid _applicationId = new Guid("742d2fc4-69bd-47b6-b93f-c059d59db0c0");

        [SetUp]
        public void Before_each_test()
        {
            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _logger = new Mock<ILogger<GatewayChecksController>>();
            _lookupService = new Mock<ICriminalComplianceChecksQuestionLookupService>(); 
            _controller = new GatewayChecksController(_qnaApiClient.Object, _logger.Object, _lookupService.Object);
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
        public void GetTradingName_QnaUnavailable_ThrowsException()
        {
            _controller = new GatewayChecksController(null, _logger.Object, _lookupService.Object);
            Assert.ThrowsAsync<ServiceUnavailableException>(() => _controller.GetTradingName(_applicationId));
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
        public void GetWebsiteAddress_QnaUnavailable_ThrowsException()
        {
            _controller = new GatewayChecksController(null, _logger.Object, _lookupService.Object);
            Assert.ThrowsAsync<ServiceUnavailableException>(() => _controller.GetOrganisationWebsiteAddress(_applicationId));
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

        [Test]
        public void OrganisationChecks_proper_address_is_returned()
        {
            var applicationId = Guid.NewGuid();
            var preambleSequenceNo = 0;
            var preambleFirstSectionNo = 1;

            var expectedContactAddress = new ContactAddress
            {
                Address1 = "First Address",
                Address2 = "Second Address",
                Address3 = "Third Address",
                Address4 = "Fourt Address",
                Town = "Coventry",
                PostCode = "CV1 2WT"
            };

            var returnedPage = new Page
            {
                PageOfAnswers = new List<PageOfAnswers> { new PageOfAnswers { Answers = new List<Answer> {
                                        new Answer { QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine1, Value = "First Address"},
                                        new Answer { QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine2, Value = "Second Address" },
                                        new Answer { QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine3, Value = "Third Address" },
                                        new Answer { QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine4, Value = "Fourt Address" },
                                        new Answer { QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalAddressTown, Value = "Coventry" },
                                        new Answer { QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalAddressPostcode, Value = "CV1 2WT" }
            } } }
            };

            _qnaApiClient.Setup(x => x.GetPageBySectionNo(applicationId, preambleSequenceNo, preambleFirstSectionNo, RoatpWorkflowPageIds.Preamble)).ReturnsAsync(returnedPage);

            var responseGetOrganisationAddress = _controller.GetOrganisationAddress(applicationId).GetAwaiter().GetResult().Result;
            var response = responseGetOrganisationAddress as OkObjectResult;
            var responseContactAddress = response.Value as ContactAddress;

            Assert.AreEqual(expectedContactAddress.Address1, responseContactAddress.Address1);
            Assert.AreEqual(expectedContactAddress.Address2, responseContactAddress.Address2);
            Assert.AreEqual(expectedContactAddress.Address3, responseContactAddress.Address3);
            Assert.AreEqual(expectedContactAddress.Address4, responseContactAddress.Address4);
            Assert.AreEqual(expectedContactAddress.Town, responseContactAddress.Town);
            Assert.AreEqual(expectedContactAddress.PostCode, responseContactAddress.PostCode);
        }

        [Test]
        public void OrganisationChecks_ico_number_is_returned()
        {
            var applicationId = Guid.NewGuid();
            var expectedIcoNumber = "TIG8ZZTQ";

            var returnedPage = new Page
            {
                PageOfAnswers = new List<PageOfAnswers>
                                    { new PageOfAnswers
                                            { Answers = new List<Answer>
                                                            {
                                                                new Answer
                                                                    {
                                                                        QuestionId = RoatpYourOrganisationQuestionIdConstants.IcoNumber,
                                                                        Value = expectedIcoNumber
                                                                    }
                                    }       }               }
            };

            _qnaApiClient.Setup(x => x.GetPageBySectionNo(applicationId,
                                                            RoatpWorkflowSequenceIds.YourOrganisation,
                                                            RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails,
                                                            RoatpWorkflowPageIds.YourOrganisationIcoNumber)).ReturnsAsync(returnedPage);

            var responseGetIcoNumber = _controller.GetIcoNumber(applicationId).GetAwaiter().GetResult().Result;
            var response = responseGetIcoNumber as JsonResult;
            var responseIcoNumber = response.Value as IcoNumber;

            Assert.AreEqual(expectedIcoNumber, responseIcoNumber.Value);
        }

    }
}
