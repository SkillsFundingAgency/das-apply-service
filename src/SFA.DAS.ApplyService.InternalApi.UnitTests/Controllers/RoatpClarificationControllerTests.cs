using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Clarification;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply.Clarification;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Services.Assessor;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests.Controllers
{
    [TestFixture]
    public class RoatpClarificationControllerTests
    {
        private readonly Guid _applicationId = Guid.NewGuid();
        private const int _sequenceNumber = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining;
        private const int _sectionNumber = RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.ManagementHierarchy;
        private const string _pageId = RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ManagementHierarchy;
        private const string _userId = "userid";

        private Mock<IMediator> _mediator;
        private Mock<IAssessorSequenceService> _sequenceService;
        private Mock<IAssessorPageService> _pageService;
        private Mock<IAssessorSectorService> _sectorService;
        private Mock<IAssessorSectorDetailsService> _sectorDetailsService;

        private RoatpClarificationController _controller;

        [SetUp]
        public void TestSetup()
        {
            _mediator = new Mock<IMediator>();
            _sequenceService = new Mock<IAssessorSequenceService>();
            _pageService = new Mock<IAssessorPageService>();
            _sectorService = new Mock<IAssessorSectorService>();
            _sectorDetailsService = new Mock<IAssessorSectorDetailsService>();

            _controller = new RoatpClarificationController(_mediator.Object, _sequenceService.Object, _pageService.Object, _sectorService.Object, _sectorDetailsService.Object);
        }

        [Test]
        public async Task GetClarificationOverview_gets_expected_sequences()
        {
            var expectedResult = new List<AssessorSequence>();
            _sequenceService.Setup(x => x.GetSequences(_applicationId)).ReturnsAsync(expectedResult);

            var actualResult = await _controller.GetClarificationOverview(_applicationId);

            _sequenceService.Verify(x => x.GetSequences(_applicationId), Times.Once);
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public async Task GetModerationOutcome_returns_expected_ModerationOutcome()
        {
            var expectedResult = new ModerationOutcome();
            _mediator.Setup(x => x.Send(It.Is<GetModerationOutcomeRequest>(r => r.ApplicationId == _applicationId && r.SequenceNumber == _sequenceNumber && r.SectionNumber == _sectionNumber &&
                   r.PageId == _pageId), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.GetModerationOutcome(_applicationId, _sequenceNumber, _sectionNumber, _pageId);

            _mediator.Verify(x => x.Send(It.Is<GetModerationOutcomeRequest>(r => r.ApplicationId == _applicationId && r.SequenceNumber == _sequenceNumber && r.SectionNumber == _sectionNumber &&
                   r.PageId == _pageId), It.IsAny<CancellationToken>()), Times.Once);
            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async Task SubmitPageReviewOutcome_calls_mediator()
        {
            var request = new RoatpClarificationController.SubmitPageReviewOutcomeCommand { SequenceNumber = _sequenceNumber, SectionNumber = _sectionNumber, PageId = _pageId, UserId = _userId, Status = "Fail", Comment = "Very bad", ClarificationResponse = "Good response" };

            await _controller.SubmitPageReviewOutcome(_applicationId, request);

            _mediator.Verify(x => x.Send(It.Is<SubmitClarificationPageOutcomeRequest>(r => r.ApplicationId == _applicationId && r.SequenceNumber == request.SequenceNumber && r.SectionNumber == request.SectionNumber &&
                   r.PageId == request.PageId && r.UserId == request.UserId && r.Status == request.Status && r.Comment == request.Comment && r.ClarificationResponse == request.ClarificationResponse), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetPageReviewOutcome_returns_expected_PageReviewOutcome()
        {
            var request = new RoatpClarificationController.GetPageReviewOutcomeRequest { SequenceNumber = _sequenceNumber, SectionNumber = _sectionNumber, PageId = _pageId, UserId = _userId };

            var expectedResult = new ClarificationPageReviewOutcome();
            _mediator.Setup(x => x.Send(It.Is<GetClarificationPageReviewOutcomeRequest>(r => r.ApplicationId == _applicationId && r.SequenceNumber == request.SequenceNumber && r.SectionNumber == request.SectionNumber &&
                   r.PageId == request.PageId && r.UserId == request.UserId), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.GetPageReviewOutcome(_applicationId, request);

            _mediator.Verify(x => x.Send(It.Is<GetClarificationPageReviewOutcomeRequest>(r => r.ApplicationId == _applicationId && r.SequenceNumber == request.SequenceNumber && r.SectionNumber == request.SectionNumber &&
                    r.PageId == request.PageId && r.UserId == request.UserId), It.IsAny<CancellationToken>()), Times.Once);
            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async Task GetPageReviewOutcomesForSection_returns_expected_list_of_PageReviewOutcome()
        {
            var request = new RoatpClarificationController.GetPageReviewOutcomesForSectionRequest { SequenceNumber = _sequenceNumber, SectionNumber = _sectionNumber, UserId = _userId };

            var expectedResult = new List<ClarificationPageReviewOutcome>();
            _mediator.Setup(x => x.Send(It.Is<GetClarificationPageReviewOutcomesForSectionRequest>(r => r.ApplicationId == _applicationId && r.SequenceNumber == request.SequenceNumber && r.SectionNumber == request.SectionNumber &&
                   r.UserId == request.UserId), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.GetPageReviewOutcomesForSection(_applicationId, request);

            _mediator.Verify(x => x.Send(It.Is<GetClarificationPageReviewOutcomesForSectionRequest>(r => r.ApplicationId == _applicationId && r.SequenceNumber == request.SequenceNumber && r.SectionNumber == request.SectionNumber &&
                   r.UserId == request.UserId), It.IsAny<CancellationToken>()), Times.Once);
            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async Task GetAllPageReviewOutcomes_returns_expected_list_of_PageReviewOutcome()
        {
            var request = new RoatpClarificationController.GetAllPageReviewOutcomesRequest { UserId = _userId };

            var expectedResult = new List<ClarificationPageReviewOutcome>();
            _mediator.Setup(x => x.Send(It.Is<GetAllClarificationPageReviewOutcomesRequest>(r => r.ApplicationId == _applicationId &&
                        r.UserId == request.UserId), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.GetAllPageReviewOutcomes(_applicationId, request);

            _mediator.Verify(x => x.Send(It.Is<GetAllClarificationPageReviewOutcomesRequest>(r => r.ApplicationId == _applicationId &&
                        r.UserId == request.UserId), It.IsAny<CancellationToken>()), Times.Once);
            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async Task GetFirstClarificationPage_returns_expected_page()
        {
            var expectedResult = new AssessorPage();
            _pageService.Setup(x => x.GetPage(_applicationId, _sequenceNumber, _sectionNumber, null)).ReturnsAsync(expectedResult);

            var actualResult = await _controller.GetFirstClarificationPage(_applicationId, _sequenceNumber, _sectionNumber);

            _pageService.Verify(x => x.GetPage(_applicationId, _sequenceNumber, _sectionNumber, null), Times.Once);
            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async Task GetClarificationPage_returns_expected_page()
        {
            var expectedResult = new AssessorPage();
            _pageService.Setup(x => x.GetPage(_applicationId, _sequenceNumber, _sectionNumber, _pageId)).ReturnsAsync(expectedResult);

            var actualResult = await _controller.GetClarificationPage(_applicationId, _sequenceNumber, _sectionNumber, _pageId);

            _pageService.Verify(x => x.GetPage(_applicationId, _sequenceNumber, _sectionNumber, _pageId), Times.Once);
            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async Task GetSectors_returns_expected_list_of_sectors()
        {
            var request = new RoatpClarificationController.GetSectorsRequest { UserId = _userId };

            var expectedResult = new List<AssessorSector>();
            _sectorService.Setup(x => x.GetSectorsForClarification(_applicationId, _userId)).ReturnsAsync(expectedResult);

            var actualResult = await _controller.GetSectors(_applicationId, request);

            _sectorService.Verify(x => x.GetSectorsForClarification(_applicationId, _userId), Times.Once);
            CollectionAssert.AreEqual(actualResult, expectedResult);
        }
    }
}
