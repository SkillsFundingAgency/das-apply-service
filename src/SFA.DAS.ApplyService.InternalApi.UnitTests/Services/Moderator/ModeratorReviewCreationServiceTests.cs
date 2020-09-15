using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Moderator;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Services.Assessor;
using SFA.DAS.ApplyService.InternalApi.Services.Moderator;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests.Services.Assessor
{
    [TestFixture]
    public class ModeratorReviewCreationServiceTests
    {
        private ModeratorReviewCreationService _reviewCreationService;
        private Mock<IAssessorSequenceService> _assessorSequenceService;
        private Mock<IAssessorSectorService> _assessorSectorService;
        private Mock<IInternalQnaApiClient> _internalQnaApiClient;
        private Mock<IMediator> _mediator;
        private List<AssessorSequence> _sequences;
        private List<AssessorSector> _sectors;
        private ApplicationSection _qnaApplicationSection;

        private Guid _applicationId;
        private string _userId;

        [SetUp]
        public void Arrange()
        {
            _applicationId = Guid.NewGuid();
            _userId = "TestUser";

            _assessorSequenceService = new Mock<IAssessorSequenceService>();
            _internalQnaApiClient = new Mock<IInternalQnaApiClient>();
            _mediator = new Mock<IMediator>();
            _assessorSectorService = new Mock<IAssessorSectorService>();

            _sectors = new List<AssessorSector>
            {
                new AssessorSector{ PageId = Guid.NewGuid().ToString() },
                new AssessorSector{ PageId = Guid.NewGuid().ToString() },
                new AssessorSector{ PageId = Guid.NewGuid().ToString() }
            };

            _assessorSectorService.Setup(x => x.GetSectorsForModerator(_applicationId, _userId)).ReturnsAsync(_sectors);

            _sequences = new List<AssessorSequence>
            {
                new AssessorSequence
                {
                    Id = Guid.NewGuid(),
                    SequenceNumber = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining,
                    SequenceTitle = "Test Sequence",
                    Sections = new List<AssessorSection>
                    {
                        new AssessorSection
                        {
                            Id = Guid.NewGuid(),
                            LinkTitle = "Test Section",
                            SectionNumber = 1
                        },
                        new AssessorSection
                        {
                            Id = Guid.NewGuid(),
                            LinkTitle = "Test Sectors Section",
                            SectionNumber = RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees
                        }
                    }
                }
            };

            _assessorSequenceService.Setup(x => x.GetSequences(_applicationId)).ReturnsAsync(() => _sequences);

            _qnaApplicationSection = new ApplicationSection
            {
                QnAData = new QnAData
                {
                    Pages = new List<Page>
                    {
                        new Page{PageId = Guid.NewGuid().ToString()},
                        new Page{PageId = Guid.NewGuid().ToString()}
                    }
                }
            };

            _internalQnaApiClient.Setup(x =>
                    x.GetSectionBySectionNo(_applicationId, RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining,
                        1))
                .ReturnsAsync(() => _qnaApplicationSection);

            _reviewCreationService = new ModeratorReviewCreationService(_assessorSequenceService.Object,
            _assessorSectorService.Object, _internalQnaApiClient.Object, _mediator.Object);
        }

        [Test]
        public async Task CreateEmptyReview_creates_empty_review_outcomes()
        {
            await _reviewCreationService.CreateEmptyReview(_applicationId, _userId);

            var expectedNumberOfOutcomes = _qnaApplicationSection.QnAData.Pages.Count + _sectors.Count;

            _mediator.Verify(x =>
                    x.Send(It.Is<CreateModeratorPageReviewOutcomesRequest>(r =>
                            r.ModeratorPageReviewOutcomes.Count == expectedNumberOfOutcomes &&
                            r.ModeratorPageReviewOutcomes.TrueForAll(y =>
                                _sectors.Exists(s => s.PageId == y.PageId) ||
                                _qnaApplicationSection.QnAData.Pages.Exists(p => p.PageId == y.PageId))),
                        It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
