﻿using NUnit.Framework;
using Moq;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Services.Files;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
   public class ExperienceAndAccreditationControllerTests
   {
        private Mock<IInternalQnaApiClient> _qnaApiClient;
        private Mock<IFileStorageService> _fileStorageService;
        private ExperienceAndAccreditationController _controller;

        private const string ValueOfQuestion = "swordfish";
        private readonly Guid _applicationId = new Guid("742d2fc4-69bd-47b6-b93f-c059d59db0c0");
        [SetUp]
        public void Before_each_test()
        {
            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _fileStorageService = new Mock<IFileStorageService>();
            _controller = new ExperienceAndAccreditationController(_qnaApiClient.Object, _fileStorageService.Object);
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
        public void GetInitialTeacherTraining_QnaUnavailable_ThrowsException()
        {
            _controller = new ExperienceAndAccreditationController(null, _fileStorageService.Object);
            Assert.ThrowsAsync<ServiceUnavailableException>(() => _controller.GetInitialTeacherTraining(_applicationId));
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
        public void get_gateway_declaration_contract_file_clarification_returns_the_submitted_file()
        {
            var applicationId = Guid.NewGuid();
            var fileName = "something.pdf";
            var fileStream = new FileStreamResult(new MemoryStream(), "application/pdf");
            fileStream.FileDownloadName = fileName;

            var file = new InternalApi.Services.Files.DownloadFile
            {
                ContentType = "application/pdf", FileName = fileName, Stream = fileStream.FileStream
            };

            _fileStorageService.Setup(x => x.DownloadFile(applicationId, RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                RoatpClarificationUpload.SubcontractorDeclarationClarificationFile, fileName,
                ContainerType.Gateway,It.IsAny<CancellationToken>())).ReturnsAsync(file);

            var result = _controller.GetSubcontractorDeclarationContractFileClarification(applicationId, fileName).Result as FileStreamResult;

            Assert.AreEqual(fileStream.ContentType,result.ContentType);
            Assert.AreEqual(fileStream.FileDownloadName, result.FileDownloadName); 
            Assert.AreEqual(fileStream.FileStream, result.FileStream);
        }

        [Test]
        public void get_ofsted_details_returns_expected_value()
        {
            var expectedOverallGrade = "Average";
            var expectedApprenticeshipGrade = "Good";
            var applicationSection = new ApplicationSection {QnAData = new QnAData()};

            var pages = new List<Page>
            {
                GetConstructedPage(
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.HasHadFullInspection,
                    RoatpYourOrganisationQuestionIdConstants.HasHadFullInspection,
                    "Yes"),
                GetConstructedPage(
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.ReceivedFullInspectionGradeForApprenticeships,
                    RoatpYourOrganisationQuestionIdConstants.ReceivedFullInspectionGradeForApprenticeships,
                    "No"),
                GetConstructedPage(
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.FullInspectionOverallEffectivenessGrade,
                    RoatpYourOrganisationQuestionIdConstants.FullInspectionOverallEffectivenessGrade,
                    expectedOverallGrade),
                GetConstructedPage(
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.HasHadMonitoringVisit,
                    RoatpYourOrganisationQuestionIdConstants.HasHadMonitoringVisit,
                    "Yes"),
                GetConstructedPage(
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.HasMaintainedFundingSinceInspection,
                    RoatpYourOrganisationQuestionIdConstants.HasMaintainedFundingSinceInspection,
                    "No"),
                GetConstructedPage(
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.HasHadShortInspectionWithinLast3Years,
                    RoatpYourOrganisationQuestionIdConstants.HasHadShortInspectionWithinLast3Years,
                    "Yes"),
                GetConstructedPage(
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.HasMaintainedFullGradeInShortInspection,
                    RoatpYourOrganisationQuestionIdConstants.HasMaintainedFullGradeInShortInspection,
                    "No"),
                GetConstructedPage(
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.Has2MonitoringVisitsGradedInadequate,
                    RoatpYourOrganisationQuestionIdConstants.Has2MonitoringVisitsGradedInadequate,
                    "Yes"),
                GetConstructedPage(
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.FullInspectionApprenticeshipGradeNonOfsFunded,
                    RoatpYourOrganisationQuestionIdConstants.FullInspectionApprenticeshipGradeOfsFunded,
                    expectedApprenticeshipGrade),
                GetConstructedPage(
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.GradeWithinLast3YearsOfsFunded,
                    RoatpYourOrganisationQuestionIdConstants.GradeWithinLast3YearsOfsFunded,
                    "Yes"),
                GetConstructedPage(
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.HasMonitoringVisitGradedInadequateInLast18Months,
                    RoatpYourOrganisationQuestionIdConstants.HasMonitoringVisitGradedInadequateInLast18Months,
                    "Yes"),
            };

            applicationSection.QnAData.Pages = pages;
            ;

            _qnaApiClient.Setup(x => x.GetSectionBySectionNo(_applicationId,
                RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations)).ReturnsAsync(applicationSection);
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
            Assert.IsTrue(actualResult.Has2MonitoringVisitsGradedInadequate);
            Assert.IsTrue(actualResult.HasMonitoringVisitGradedInadequateInLast18Months);
        }

        [Test]
        public void get_ofsted_details_does_not_return_questions_that_arent_active()
        {
            _qnaApiClient
                .Setup(x => x.GetSectionBySectionNo(_applicationId, RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations))
                .ReturnsAsync(new ApplicationSection());

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



        private static Page GetConstructedPage(string pageId, string questionId, string value)
        {
            return new Page
            {
                Active = true,
                PageId = pageId,
                PageOfAnswers = new List<PageOfAnswers>
                {
                    new PageOfAnswers
                    {
                        Answers = new List<Answer>
                        {
                            new Answer
                            {
                                QuestionId = questionId,
                                Value = value
                            }
                        }
                    }
                }
            };
        }
    }
}
