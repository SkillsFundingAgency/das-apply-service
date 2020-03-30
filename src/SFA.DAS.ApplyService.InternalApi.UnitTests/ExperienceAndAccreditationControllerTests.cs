using NUnit.Framework;
using Moq;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using System;
using SFA.DAS.ApplyService.Application.Apply.Roatp;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
   public class ExperienceAndAccreditationControllerTests
   {
        private Mock<IInternalQnaApiClient> _qnaApiClient;
        
        private ExperienceAndAccreditationController _controller;

        private const string ValueOfQuestion = "swordfish";
        private readonly Guid _applicationId = new Guid("742d2fc4-69bd-47b6-b93f-c059d59db0c0");
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
                .Setup(x => x.GetAnswerValue(_applicationId, 
                    RoatpWorkflowSequenceIds.YourOrganisation, 
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations, 
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.OfficeForStudents,
                    RoatpYourOrganisationQuestionIdConstants.OfficeForStudents)).ReturnsAsync(ValueOfQuestion);
            var actualResult = _controller.GetOfficeForStudents(_applicationId).Result;

            Assert.AreEqual(ValueOfQuestion,actualResult);
            _qnaApiClient.Verify(x=>x.GetAnswerValue(_applicationId, 
                RoatpWorkflowSequenceIds.YourOrganisation, 
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations, 
                RoatpWorkflowPageIds.ExperienceAndAccreditations.OfficeForStudents,
                RoatpYourOrganisationQuestionIdConstants.OfficeForStudents),
                Times.Once);
        }

        [Test]
        public void get_office_for_students_returns_no_value_when_not_present()
        {
            _qnaApiClient
                .Setup(x => x.GetAnswerValue(_applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation, 
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations, 
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.OfficeForStudents,
                    RoatpYourOrganisationQuestionIdConstants.OfficeForStudents)).ReturnsAsync((string)null);

            var actualResult = _controller.GetOfficeForStudents(_applicationId).Result;

            Assert.AreEqual(null, actualResult);
            _qnaApiClient.Verify(x => x.GetAnswerValue(_applicationId, 
                RoatpWorkflowSequenceIds.YourOrganisation, 
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations, 
                RoatpWorkflowPageIds.ExperienceAndAccreditations.OfficeForStudents,
                RoatpYourOrganisationQuestionIdConstants.OfficeForStudents),
                Times.Once);
        }


        [Test]
        public void get_initial_teacher_training_returns_expected_value_when_present()
        {
            _qnaApiClient
                .Setup(x => x.GetAnswerValue(_applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.InitialTeacherTraining,
                    RoatpYourOrganisationQuestionIdConstants.InitialTeacherTraining)).ReturnsAsync("Yes");

            _qnaApiClient
                .Setup(x => x.GetAnswerValue(_applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.IsPostGradTrainingOnlyApprenticeship,
                    RoatpYourOrganisationQuestionIdConstants.IsPostGradTrainingOnlyApprenticeship)).ReturnsAsync("No");


            var actualResult = _controller.GetInitialTeacherTraining(_applicationId).Result;

            Assert.IsTrue(actualResult.DoesOrganisationOfferInitialTeacherTraining);
            Assert.IsFalse(actualResult.IsPostGradOnlyApprenticeship);
        }
   }
}
