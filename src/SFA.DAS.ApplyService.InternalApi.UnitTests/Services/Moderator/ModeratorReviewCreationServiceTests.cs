using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Moderator;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Services.Assessor;
using SFA.DAS.ApplyService.InternalApi.Services.Moderator;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests.Services.Moderator
{
    [TestFixture]
    public class ModeratorReviewCreationServiceTests
    {
        private ModeratorReviewCreationService _reviewCreationService;
        private Mock<IAssessorSequenceService> _assessorSequenceService;
        private Mock<IAssessorSectorService> _assessorSectorService;
        private Mock<IMediator> _mediator;

        private List<AssessorSequence> _sequences;
        private List<AssessorSector> _sectors;

        private Guid _applicationId;
        private string _userId;
        private string _userName;

        private string _firstPageId;
        private string _secondPageId;

        [SetUp]
        public void Arrange()
        {
            _applicationId = Guid.NewGuid();
            _userId = "TestUser";
            _userName = "TestUserName";

            _firstPageId = Guid.NewGuid().ToString();
            _secondPageId = Guid.NewGuid().ToString();

            _assessorSequenceService = new Mock<IAssessorSequenceService>();
            _mediator = new Mock<IMediator>();
            _assessorSectorService = new Mock<IAssessorSectorService>();

            var sectorsSection = new AssessorSection
            {
                LinkTitle = "Sectors Section",
                SectionNumber = RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees,
                SequenceNumber = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining,
                Pages = new List<Page>
                            {
                                new Page{ PageId = _firstPageId, DisplayType = SectionDisplayType.PagesWithSections, LinkTitle = "First Sector Starting Page", Active = true, Complete = true },
                                new Page{ PageId = _secondPageId, DisplayType = SectionDisplayType.Questions, LinkTitle = "First Sector Question Page", Active = true, Complete = true }
                            }
            };

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

            _sequences = new List<AssessorSequence>
            {
                new AssessorSequence
                {
                    Id = Guid.NewGuid(),
                    SequenceNumber = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining,
                    SequenceTitle = "Delivering Apprenticeship Training Sequence",
                    Sections = new List<AssessorSection>
                    {
                        sectorsSection,
                        managementHierarchySection
                    }
                }
            };

            _assessorSequenceService.Setup(x => x.GetSequences(_applicationId)).ReturnsAsync(_sequences);

            _sectors = sectorsSection.Pages.Where(pg => pg.DisplayType == SectionDisplayType.PagesWithSections && pg.Active && pg.Complete)
                                               .Select(pg => new AssessorSector { PageId = pg.PageId, Title = pg.LinkTitle })
                                               .ToList();

            _assessorSectorService.Setup(x => x.GetSectorsForEmptyReview(sectorsSection)).Returns(_sectors);

            _reviewCreationService = new ModeratorReviewCreationService(_assessorSequenceService.Object, _assessorSectorService.Object, _mediator.Object);
        }

        [Test]
        public async Task CreateEmptyReview_creates_expected_empty_review_outcomes()
        {
            _mediator.Setup(x => x.Send(It.IsAny<GetBlindAssessmentOutcomeRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new BlindAssessmentOutcome());

            _mediator.Setup(x => x.Send(It.IsAny<GetAllBlindAssessmentOutcomesRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<BlindAssessmentOutcome>());

            await _reviewCreationService.CreateEmptyReview(_applicationId, _userId, _userName);

            var allSections = _sequences.SelectMany(seq => seq.Sections);
            var allSectionsExcludingSectors = allSections.Where(sec => sec.SequenceNumber != RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining || sec.SectionNumber != RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees);
            var sectionPages = allSectionsExcludingSectors.SelectMany(sec => sec.Pages).ToList();

            var expectedNumberOfOutcomes = sectionPages.Count + _sectors.Count + 1; // Note: Add 1 due to inserted Management Hierarchy Financial page

            _mediator.Verify(x =>
                    x.Send(It.Is<CreateEmptyModeratorReviewRequest>(r =>
                            r.PageReviewOutcomes.Count == expectedNumberOfOutcomes &&
                            r.PageReviewOutcomes.TrueForAll(y =>
                                _sectors.Exists(s => s.PageId == y.PageId) ||
                                sectionPages.Exists(p => p.PageId == y.PageId) ||
                                y.PageId == RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ManagementHierarchy_Financial)),
                        It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task CreateEmptyReview_creates_expected_empty_review_outcomes_when_ManagementHierarchyFinancial_page_not_BlindAssessed()
        {
            _mediator.Setup(x => x.Send(It.Is<GetBlindAssessmentOutcomeRequest>(r => r.ApplicationId == _applicationId
                                                                                && r.SequenceNumber == RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining
                                                                                && r.SectionNumber == RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.ManagementHierarchy
                                                                                && r.PageId == RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ManagementHierarchy_Financial), It.IsAny<CancellationToken>()))
                                                                               .ReturnsAsync(default(BlindAssessmentOutcome));
            _mediator.Setup(x => x.Send(It.IsAny<GetAllBlindAssessmentOutcomesRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<BlindAssessmentOutcome>());

            await _reviewCreationService.CreateEmptyReview(_applicationId, _userId, _userName);

            var allSections = _sequences.SelectMany(seq => seq.Sections);
            var allSectionsExcludingSectors = allSections.Where(sec => sec.SequenceNumber != RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining || sec.SectionNumber != RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees);
            var sectionPages = allSectionsExcludingSectors.SelectMany(sec => sec.Pages).ToList();

            var expectedNumberOfOutcomes = sectionPages.Count + _sectors.Count;

            _mediator.Verify(x =>
                    x.Send(It.Is<CreateEmptyModeratorReviewRequest>(r =>
                            r.PageReviewOutcomes.Count == expectedNumberOfOutcomes &&
                            r.PageReviewOutcomes.TrueForAll(y =>
                                _sectors.Exists(s => s.PageId == y.PageId) ||
                                sectionPages.Exists(p => p.PageId == y.PageId))),
                        It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [TestCase("Pass", null, "Pass", null, "Pass")]
        [TestCase("Pass", "some comments", "Pass", null, null)]
        [TestCase("Pass", null, "Pass", "some comments", null)]
        [TestCase("Fail", null, "Pass", null, null)]
        [TestCase("Pass", null, "Fail", null, null)]
        [TestCase("Fail", null, "Fail", null, null)]
        public async Task CreateEmptyReview_CheckAutoPass_AppliesPassStatusToModerationWhereApplicable(
            string assessor1ReviewStatus, string assessor1ReviewComments,
            string assessor2ReviewStatus, string assessor2ReviewComments, string moderatorReviewStatus)
        {
            var blindAssessmentOutcome = new BlindAssessmentOutcome 
            {
                SequenceNumber = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining,
                SectionNumber = RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees,
                PageId = _firstPageId,
                Assessor1ReviewStatus = assessor1ReviewStatus,
                Assessor1ReviewComment = assessor1ReviewComments,
                Assessor2ReviewStatus = assessor2ReviewStatus,
                Assessor2ReviewComment = assessor2ReviewComments
            };

            _mediator.Setup(x => x.Send(It.IsAny<GetBlindAssessmentOutcomeRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new BlindAssessmentOutcome());

            _mediator.Setup(x => x.Send(It.IsAny<GetAllBlindAssessmentOutcomesRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<BlindAssessmentOutcome>() { blindAssessmentOutcome });

            await _reviewCreationService.CreateEmptyReview(_applicationId, _userId, _userName);

            _mediator.Verify(x =>
                    x.Send(It.Is<CreateEmptyModeratorReviewRequest>(r =>
                            r.PageReviewOutcomes.Any(y =>
                                y.SequenceNumber == RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining 
                                && y.SectionNumber == RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees 
                                && y.PageId == _firstPageId 
                                && y.ModeratorReviewStatus == moderatorReviewStatus)),
                        It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
