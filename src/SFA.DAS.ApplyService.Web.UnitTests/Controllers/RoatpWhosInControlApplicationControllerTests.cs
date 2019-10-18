using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Web.Controllers.Roatp;
using SFA.DAS.ApplyService.Web.Infrastructure;
using System;

namespace SFA.DAS.ApplyService.Web.UnitTests.Controllers
{
    [TestFixture]
    public class RoatpWhosInControlApplicationControllerTests
    {
        private Mock<IQnaApiClient> _client;
        private RoatpWhosInControlApplicationController _controller;

        [SetUp]
        public void Before_each_test()
        {
            _client = new Mock<IQnaApiClient>();
            _controller = new RoatpWhosInControlApplicationController(_client.Object);
        }

        [Test]
        public void Start_page_routes_to_confirm_directors_pscs_if_provider_verified_with_companies_house()
        {
            var verifiedCompaniesHouseAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCompany,
                Value = "TRUE"
            };

            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCompany)).ReturnsAsync(verifiedCompaniesHouseAnswer);
            
            var directorsDataAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHouseDirectors,
                Value = "{}"
            };
            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHouseDirectors)).ReturnsAsync(directorsDataAnswer);

            var pscsDataAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHousePSCs,
                Value = "{}"
            };
            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHousePscs)).ReturnsAsync(pscsDataAnswer);

            var verifiedCharityCommissionAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity,
                Value = ""
            };
            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCharity)).ReturnsAsync(verifiedCharityCommissionAnswer);
            
            var result = _controller.StartPage(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ConfirmDirectorsPscs");
        }

        [Test]
        public void Start_page_routes_to_confirm_directors_pscs_if_provider_verified_with_companies_house_and_charity_commission()
        {
            var verifiedCompaniesHouseAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCompany,
                Value = "TRUE"
            };

            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCompany)).ReturnsAsync(verifiedCompaniesHouseAnswer);

            var directorsDataAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHouseDirectors,
                Value = "{}"
            };
            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHouseDirectors)).ReturnsAsync(directorsDataAnswer);

            var pscsDataAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHousePSCs,
                Value = "{}"
            };
            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHousePscs)).ReturnsAsync(pscsDataAnswer);

            var verifiedCharityCommissionAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity,
                Value = "TRUE"
            };

            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCharity)).ReturnsAsync(verifiedCharityCommissionAnswer);

            var result = _controller.StartPage(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ConfirmDirectorsPscs");
        }

        [Test]
        public void Start_page_routes_to_confirm_trustees_if_provider_verified_with_charity_commission()
        {
            var verifiedCompaniesHouseAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCompany,
                Value = ""
            };

            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCompany)).ReturnsAsync(verifiedCompaniesHouseAnswer);

            var verifiedCharityCommissionAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity,
                Value = "TRUE"
            };
            
            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCharity)).ReturnsAsync(verifiedCharityCommissionAnswer);
            
            var trusteesDataAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CharityCommissionTrustees,
                Value = "{}"
            };
            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CharityCommissionTrustees)).ReturnsAsync(trusteesDataAnswer);

            var result = _controller.StartPage(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ConfirmTrusteesNoDob");
        }

        [Test]
        public void Start_page_routes_to_add_people_in_control_if_not_verified_with_companies_house_or_charity_commission()
        {
            var verifiedCompaniesHouseAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCompany,
                Value = ""
            };

            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCompany)).ReturnsAsync(verifiedCompaniesHouseAnswer);

            var verifiedCharityCommissionAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity,
                Value = ""
            };

            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCharity)).ReturnsAsync(verifiedCharityCommissionAnswer);

            var result = _controller.StartPage(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("AddPeopleInControl");
        }
    }
}
