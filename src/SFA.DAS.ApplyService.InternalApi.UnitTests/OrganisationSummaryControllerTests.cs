using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    public class OrganisationSummaryControllerTests
    {
        private Mock<IInternalQnaApiClient> _qnaApiClient;
        private OrganisationSummaryController _controller;

        private readonly Guid _applicationId = new Guid("742d2fc4-69bd-47b6-b93f-c059d59db0c0");

        [SetUp]
        public void Before_each_test()
        {
            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _controller = new OrganisationSummaryController(_qnaApiClient.Object);
            _qnaApiClient.Setup(x => x.GetQuestionTag(_applicationId, It.IsAny<string>())).ReturnsAsync((string)null);
        }

        [Test]
        public void get_type_of_organisation_statutory_institute_when_no_tags_set()
        {
            var result = (OkObjectResult)_controller.GetTypeOfOrganisation(_applicationId).Result;
            result.Should().BeOfType<OkObjectResult>();
            Assert.AreEqual("Statutory instrument", ((OkObjectResult)result).Value);
        }

        [Test]
        public void get_type_of_organisation_company_when_company_tag_set()
        {
            _qnaApiClient.Setup(x => x.GetQuestionTag(_applicationId, RoatpWorkflowQuestionTags.UkrlpVerificationCompany)).ReturnsAsync("TRUE");

            var result = (OkObjectResult)_controller.GetTypeOfOrganisation(_applicationId).Result;
            result.Should().BeOfType<OkObjectResult>();
            Assert.AreEqual("Company", ((OkObjectResult)result).Value);
        }

        [Test]
        public void get_type_of_organisation_charity_when_charity_tag_set()
        {
            _qnaApiClient.Setup(x => x.GetQuestionTag(_applicationId, RoatpWorkflowQuestionTags.UkrlpVerificationCharity)).ReturnsAsync("TRUE");

            var result = (OkObjectResult)_controller.GetTypeOfOrganisation(_applicationId).Result;
            result.Should().BeOfType<OkObjectResult>();
            Assert.AreEqual("Charity", ((OkObjectResult)result).Value);
        }

        [Test]
        public void get_type_of_organisation_company_and_charity_when_compnay_and_charity_tag_set()
        {
            _qnaApiClient.Setup(x => x.GetQuestionTag(_applicationId, RoatpWorkflowQuestionTags.UkrlpVerificationCharity)).ReturnsAsync("TRUE");
            _qnaApiClient.Setup(x => x.GetQuestionTag(_applicationId, RoatpWorkflowQuestionTags.UkrlpVerificationCompany)).ReturnsAsync("TRUE");

            var result = (OkObjectResult)_controller.GetTypeOfOrganisation(_applicationId).Result;
            result.Should().BeOfType<OkObjectResult>();
            Assert.AreEqual("Company and charity", ((OkObjectResult)result).Value);
        }


        [TestCase("Sole trader")]
        [TestCase("Partnership")]
        public void get_type_of_organisation_sole_trader_when_sole_trader_or_partnership_tag_set(string soleTraderOrPartnershipDescription)
        {
            _qnaApiClient.Setup(x => x.GetQuestionTag(_applicationId, RoatpWorkflowQuestionTags.SoleTraderOrPartnership)).ReturnsAsync(soleTraderOrPartnershipDescription);

            var result = (OkObjectResult)_controller.GetTypeOfOrganisation(_applicationId).Result;
            result.Should().BeOfType<OkObjectResult>();
            Assert.AreEqual(soleTraderOrPartnershipDescription,((OkObjectResult)result).Value);
        }
    }
}
