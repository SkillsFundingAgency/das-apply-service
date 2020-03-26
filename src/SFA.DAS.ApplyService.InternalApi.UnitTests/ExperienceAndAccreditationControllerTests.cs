using NUnit.Framework;
using Moq;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
   public class ExperienceAndAccreditationControllerTests
   {
        private Mock<IInternalQnaApiClient> _qnaApiClient;
        
        private ExperienceAndAccreditationController _controller;

        private const string ValueOfQuestion = "swordfish";

        [SetUp]
        public void Before_each_test()
        {
            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _controller = new ExperienceAndAccreditationController(_qnaApiClient.Object);
        }

        [Test]
        public void get_office_for_students_returns_expected_value_when_present()
        {
            _qnaApiClient
                .Setup(x => x.GetAnswerValue(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(),
                    It.IsAny<string>())).ReturnsAsync(ValueOfQuestion);
            var actualResult = _controller.GetOfficeForStudents(new Guid()).Result;

            Assert.AreEqual(ValueOfQuestion,actualResult);
            _qnaApiClient.Verify(x=>x.GetAnswerValue(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()),Times.Once);
        }

        [Test]
        public void get_office_for_students_returns_no_value_when_not_present()
        {
            _qnaApiClient
                .Setup(x => x.GetAnswerValue(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(),
                    It.IsAny<string>())).ReturnsAsync((string)null);

            var actualResult = _controller.GetOfficeForStudents(new Guid()).Result;

            Assert.AreEqual(null, actualResult);
            _qnaApiClient.Verify(x => x.GetAnswerValue(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }


        [Test]
        public void get_initial_teacher_training_returns_expected_value_when_present()
        {
            _qnaApiClient
                .Setup(x => x.GetAnswerValue(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(),
                    It.IsAny<string>())).ReturnsAsync(ValueOfQuestion);


            var actualResult = _controller.GetInitialTeacherTraining(new Guid()).Result;

            Assert.AreEqual(ValueOfQuestion, actualResult);
            _qnaApiClient.Verify(x => x.GetAnswerValue(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void get_initial_teacher_training_returns_no_value_when_not_present()
        {
            _qnaApiClient
                .Setup(x => x.GetAnswerValue(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(),
                    It.IsAny<string>())).ReturnsAsync((string)null);

            var actualResult = _controller.GetInitialTeacherTraining(new Guid()).Result;

            Assert.AreEqual(null, actualResult);
            _qnaApiClient.Verify(x => x.GetAnswerValue(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void get_gateway_declaration_returns_expected_declaration_answers()
        {
            var expectedHasDeliveredTrainingAsSubcontractor = true;
            var expectedContractFileName = "filename";

            _qnaApiClient
                .Setup(x => x.GetAnswerValue(It.IsAny<Guid>(), RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.SubcontractorDeclaration,
                    RoatpYourOrganisationQuestionIdConstants.HasDeliveredTrainingAsSubcontractor))
                .ReturnsAsync("Yes");

            _qnaApiClient
                .Setup(x => x.GetAnswerValue(It.IsAny<Guid>(), RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.SubcontractorContractFile,
                    RoatpYourOrganisationQuestionIdConstants.ContractFileName))
                .ReturnsAsync(expectedContractFileName);

            var actualResult = _controller.GetSubcontractorDeclaration(new Guid()).Result;

            Assert.AreEqual(expectedHasDeliveredTrainingAsSubcontractor, actualResult.HasDeliveredTrainingAsSubcontractor);
            Assert.AreEqual(expectedContractFileName, actualResult.ContractFileName);
        }

        [Test]
        public void get_gateway_declaration_contract_file_returns_the_submitted_file()
        {
            var applicationId = Guid.NewGuid();
            var expectedFileStream = new FileStreamResult(new MemoryStream(), "application/pdf");

            _qnaApiClient.Setup(x => x.GetDownloadFile(applicationId, RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                RoatpWorkflowPageIds.ExperienceAndAccreditations.SubcontractorContractFile,
                RoatpYourOrganisationQuestionIdConstants.ContractFileName)).ReturnsAsync(expectedFileStream);

            var result = _controller.GetSubcontractorDeclarationContractFile(applicationId).Result;

            Assert.AreSame(expectedFileStream, result);
        }
    }
}
