using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Assessor;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Services.Assessor;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests.Services.Assessor
{
    [TestFixture]
    public class AssessorReviewCreationServiceTests
    {
        private AssessorReviewCreationService _assessorReviewCreationService;
        private Mock<IAssessorSequenceService> _assessorSequenceService;
        private Mock<IAssessorSectorService> _assessorSectorService;
        private Mock<IInternalQnaApiClient> _internalQnaApiClient;
        private Mock<IMediator> _mediator;
        private List<AssessorSequence> _sequences;
        private List<AssessorSector> _sectors;
        private ApplicationSection _qnaApplicationSection;

        private Guid _applicationId;
        private string _assessorUserId;
        private int _assessorNumber;

        [SetUp]
        public void Arrange()
        {
            _applicationId = Guid.NewGuid();
            _assessorUserId = "TestUser";
            _assessorNumber = 1;

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

            _assessorSectorService.Setup(x => x.GetSectorsForAssessor(_applicationId, _assessorUserId)).ReturnsAsync(_sectors);

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

            _assessorReviewCreationService = new AssessorReviewCreationService(_assessorSequenceService.Object,
            _assessorSectorService.Object, _internalQnaApiClient.Object, _mediator.Object);
        }

        [Test]
        public async Task CreateEmptyReview_creates_empty_review_outcomes()
        {
            await _assessorReviewCreationService.CreateEmptyReview(_applicationId, _assessorUserId, _assessorNumber);

            var expectedNumberOfOutcomes = _qnaApplicationSection.QnAData.Pages.Count + _sectors.Count;

            _mediator.Verify(x =>
                    x.Send(It.Is<CreateEmptyAssessorReviewRequest>(r =>
                            r.PageReviewOutcomes.Count == expectedNumberOfOutcomes &&
                            r.PageReviewOutcomes.TrueForAll(y =>
                                _sectors.Exists(s => s.PageId == y.PageId) ||
                                _qnaApplicationSection.QnAData.Pages.Exists(p => p.PageId == y.PageId))),
                        It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
