using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Assessor;
using SFA.DAS.ApplyService.Application.Apply.Moderator;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Services.Assessor;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests.Services.Assessor
{
    [TestFixture]
    public class AssessorSectorServiceTests
    {
        private Guid _applicationId;
        private const string _userId = "User";

        private const int SectorsSequenceNumber = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining;
        private const int SectorsSectionNumber = RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees;
        private const string OverallAccountabilityPageId = RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.OverallAccountability;
        private const string ManagementHierarchyPageId = RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ManagementHierarchy;
        private const string PassStatus = "Pass";

        private Mock<IMediator> _mediator;
        private Mock<IInternalQnaApiClient> _qnaApiClient;
        private AssessorSectorService _assessorSectorService;

        [SetUp]
        public void TestSetup()
        {
            _applicationId = Guid.NewGuid();

            _mediator = new Mock<IMediator>();
            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _assessorSectorService = new AssessorSectorService(_mediator.Object, _qnaApiClient.Object);

            var startingSection = GenerateStatingSection(_applicationId, SectorsSequenceNumber, SectorsSectionNumber);
            _qnaApiClient.Setup(x => x.GetSectionBySectionNo(_applicationId, SectorsSequenceNumber, SectorsSectionNumber)).ReturnsAsync(startingSection);

            var assessorPageReviewOutcomes = new List<AssessorPageReviewOutcome>();
            foreach(var page in startingSection.QnAData.Pages)
            {
                var pageReviewOutcome = GenerateAssessorPageReviewOutcome(startingSection.ApplicationId, startingSection.SequenceId, startingSection.SectionId, page.PageId);
                assessorPageReviewOutcomes.Add(pageReviewOutcome);
            }
            _mediator.Setup(x => x.Send(It.IsAny<GetAssessorPageReviewOutcomesForSectionRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(assessorPageReviewOutcomes);

            var moderatorPageReviewOutcomes = new List<ModeratorPageReviewOutcome>();
            foreach (var page in startingSection.QnAData.Pages)
            {
                var pageReviewOutcome = GenerateModeratorPageReviewOutcome(startingSection.ApplicationId, startingSection.SequenceId, startingSection.SectionId, page.PageId);
                moderatorPageReviewOutcomes.Add(pageReviewOutcome);
            }
            _mediator.Setup(x => x.Send(It.IsAny<GetModeratorPageReviewOutcomesForSectionRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(moderatorPageReviewOutcomes);
        }

        [Test]
        public async Task GetSectorsForAssessor_returns_expected_result()
        {
            var result = await _assessorSectorService.GetSectorsForAssessor(_applicationId, _userId);

            CollectionAssert.IsNotEmpty(result);
            Assert.AreEqual(result[0].PageId, OverallAccountabilityPageId);
            Assert.AreEqual(result[0].Status, PassStatus);
            Assert.AreEqual(result[1].PageId, ManagementHierarchyPageId);
            Assert.AreEqual(result[1].Status, PassStatus);
        }

        [Test]
        public async Task GetSectorsForModerator_returns_expected_result()
        {
            var result = await _assessorSectorService.GetSectorsForModerator(_applicationId, _userId);
            CollectionAssert.IsNotEmpty(result);
            Assert.AreEqual(result[0].PageId, OverallAccountabilityPageId);
            Assert.AreEqual(result[0].Status, PassStatus);
            Assert.AreEqual(result[1].PageId, ManagementHierarchyPageId);
            Assert.AreEqual(result[1].Status, PassStatus);
        }

        [Test]
        public void GetSectorsForEmptyReview_when_not_sectors_section_returns_no_sectors()
        {
            var managementHierarchySection = new AssessorSection
            {
                LinkTitle = "Management Hierarchy Section",
                SectionNumber = RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.ManagementHierarchy,
                SequenceNumber = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining,
                Pages = new List<Page>
                            {
                                new Page{ PageId = Guid.NewGuid().ToString(), Active = true, Complete = true  },
                                new Page{ PageId = Guid.NewGuid().ToString(), Active = true, Complete = true  }
                            }
            };

            var result = _assessorSectorService.GetSectorsForEmptyReview(managementHierarchySection);
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void GetSectorsForEmptyReview_when_sectors_section_returns_expected_result()
        {
            var startingPageId = Guid.NewGuid().ToString();

            var sectorsSection = new AssessorSection
            {
                LinkTitle = "Sectors Section",
                SectionNumber = RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees,
                SequenceNumber = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining,
                Pages = new List<Page>
                            {
                                new Page{ PageId = startingPageId, DisplayType = SectionDisplayType.PagesWithSections, LinkTitle = "First Sector Starting Page", Active = true, Complete = true },
                                new Page{ PageId = Guid.NewGuid().ToString(), DisplayType = SectionDisplayType.Questions, LinkTitle = "First Sector Question Page", Active = true, Complete = true }
                            }
            };

            var result = _assessorSectorService.GetSectorsForEmptyReview(sectorsSection);
            CollectionAssert.IsNotEmpty(result);
            Assert.AreEqual(result[0].PageId, startingPageId);
        }

        private static ApplicationSection GenerateStatingSection(Guid applicationId, int sequenceNumber, int sectionNumber)
        {
            var section = GenerateQnASection(applicationId, sequenceNumber, sectionNumber);

            var accountabilityPage = GenerateQnAPage(OverallAccountabilityPageId, SectionDisplayType.PagesWithSections);
            section.QnAData.Pages.Add(accountabilityPage);

            var managementHierarchyPage = GenerateQnAPage(ManagementHierarchyPageId, SectionDisplayType.PagesWithSections);
            section.QnAData.Pages.Add(managementHierarchyPage);

            return section;
        }

        private static AssessorPageReviewOutcome GenerateAssessorPageReviewOutcome(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId)
        {
            return new AssessorPageReviewOutcome
            {
                ApplicationId = applicationId,
                SequenceNumber = sequenceNumber,
                SectionNumber = sectionNumber,
                PageId = pageId,
                Status = PassStatus,
                UserId = _userId,
                AssessorNumber = 1
            };
        }

        private static ModeratorPageReviewOutcome GenerateModeratorPageReviewOutcome(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId)
        {
            return new ModeratorPageReviewOutcome
            {
                ApplicationId = applicationId,
                SequenceNumber = sequenceNumber,
                SectionNumber = sectionNumber,
                PageId = pageId,
                Status = PassStatus,
                UserId = _userId
            };
        }

        private static ApplicationSection GenerateQnASection(Guid applicationId, int sequenceNumber, int sectionNumber)
        {
            return new ApplicationSection
            {
                ApplicationId = applicationId,
                SequenceId = sequenceNumber,
                SectionId = sectionNumber,
                QnAData = new QnAData
                {
                    Pages = new List<Page>()
                }
            };
        }

        private static Page GenerateQnAPage(string pageId, string displayType)
        {
            return new Page
            {
                PageId = pageId,
                DisplayType = displayType,
                Active = true,
                Complete = true,
                NotRequired = false,
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
