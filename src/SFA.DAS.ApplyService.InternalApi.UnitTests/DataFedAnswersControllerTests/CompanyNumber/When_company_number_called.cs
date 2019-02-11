using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Organisations;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Controllers;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests.DataFedAnswersControllerTests.CompanyNumber
{
    [TestFixture]
    public class When_company_number_called
    {
        [Test]
        public void With_non_existent_applicationId_Then_bad_request_returned()
        {
            var organisationRepository = new Mock<IOrganisationRepository>();
            organisationRepository.Setup(r => r.GetOrganisationByApplicationId(It.IsAny<Guid>())).ReturnsAsync(default(Organisation));
            
            var controller = new DataFedAnswersController(organisationRepository.Object);

            var result = controller.CompanyNumber(Guid.NewGuid()).Result;
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public void For_organisation_with_no_company_number_should_return_answer_of_null()
        {
            var organisationRepository = new Mock<IOrganisationRepository>();
            organisationRepository.Setup(r => r.GetOrganisationByApplicationId(It.IsAny<Guid>())).ReturnsAsync(new Organisation{OrganisationDetails = new OrganisationDetails()});
            
            var controller = new DataFedAnswersController(organisationRepository.Object);

            var result = controller.CompanyNumber(Guid.NewGuid()).Result;
            ((DataFedAnswerResult)((OkObjectResult) result.Result).Value).Answer.Should().BeNull();
        }
        
        [Test]
        public void For_organisation_with_a_company_number_should_return_that_company_number()
        {
            var organisationRepository = new Mock<IOrganisationRepository>();
            organisationRepository.Setup(r => r.GetOrganisationByApplicationId(It.IsAny<Guid>()))
                .ReturnsAsync(new Organisation {OrganisationDetails = new OrganisationDetails() {CompanyNumber = "12345"}});
            
            var controller = new DataFedAnswersController(organisationRepository.Object);

            var result = controller.CompanyNumber(Guid.NewGuid()).Result;
            ((DataFedAnswerResult)((OkObjectResult) result.Result).Value).Answer.Should().Be("12345");
        }
    }
}