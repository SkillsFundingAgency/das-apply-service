using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Types;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    public class OrganisationSummaryControllerTypeOfOrganisationTests
    {
        private Mock<IMediator> _mediator;
        private Mock<IInternalQnaApiClient> _qnaApiClient;
        private Mock<ILogger<OrganisationSummaryController>> _logger;

        private OrganisationSummaryController _controller;

        private readonly Guid _applicationId = new Guid("742d2fc4-69bd-47b6-b93f-c059d59db0c0");

        [SetUp]
        public void Before_each_test()
        {
            _mediator = new Mock<IMediator>();
            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _logger = new Mock<ILogger<OrganisationSummaryController>>();
            _controller = new OrganisationSummaryController(_mediator.Object, _qnaApiClient.Object, _logger.Object);
        }

        [Test]
        public async Task get_type_of_organisation_statutory_institute_when_no_tags_set()
        {
            var result = await _controller.GetTypeOfOrganisation(_applicationId) as OkObjectResult;
            Assert.AreEqual(RoatpOrganisationTypes.StatutoryInstitute, result.Value);
        }

        [Test]
        public async Task get_type_of_organisation_company_when_company_tag_set()
        {
            _qnaApiClient.Setup(x => x.GetQuestionTag(_applicationId, RoatpWorkflowQuestionTags.UkrlpVerificationCompany)).ReturnsAsync("TRUE");

            var result = await _controller.GetTypeOfOrganisation(_applicationId) as OkObjectResult;
            Assert.AreEqual(RoatpOrganisationTypes.Company, result.Value);
        }

        [Test]
        public async Task get_type_of_organisation_charity_when_charity_tag_set()
        {
            _qnaApiClient.Setup(x => x.GetQuestionTag(_applicationId, RoatpWorkflowQuestionTags.UkrlpVerificationCharity)).ReturnsAsync("TRUE");

            var result = await _controller.GetTypeOfOrganisation(_applicationId) as OkObjectResult;
            Assert.AreEqual(RoatpOrganisationTypes.Charity, result.Value);
        }

        [Test]
        public async Task get_type_of_organisation_company_and_charity_when_compnay_and_charity_tag_set()
        {
            _qnaApiClient.Setup(x => x.GetQuestionTag(_applicationId, RoatpWorkflowQuestionTags.UkrlpVerificationCharity)).ReturnsAsync("TRUE");
            _qnaApiClient.Setup(x => x.GetQuestionTag(_applicationId, RoatpWorkflowQuestionTags.UkrlpVerificationCompany)).ReturnsAsync("TRUE");

            var result = await _controller.GetTypeOfOrganisation(_applicationId) as OkObjectResult;
            Assert.AreEqual(RoatpOrganisationTypes.CompanyAndCharity, result.Value);
        }

        [TestCase("Sole trader")]
        [TestCase("Partnership")]
        public async Task get_type_of_organisation_sole_trader_when_sole_trader_or_partnership_tag_set(string soleTraderOrPartnershipDescription)
        {
            _qnaApiClient.Setup(x => x.GetQuestionTag(_applicationId, RoatpWorkflowQuestionTags.SoleTraderOrPartnership)).ReturnsAsync(soleTraderOrPartnershipDescription);

            var result = await _controller.GetTypeOfOrganisation(_applicationId) as OkObjectResult;
            Assert.AreEqual(soleTraderOrPartnershipDescription, result.Value);
        }
    }
}
