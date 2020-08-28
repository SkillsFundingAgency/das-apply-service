using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;
using SFA.DAS.ApplyService.Application.Apply.Moderator;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Services.Assessor;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;
using SFA.DAS.ApplyService.InternalApi.Types.Moderator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests.Controllers
{
    [TestFixture]
    public class RoatpModeratorControllerTests
    {
        private readonly Guid _applicationId = Guid.NewGuid();
        private readonly int _sequenceId = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining;
        private readonly int _sectionId = RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.ManagementHierarchy;
        private readonly string _firstPageId = "1";
        private readonly string _lastPageId = "999";
        private readonly string _userId = "user id";

        private Mock<ILogger<RoatpModeratorController>> _logger;
        private Mock<IMediator> _mediator;
        private Mock<IInternalQnaApiClient> _qnaApiClient;
        private Mock<IAssessorLookupService> _assessorLookupService;
        private Mock<IAssessorPageService> _getAssessorPageService;
        private Mock<IAssessorSectorDetailsService> _sectorDetailsOrchestratorService;

        private RoatpModeratorController _controller;

        [SetUp]
        public void TestSetup()
        {
            _logger = new Mock<ILogger<RoatpModeratorController>>();
            _mediator = new Mock<IMediator>();
            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _assessorLookupService = new Mock<IAssessorLookupService>();
            _sectorDetailsOrchestratorService = new Mock<IAssessorSectorDetailsService>();
            _getAssessorPageService = new Mock<IAssessorPageService>();

            _controller = new RoatpModeratorController(_logger.Object, _mediator.Object, _qnaApiClient.Object, _assessorLookupService.Object, _getAssessorPageService.Object, _sectorDetailsOrchestratorService.Object);
        }

        [Test]
        public async Task GetModeratorOverview_gets_expected_sequences()
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

            var actualSequences = await _controller.GetModeratorOverview(_applicationId);

            Assert.That(actualSequences, Is.Not.Null);
            Assert.That(actualSequences.Select(seq => seq.SequenceNumber), Is.EquivalentTo(expectedSequenceNumbers));
        }

        [Test]
        public async Task GetBlindAssessmentOutcome_returns_expected_BlindAssessmentOutcome()
        {
            var applicationId = Guid.NewGuid();
            var sequenceNumber = 1;
            var sectionNumber = 2;
            var pageId = "30";

            var expectedResult = new BlindAssessmentOutcome();
            _mediator.Setup(x => x.Send(It.Is<GetBlindAssessmentOutcomeRequest>(r => r.ApplicationId == applicationId && r.SequenceNumber == sequenceNumber && r.SectionNumber == sectionNumber &&
                   r.PageId == pageId), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.GetBlindAssessmentOutcome(applicationId, sequenceNumber, sectionNumber, pageId);

            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async Task SubmitPageReviewOutcome_calls_mediator()
        {
            var applicationId = Guid.NewGuid();
            var request = new RoatpModeratorController.SubmitPageReviewOutcomeCommand { SequenceNumber = 1, SectionNumber = 2, PageId = "30", UserId = _userId, Status = "Fail", Comment = "Very bad", ExternalComment = "Not good" };

            await _controller.SubmitPageReviewOutcome(applicationId, request);

            _mediator.Verify(x => x.Send(It.Is<SubmitModeratorPageOutcomeRequest>(r => r.ApplicationId == applicationId && r.SequenceNumber == request.SequenceNumber && r.SectionNumber == request.SectionNumber &&
                   r.PageId == request.PageId && r.UserId == request.UserId && r.Status == request.Status && r.Comment == request.Comment && r.ExternalComment == request.ExternalComment), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetPageReviewOutcome_returns_expected_PageReviewOutcome()
        {
            var applicationId = Guid.NewGuid();
            var request = new RoatpModeratorController.GetPageReviewOutcomeRequest { SequenceNumber = 1, SectionNumber = 2, PageId = "30", UserId = _userId };

            var expectedResult = new ModeratorPageReviewOutcome();
            _mediator.Setup(x => x.Send(It.Is<GetModeratorPageReviewOutcomeRequest>(r => r.ApplicationId == applicationId && r.SequenceNumber == request.SequenceNumber && r.SectionNumber == request.SectionNumber &&
                   r.PageId == request.PageId && r.UserId == request.UserId), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.GetPageReviewOutcome(applicationId, request);

            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async Task GetPageReviewOutcomesForSection_returns_expected_list_of_PageReviewOutcome()
        {
            var applicationId = Guid.NewGuid();
            var request = new RoatpModeratorController.GetPageReviewOutcomesForSectionRequest { SequenceNumber = 1, SectionNumber = 2, UserId = _userId };

            var expectedResult = new List<ModeratorPageReviewOutcome>();
            _mediator.Setup(x => x.Send(It.Is<GetModeratorPageReviewOutcomesForSectionRequest>(r => r.ApplicationId == applicationId && r.SequenceNumber == request.SequenceNumber && r.SectionNumber == request.SectionNumber &&
                   r.UserId == request.UserId), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.GetPageReviewOutcomesForSection(applicationId, request);

            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async Task GetAllPageReviewOutcomes_returns_expected_list_of_PageReviewOutcome()
        {
            var applicationId = Guid.NewGuid();
            var request = new RoatpModeratorController.GetAllPageReviewOutcomesRequest { UserId = _userId };

            var expectedResult = new List<ModeratorPageReviewOutcome>();
            _mediator.Setup(x => x.Send(It.Is<GetAllModeratorPageReviewOutcomesRequest>(r => r.ApplicationId == applicationId &&
                        r.UserId == request.UserId), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.GetAllPageReviewOutcomes(applicationId, request);

            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async Task GetFirstModeratorPage_gets_first_page_in_section()
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
            _getAssessorPageService
                .Setup(x => x.GetPage(section.ApplicationId, section.SequenceId, section.SectionId,
                    null)).ReturnsAsync(new AssessorPage { PageId = _firstPageId, NextPageId = _lastPageId });
            var actualPage = await _controller.GetFirstModeratorPage(_applicationId, _sequenceId, _sectionId);

            Assert.That(actualPage, Is.Not.Null);
            Assert.That(actualPage.PageId, Is.EqualTo(_firstPageId));
            Assert.That(actualPage.NextPageId, Is.EqualTo(_lastPageId));
        }

        [Test]
        public async Task GetModeratorPage_when_last_page_gets_expected_page()
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
            _getAssessorPageService
                .Setup(x => x.GetPage(section.ApplicationId, section.SequenceId, section.SectionId,
                    _lastPageId)).ReturnsAsync(new AssessorPage { PageId = _lastPageId });
            var actualPage = await _controller.GetModeratorPage(_applicationId, _sequenceId, _sectionId, _lastPageId);

            Assert.That(actualPage, Is.Not.Null);
            Assert.That(actualPage.PageId, Is.EqualTo(_lastPageId));
            Assert.That(actualPage.NextPageId, Is.Null);
        }

        [Test]
        public async Task GetFirstAssessorPage_returns_null_if_invalid_sequence()
        {
            var invalidSequenceId = int.MinValue;

            var actualPage = await _controller.GetFirstModeratorPage(_applicationId, invalidSequenceId, _sectionId);
            Assert.That(actualPage, Is.Null);
        }

        [Test]
        public async Task GetSectors_for_application()
        {
            var sector1PageId = "Sector1PageId";
            var sector2PageId = "Sector2PageId";
            var sector1Title = "Sector 1 Title";
            var sector2Title = "Sector 2 Title";
            var sector1Status = "Pass";
            var sector2Status = "Fail";

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

            _qnaApiClient.Setup(x => x.GetSectionBySectionNo(_applicationId,
                RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining,
                RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees)).ReturnsAsync(section);

            var _sectionStatuses = new List<ModeratorPageReviewOutcome>();
            _sectionStatuses.Add(new ModeratorPageReviewOutcome
            {
                ApplicationId = _applicationId,
                PageId = sector1PageId,
                Status = sector1Status
            });
            _sectionStatuses.Add(new ModeratorPageReviewOutcome
            {
                ApplicationId = _applicationId,
                PageId = sector2PageId,
                Status = sector2Status
            });

            var request = new RoatpModeratorController.GetSectorsRequest { UserId = _userId };

            _mediator.Setup(x => x.Send(It.Is<GetModeratorPageReviewOutcomesForSectionRequest>(y => y.ApplicationId == _applicationId && y.SequenceNumber == RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining
            && y.SectionNumber == RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees && y.UserId == request.UserId), It.IsAny<CancellationToken>())).ReturnsAsync(_sectionStatuses);

            var listOfSectors = await _controller.GetSectors(_applicationId, request);

            var expectedListOfSectors = new List<ModeratorSector>
            {
                new ModeratorSector {PageId = sector1PageId, Title = sector1Title, Status = sector1Status},
                new ModeratorSector {PageId = sector2PageId, Title = sector2Title, Status = sector2Status}
            };

            Assert.AreEqual(JsonConvert.SerializeObject(expectedListOfSectors), JsonConvert.SerializeObject(listOfSectors));
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
