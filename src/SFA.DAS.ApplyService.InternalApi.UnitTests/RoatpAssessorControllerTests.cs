using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Assessor;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
    public class RoatpAssessorControllerTests
    {
        private readonly Guid _applicationId = Guid.NewGuid();
        private readonly int _sequenceId = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining;
        private readonly int _sectionId = RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.ManagementHierarchy;
        private readonly string _firstPageId = "1";
        private readonly string _lastPageId = "999";

        private Mock<ILogger<RoatpAssessorController>> _logger;
        private Mock<IMediator> _mediator;
        private Mock<IApplyRepository> _applyRepository;
        private Mock<IInternalQnaApiClient> _qnaApiClient;
        private Mock<IAssessorLookupService> _lookupService;
        private RoatpAssessorController _controller;


        [SetUp]
        public void TestSetup()
        {
            _mediator = new Mock<IMediator>();
            _applyRepository = new Mock<IApplyRepository>();     
            _logger = new Mock<ILogger<RoatpAssessorController>>();
            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _lookupService = new Mock<IAssessorLookupService>();

            _controller = new RoatpAssessorController(_logger.Object, _mediator.Object, _applyRepository.Object, _qnaApiClient.Object, _lookupService.Object);
        }

        [Test]
        public async Task Get_summary_returns_summary_for_the_user()
        {
            var expectedUser = "sadjkffgdji";
            var newApplications = 1;
            var inprogressApplications = 2;
            var moderationApplications = 3;
            var clarificationApplications = 4;
            var expectedResult = new RoatpAssessorSummary(newApplications, inprogressApplications, moderationApplications, clarificationApplications);
            _mediator.Setup(x => x.Send(It.Is<AssessorSummaryRequest>(y => y.UserId == expectedUser), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.AssessorSummary(expectedUser);

            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async Task Get_new_applications_returns_new_applications_for_the_user()
        {
            var expectedUser = "sadjkffgdji";
            var expectedResult = new List<RoatpAssessorApplicationSummary>();
            _mediator.Setup(x => x.Send(It.Is<NewAssessorApplicationsRequest>(y => y.UserId == expectedUser), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.NewApplications(expectedUser);

            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async Task Assign_application_sets_assessor_details()
        {
            var request = new AssignAssessorApplicationRequest { AssessorName = "sdfjfsdg", AssessorNumber = 1, AssessorUserId = "dsalkjfhjfdg" };
            var applicationid = Guid.NewGuid();

            await _controller.AssignApplication(applicationid, request);

            _mediator.Verify(x => x.Send(It.Is<AssignAssessorRequest>(r => r.ApplicationId == applicationid && r.AssessorName == request.AssessorName && r.AssessorNumber == request.AssessorNumber && r.AssessorUserId == request.AssessorUserId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Get_in_progress_applications_returns_in_progress_applications_for_the_user()
        {
            var expectedUser = "sadjkffgdji";
            var expectedResult = new List<RoatpAssessorApplicationSummary>();
            _mediator.Setup(x => x.Send(It.Is<InProgressAssessorApplicationsRequest>(y => y.UserId == expectedUser), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.InProgressApplications(expectedUser);

            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async Task GetAssessorOverview_gets_expected_sequences()
        {
            var application = new Apply
            {
                ApplicationId = _applicationId,
                ApplyData = new ApplyData
                {
                    Sequences = new List<ApplySequence>()
                }
            };

            _mediator.Setup(x => x.Send(It.Is<GetApplicationRequest>(y => y.ApplicationId == _applicationId), It.IsAny<CancellationToken>())).ReturnsAsync(application);

            var allSections = new List<ApplicationSection>
            {
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.Preamble, SectionId = 1},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.YourOrganisation, SectionId = RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.YourOrganisation, SectionId = RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.FinancialEvidence, SectionId = RoatpWorkflowSectionIds.FinancialEvidence.WhatYouWillNeed},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.FinancialEvidence, SectionId = RoatpWorkflowSectionIds.FinancialEvidence.YourOrganisationsFinancialEvidence},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.CriminalComplianceChecks, SectionId = RoatpWorkflowSectionIds.CriminalComplianceChecks.WhatYouWillNeed},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.CriminalComplianceChecks, SectionId = RoatpWorkflowSectionIds.CriminalComplianceChecks.ChecksOnYourOrganisation},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.ProtectingYourApprentices, SectionId = 1},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.ProtectingYourApprentices, SectionId = 2},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.ReadinessToEngage, SectionId = 1},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.ReadinessToEngage, SectionId = 2},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.PlanningApprenticeshipTraining, SectionId = 1},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.PlanningApprenticeshipTraining, SectionId = 2},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining, SectionId = 1},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining, SectionId = 2},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.EvaluatingApprenticeshipTraining, SectionId = 1},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.EvaluatingApprenticeshipTraining, SectionId = 2},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.Finish, SectionId = 1}
            };

            _qnaApiClient.Setup(x => x.GetAllApplicationSections(_applicationId)).ReturnsAsync(allSections);

            var expectedSequenceNumbers = new List<int>
                                        { 
                                            RoatpWorkflowSequenceIds.ProtectingYourApprentices,
                                            RoatpWorkflowSequenceIds.ReadinessToEngage,
                                            RoatpWorkflowSequenceIds.PlanningApprenticeshipTraining,
                                            RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining,
                                            RoatpWorkflowSequenceIds.EvaluatingApprenticeshipTraining
                                        };

            var actualSequences = await _controller.GetAssessorOverview(_applicationId);

            Assert.That(actualSequences, Is.Not.Null);
            Assert.That(actualSequences.Select(seq => seq.SequenceNumber), Is.EquivalentTo(expectedSequenceNumbers));
        }

        [Test]
        public async Task SubmitAssessorPageOutcome_calls_mediator()
        {
            var applicationId = Guid.NewGuid();
            var request = new SubmitAssessorPageOutcomeRequest(applicationId, 1, 2, "30", 2, "4fs7f-userId-7gfhh", "Fail", "Very bad");

            await _controller.SubmitAssessorPageOutcome(request);

            _mediator.Verify(x => x.Send(It.Is<SubmitAssessorPageOutcomeRequest>(r => r.ApplicationId == applicationId && r.SequenceNumber == request.SequenceNumber && r.SectionNumber == request.SectionNumber && 
                   r.PageId == request.PageId && r.AssessorType == request.AssessorType && r.UserId == request.UserId && r.Status == request.Status && r.Comment == request.Comment), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetPageReviewOutcome_returns_expected_PageReviewOutcome()
        {
            var expectedApplicationId = Guid.NewGuid();
            var expectedSequenceNumber = 1;
            var expectedSectionNumber = 2;
            var expectedPageId = "30";
            var expectedAssessorType = 2;
            var expectedUserId = "4fs7f-userId-7gfhh";

            var request = new GetPageReviewOutcomeRequest( expectedApplicationId,
                                                            expectedSequenceNumber,
                                                            expectedSectionNumber,
                                                            expectedPageId,
                                                            expectedAssessorType,
                                                            expectedUserId);

            var expectedResult = new PageReviewOutcome();
            _mediator.Setup(x => x.Send(It.Is<GetPageReviewOutcomeRequest>(r => r.ApplicationId == expectedApplicationId && r.SequenceNumber == expectedSequenceNumber &&
                        r.SectionNumber == expectedSectionNumber && r.PageId == expectedPageId && r.AssessorType == expectedAssessorType && 
                        r.UserId == expectedUserId), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.GetPageReviewOutcome(request);

            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async Task GetAssessorReviewOutcomesPerSection_returns_expected_list_of_PageReviewOutcome()
        {
            var expectedApplicationId = Guid.NewGuid();
            var expectedSequenceNumber = 1;
            var expectedSectionNumber = 2;
            var expectedAssessorType = 2;
            var expectedUserId = "4fs7f-userId-7gfhh";

            var request = new GetAssessorReviewOutcomesPerSectionRequest(expectedApplicationId, expectedSequenceNumber, expectedSectionNumber, expectedAssessorType, expectedUserId);

            var expectedResult = new List<PageReviewOutcome>();
            _mediator.Setup(x => x.Send(It.Is<GetAssessorReviewOutcomesPerSectionRequest>(r => r.ApplicationId == expectedApplicationId && r.SequenceNumber == expectedSequenceNumber &&
                        r.SectionNumber == expectedSectionNumber && r.AssessorType == expectedAssessorType &&
                        r.UserId == expectedUserId), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.GetAssessorReviewOutcomesPerSection(request);

            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async Task GetAllAssessorReviewOutcomes_returns_expected_list_of_PageReviewOutcome()
        {
            var expectedApplicationId = Guid.NewGuid();
            var expectedAssessorType = 2;
            var expectedUserId = "4fs7f-userId-7gfhh";

            var request = new GetAllAssessorReviewOutcomesRequest(expectedApplicationId, expectedAssessorType, expectedUserId);

            var expectedResult = new List<PageReviewOutcome>();
            _mediator.Setup(x => x.Send(It.Is<GetAllAssessorReviewOutcomesRequest>(r => r.ApplicationId == expectedApplicationId && 
                        r.AssessorType == expectedAssessorType && r.UserId == expectedUserId), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.GetAllAssessorReviewOutcomes(request);

            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async Task GetFirstAssessorPage_gets_first_page_in_section()
        {
            var section = new ApplicationSection
            {
                ApplicationId = _applicationId,
                SequenceId = _sequenceId,
                SectionId = _sectionId,
                QnAData = new QnAData
                {
                    Pages = new List<Page> { GenerateQnAPage(_firstPageId), GenerateQnAPage(_lastPageId) }
                }
            };

            _qnaApiClient.Setup(x => x.GetSectionBySectionNo(section.ApplicationId, section.SequenceId, section.SectionId)).ReturnsAsync(section);

            _qnaApiClient.Setup(x => x.SkipPageBySectionNo(section.ApplicationId, section.SequenceId, section.SectionId, _firstPageId)).ReturnsAsync(new SkipPageResponse { NextAction = "NextPage", NextActionId = _lastPageId });
            _qnaApiClient.Setup(x => x.SkipPageBySectionNo(section.ApplicationId, section.SequenceId, section.SectionId, _lastPageId)).ReturnsAsync(new SkipPageResponse { NextAction = "ReturnToSection" });

            var actualPage = await _controller.GetFirstAssessorPage(_applicationId, _sequenceId, _sectionId);

            Assert.That(actualPage, Is.Not.Null);
            Assert.That(actualPage.PageId, Is.EqualTo(_firstPageId));
            Assert.That(actualPage.NextPageId, Is.EqualTo(_lastPageId));
        }

        [Test]
        public async Task GetAssessorPage_when_last_page_gets_expected_page()
        {
            var section = new ApplicationSection
            {
                ApplicationId = _applicationId,
                SequenceId = _sequenceId,
                SectionId = _sectionId,
                QnAData = new QnAData
                {
                    Pages = new List<Page> { GenerateQnAPage(_firstPageId), GenerateQnAPage(_lastPageId) }
                }
            };

            _qnaApiClient.Setup(x => x.GetSectionBySectionNo(section.ApplicationId, section.SequenceId, section.SectionId)).ReturnsAsync(section);

            _qnaApiClient.Setup(x => x.SkipPageBySectionNo(section.ApplicationId, section.SequenceId, section.SectionId, _firstPageId)).ReturnsAsync(new SkipPageResponse { NextAction = "NextPage", NextActionId = _lastPageId });
            _qnaApiClient.Setup(x => x.SkipPageBySectionNo(section.ApplicationId, section.SequenceId, section.SectionId, _lastPageId)).ReturnsAsync(new SkipPageResponse { NextAction = "ReturnToSection" });

            var actualPage = await _controller.GetAssessorPage(_applicationId, _sequenceId, _sectionId, _lastPageId);

            Assert.That(actualPage, Is.Not.Null);
            Assert.That(actualPage.PageId, Is.EqualTo(_lastPageId));
            Assert.That(actualPage.NextPageId, Is.Null);
        }

        [Test]
        public async Task GetFirstAssessorPage_returns_null_if_invalid_sequence()
        {
            var invalidSequenceId = int.MinValue;

            var actualPage = await _controller.GetFirstAssessorPage(_applicationId, invalidSequenceId, _sectionId);
            Assert.That(actualPage, Is.Null);
        }

        [Test]
        public async Task GetChosenSectors_for_application()
        {

            var sector1PageId = "Sector1PageId";
            var sector2PageId = "Sector2PageId";
            var sector1Title = "Sector 1 Title";
            var sector2Title = "Sector 2 Title";

            var pageSector1Page1 = new Page
            {
                PageId = sector1PageId,
                DisplayType = SectionDisplayType.PagesWithSections,
                LinkTitle = sector1Title,
                Active = true,
                Complete = true,
                NotRequired = false
            };


            var pageSector1Page2WrongType = new Page
            {
                PageId = "Sector1PageId",
                DisplayType = SectionDisplayType.OtherPagesInPagesWithSections,
                LinkTitle = "Sector 1 page 2 title",
                Active = true,
                Complete = true,
                NotRequired = false
            };

            var pageSector2Page1 = new Page
            {
                PageId = sector2PageId,
                DisplayType = SectionDisplayType.PagesWithSections,
                LinkTitle = sector2Title,
                Active = true,
                Complete = true,
                NotRequired = false
            };

            var pageSector3Page1Inactive = new Page
            {
                PageId = "Sector3PageId",
                DisplayType = SectionDisplayType.PagesWithSections,
                LinkTitle = "Sector 3 title",
                Active = false,
                Complete = true,
                NotRequired = false
            };

            var pageSector4Page1Incomplete = new Page
            {
                PageId = "Sector4PageId",
                DisplayType = SectionDisplayType.PagesWithSections,
                LinkTitle = "Sector 4 title",
                Active = true,
                Complete = false,
                NotRequired = false
            };

            var pageSector5Page1NotRequired = new Page
            {
                PageId = "Sector5PageId",
                DisplayType = SectionDisplayType.PagesWithSections,
                LinkTitle = "Sector 5 title",
                Active = true,
                Complete = true,
                NotRequired = true
            };

            var section = new ApplicationSection
            {
                ApplicationId = _applicationId,
                SequenceId = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining,
                SectionId = RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees,
                QnAData = new QnAData
                {
                    Pages = new List<Page> { pageSector1Page1,
                        pageSector1Page2WrongType,
                        pageSector2Page1,
                        pageSector3Page1Inactive,
                        pageSector4Page1Incomplete,
                        pageSector5Page1NotRequired }
                }
            };

            var sectors = new List<Sector>();
            _qnaApiClient.Setup(x => x.GetSectionBySectionNo(_applicationId, 
                RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining,
                RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees)).ReturnsAsync(section);

            var listOfSectors = await _controller.GetChosenSectors(_applicationId);

            var expectedListOfSectors = new List<Sector>
            {
                new Sector {PageId = sector1PageId, Title = sector1Title},
                new Sector {PageId = sector2PageId, Title = sector2Title}
            };

            Assert.AreEqual(JsonConvert.SerializeObject(expectedListOfSectors), JsonConvert.SerializeObject(listOfSectors));
        }


        [Test]
        public async Task DownloadFile_gets_expected_file()
        {
            var questionId = "1";
            var filename = "file.txt";
            var expectedFileStream = new FileStreamResult(new MemoryStream(), "application/pdf");

            _qnaApiClient.Setup(x => x.DownloadSpecifiedFile(_applicationId, _sequenceId, _sectionId, _firstPageId, questionId, filename)).ReturnsAsync(expectedFileStream);

            var result = await _controller.DownloadFile(_applicationId, _sequenceId, _sectionId, _firstPageId, questionId, filename);

            Assert.AreSame(expectedFileStream, result);
        }

        [Test]
        public async Task UpdateAssessorReviewStatus_calls_mediator()
        {
            var applicationId = Guid.NewGuid();
            var request = new UpdateAssessorReviewStatusRequest(applicationId, 1, "4fs7f-userId-7gfhh", AssessorReviewStatus.Approved);

            await _controller.UpdateAssessorReviewStatus(request);

            _mediator.Verify(x => x.Send(It.Is<UpdateAssessorReviewStatusRequest>(r => r.ApplicationId == applicationId && r.AssessorType == request.AssessorType && r.UserId == request.UserId && r.Status == request.Status), It.IsAny<CancellationToken>()), Times.Once);
        }



        private static Page GenerateQnAPage(string pageId)
        {
            return new Page
            {
                PageId = pageId,
                Questions = new List<Question>
                {
                    new Question
                    {
                        QuestionId = $"Q{pageId}",
                        QuestionBodyText = "QuestionBodyText",
                        Input = new Input { Type = "TextArea" }
                    }
                },
                PageOfAnswers = new List<PageOfAnswers> { new PageOfAnswers { Answers = new List<Answer> { new Answer { QuestionId = $"Q{pageId}", Value = "Value" } } } }
            };
        }
    }
}
