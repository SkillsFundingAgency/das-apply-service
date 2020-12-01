using NUnit.Framework;
using Moq;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using System;
using System.Linq;
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


        [Test]
        public void get_initial_teacher_training_does_not_lookup_pgta_answer_if_itt_is_no()
        {
            _qnaApiClient
                .Setup(x => x.GetAnswerValue(_applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.InitialTeacherTraining,
                    RoatpYourOrganisationQuestionIdConstants.InitialTeacherTraining)).ReturnsAsync("No");

            _qnaApiClient
                .Setup(x => x.GetAnswerValue(_applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.IsPostGradTrainingOnlyApprenticeship,
                    RoatpYourOrganisationQuestionIdConstants.IsPostGradTrainingOnlyApprenticeship)).ReturnsAsync("No");


            var actualResult = _controller.GetInitialTeacherTraining(_applicationId).Result;

            Assert.IsFalse(actualResult.DoesOrganisationOfferInitialTeacherTraining);
            Assert.IsNull(actualResult.IsPostGradOnlyApprenticeship);
            _qnaApiClient.Verify(x => x.GetAnswerValue(_applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.IsPostGradTrainingOnlyApprenticeship,
                    RoatpYourOrganisationQuestionIdConstants.IsPostGradTrainingOnlyApprenticeship), Times.Never);
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

        [Test]
        public void get_ofsted_details_returns_expected_value()
        {
            _qnaApiClient
                .Setup(x => x.GetAnswerValueFromActiveQuestion(_applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.HasHadFullInspection,
                    RoatpYourOrganisationQuestionIdConstants.HasHadFullInspection)).ReturnsAsync("Yes");

            _qnaApiClient
                .Setup(x => x.GetAnswerValueFromActiveQuestion(_applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.ReceivedFullInspectionGradeForApprenticeships,
                    RoatpYourOrganisationQuestionIdConstants.ReceivedFullInspectionGradeForApprenticeships)).ReturnsAsync("No");

            var expectedOverallGrade = "Average";
            _qnaApiClient
                .Setup(x => x.GetAnswerValueFromActiveQuestion(_applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.FullInspectionOverallEffectivenessGrade,
                    RoatpYourOrganisationQuestionIdConstants.FullInspectionOverallEffectivenessGrade)).ReturnsAsync(expectedOverallGrade);

            _qnaApiClient
                .Setup(x => x.GetAnswerValueFromActiveQuestion(_applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.HasHadMonitoringVisit,
                    RoatpYourOrganisationQuestionIdConstants.HasHadMonitoringVisit)).ReturnsAsync("Yes");

            _qnaApiClient
                .Setup(x => x.GetAnswerValueFromActiveQuestion(_applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.HasMaintainedFundingSinceInspection,
                    RoatpYourOrganisationQuestionIdConstants.HasMaintainedFundingSinceInspection)).ReturnsAsync("No");

            _qnaApiClient
                .Setup(x => x.GetAnswerValueFromActiveQuestion(_applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.HasHadShortInspectionWithinLast3Years,
                    RoatpYourOrganisationQuestionIdConstants.HasHadShortInspectionWithinLast3Years)).ReturnsAsync("Yes");

            _qnaApiClient
                .Setup(x => x.GetAnswerValueFromActiveQuestion(_applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.HasMaintainedFullGradeInShortInspection,
                    RoatpYourOrganisationQuestionIdConstants.HasMaintainedFullGradeInShortInspection)).ReturnsAsync("No");

            var expectedApprenticeshipGrade = "Good";
            _qnaApiClient
                .Setup(x => x.GetAnswerValueFromActiveQuestion(_applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    It.Is<PageAndQuestion[]>(y =>
                        y.First().PageId == RoatpWorkflowPageIds.ExperienceAndAccreditations.FullInspectionApprenticeshipGradeNonOfsFunded && y.First().QuestionId ==RoatpYourOrganisationQuestionIdConstants.FullInspectionApprenticeshipGradeOfsFunded
                        && y.Last().PageId == RoatpWorkflowPageIds.ExperienceAndAccreditations.FullInspectionApprenticeshipGradeOfsFunded && y.Last().QuestionId == RoatpYourOrganisationQuestionIdConstants.FullInspectionApprenticeshipGradeNonOfsFunded)))
                .ReturnsAsync(expectedApprenticeshipGrade);

            _qnaApiClient
                .Setup(x => x.GetAnswerValueFromActiveQuestion(_applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    It.Is<PageAndQuestion[]>(y =>
                        y.First().PageId == RoatpWorkflowPageIds.ExperienceAndAccreditations.GradeWithinLast3YearsOfsFunded && y.First().QuestionId == RoatpYourOrganisationQuestionIdConstants.GradeWithinLast3YearsOfsFunded
                        && y.Last().PageId == RoatpWorkflowPageIds.ExperienceAndAccreditations.GradeWithinLast3YearsNonOfsFunded && y.Last().QuestionId == RoatpYourOrganisationQuestionIdConstants.GradeWithinLast3YearsNonOfsFunded)))
                .ReturnsAsync("Yes");

            var actualResult = _controller.GetOfstedDetails(_applicationId).Result;

            Assert.IsTrue(actualResult.HasHadFullInspection);
            Assert.IsFalse(actualResult.ReceivedFullInspectionGradeForApprenticeships);
            Assert.AreEqual(expectedOverallGrade, actualResult.FullInspectionOverallEffectivenessGrade);
            Assert.IsTrue(actualResult.HasHadMonitoringVisit);
            Assert.IsFalse(actualResult.HasMaintainedFundingSinceInspection);
            Assert.IsTrue(actualResult.HasHadShortInspectionWithinLast3Years);
            Assert.IsFalse(actualResult.HasMaintainedFullGradeInShortInspection);
            Assert.AreEqual(expectedApprenticeshipGrade, actualResult.FullInspectionApprenticeshipGrade);
            Assert.IsTrue(actualResult.GradeWithinTheLast3Years);
        }

        [Test]
        public void get_ofsted_details_does_not_return_questions_that_arent_active()
        {
            _qnaApiClient
                .Setup(x => x.GetAnswerValueFromActiveQuestion(_applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.HasHadFullInspection,
                    RoatpYourOrganisationQuestionIdConstants.HasHadFullInspection)).ReturnsAsync((string)null);

            _qnaApiClient
                .Setup(x => x.GetAnswerValueFromActiveQuestion(_applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.ReceivedFullInspectionGradeForApprenticeships,
                    RoatpYourOrganisationQuestionIdConstants.ReceivedFullInspectionGradeForApprenticeships)).ReturnsAsync((string)null);

            _qnaApiClient
                .Setup(x => x.GetAnswerValueFromActiveQuestion(_applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.FullInspectionOverallEffectivenessGrade,
                    RoatpYourOrganisationQuestionIdConstants.FullInspectionOverallEffectivenessGrade)).ReturnsAsync((string)null);

            _qnaApiClient
                .Setup(x => x.GetAnswerValueFromActiveQuestion(_applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.HasHadMonitoringVisit,
                    RoatpYourOrganisationQuestionIdConstants.HasHadMonitoringVisit)).ReturnsAsync((string)null);

            _qnaApiClient
                .Setup(x => x.GetAnswerValueFromActiveQuestion(_applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.HasMaintainedFundingSinceInspection,
                    RoatpYourOrganisationQuestionIdConstants.HasMaintainedFundingSinceInspection)).ReturnsAsync((string)null);

            _qnaApiClient
                .Setup(x => x.GetAnswerValueFromActiveQuestion(_applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.HasHadShortInspectionWithinLast3Years,
                    RoatpYourOrganisationQuestionIdConstants.HasHadShortInspectionWithinLast3Years)).ReturnsAsync((string)null);

            _qnaApiClient
                .Setup(x => x.GetAnswerValueFromActiveQuestion(_applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.HasMaintainedFullGradeInShortInspection,
                    RoatpYourOrganisationQuestionIdConstants.HasMaintainedFullGradeInShortInspection)).ReturnsAsync((string)null);

            _qnaApiClient
                .Setup(x => x.GetAnswerValueFromActiveQuestion(_applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    It.Is<PageAndQuestion[]>(y =>
                        y.First().PageId == RoatpWorkflowPageIds.ExperienceAndAccreditations.FullInspectionApprenticeshipGradeNonOfsFunded && y.First().QuestionId == RoatpYourOrganisationQuestionIdConstants.FullInspectionApprenticeshipGradeOfsFunded
                        && y.Last().PageId == RoatpWorkflowPageIds.ExperienceAndAccreditations.FullInspectionApprenticeshipGradeOfsFunded && y.Last().QuestionId == RoatpYourOrganisationQuestionIdConstants.FullInspectionApprenticeshipGradeNonOfsFunded)))
                .ReturnsAsync((string)null);

            _qnaApiClient
                .Setup(x => x.GetAnswerValueFromActiveQuestion(_applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    It.Is<PageAndQuestion[]>(y =>
                        y.First().PageId == RoatpWorkflowPageIds.ExperienceAndAccreditations.GradeWithinLast3YearsOfsFunded && y.First().QuestionId == RoatpYourOrganisationQuestionIdConstants.GradeWithinLast3YearsOfsFunded
                        && y.Last().PageId == RoatpWorkflowPageIds.ExperienceAndAccreditations.GradeWithinLast3YearsNonOfsFunded && y.Last().QuestionId == RoatpYourOrganisationQuestionIdConstants.GradeWithinLast3YearsNonOfsFunded)))
                .ReturnsAsync((string)null);

            var actualResult = _controller.GetOfstedDetails(_applicationId).Result;

            Assert.IsFalse(actualResult.HasHadFullInspection.HasValue);
            Assert.IsFalse(actualResult.ReceivedFullInspectionGradeForApprenticeships.HasValue);
            Assert.IsNull(actualResult.FullInspectionOverallEffectivenessGrade);
            Assert.IsFalse(actualResult.HasHadMonitoringVisit.HasValue);
            Assert.IsFalse(actualResult.HasMaintainedFundingSinceInspection.HasValue);
            Assert.IsFalse(actualResult.HasHadShortInspectionWithinLast3Years.HasValue);
            Assert.IsFalse(actualResult.HasMaintainedFullGradeInShortInspection.HasValue);
            Assert.IsNull(actualResult.FullInspectionApprenticeshipGrade);
            Assert.IsFalse(actualResult.GradeWithinTheLast3Years.HasValue);
        }
    }
}
