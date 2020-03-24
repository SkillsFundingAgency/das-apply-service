using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Ukrlp;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
    public class RoatpGatewayOrganisationChecksControllerTests
    {
        private Mock<IInternalQnaApiClient> _qnaApiClient;
        private Mock<ILogger<RoatpGatewayOrganisationChecksController>> _logger;
        private RoatpGatewayOrganisationChecksController _controller;

        [SetUp]
        public void Setup()
        {
            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _logger = new Mock<ILogger<RoatpGatewayOrganisationChecksController>>();
            _controller = new RoatpGatewayOrganisationChecksController(_qnaApiClient.Object, _logger.Object);
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

            var returnedPage = new Page { PageOfAnswers = new List<PageOfAnswers> { new PageOfAnswers { Answers = new List<Answer> { 
                                        new Answer { QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine1, Value = "First Address"},
                                        new Answer { QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine2, Value = "Second Address" },
                                        new Answer { QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine3, Value = "Third Address" },
                                        new Answer { QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine4, Value = "Fourt Address" },
                                        new Answer { QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalAddressTown, Value = "Coventry" },
                                        new Answer { QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalAddressPostcode, Value = "CV1 2WT" }
            } } } };

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
        public void OrganisationChecks_answer_is_returned()
        {
            var applicationId = Guid.NewGuid();

            var expectedAnswer = new Answer
            {
                QuestionId = RoatpYourOrganisationQuestionIdConstants.IcoNumber,
                Value = "TIG8ZZTQ"
            };

            var returnedPage = new Page
            {
                PageOfAnswers = new List<PageOfAnswers> 
                                    { new PageOfAnswers 
                                            { Answers = new List<Answer> 
                                                            {
                                                                new Answer 
                                                                    { 
                                                                        QuestionId = RoatpYourOrganisationQuestionIdConstants.IcoNumber, 
                                                                        Value = "TIG8ZZTQ"
                                                                    }
                                    }       }               }
            };

            _qnaApiClient.Setup(x => x.GetPageBySectionNo(applicationId, 
                                                            RoatpWorkflowSequenceIds.YourOrganisation, 
                                                            RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails, 
                                                            RoatpWorkflowPageIds.YourOrganisationIcoNumber)).ReturnsAsync(returnedPage);

            var responseGetIcoNumber = _controller.GetIcoNumber(applicationId).GetAwaiter().GetResult().Result;
            var response = responseGetIcoNumber as OkObjectResult;
            var responseIcoNumberAnswer = response.Value as Answer;

            Assert.AreEqual(expectedAnswer.Value, responseIcoNumberAnswer.Value);
        }
    }
}
