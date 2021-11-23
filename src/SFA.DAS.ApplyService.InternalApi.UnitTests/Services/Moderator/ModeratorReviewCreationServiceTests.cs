using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        [SetUp]
        public void Arrange()
        {
            _applicationId = Guid.NewGuid();
            _userId = "TestUser";
            _userName = "TestUserName";

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
                                new Page{ PageId = Guid.NewGuid().ToString(), DisplayType = SectionDisplayType.PagesWithSections, LinkTitle = "First Sector Starting Page", Active = true, Complete = true },
                                new Page{ PageId = Guid.NewGuid().ToString(), DisplayType = SectionDisplayType.Questions, LinkTitle = "First Sector Question Page", Active = true, Complete = true }
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

        [TestCase("Pass", null, "Pass", null, true)]
        [TestCase("Pass", "some comments", "Pass", null, false)]
        [TestCase("Pass", null, "Pass", "some comments", false)]
        [TestCase("Fail", null, "Pass", null, false)]
        [TestCase("Pass", null, "Fail", null, false)]
        [TestCase("Fail", null, "Fail", null, false)]
        public void CreateEmptyReview_CheckAutoPass_AppliesPassStatusToModerationWhereApplicable(
            string assessor1ReviewStatus, string assessor1ReviewComments,
            string assessor2ReviewStatus, string assessor2ReviewComments, bool IsModerationStatusSetToPass)
        {
            _mediator.Setup(x => x.Send(It.IsAny<GetBlindAssessmentOutcomeRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new BlindAssessmentOutcome());

            _mediator.Setup(x => x.Send(It.IsAny<GetAllBlindAssessmentOutcomesRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<BlindAssessmentOutcome>());

        }
    }
}
