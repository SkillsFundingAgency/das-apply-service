using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Clarification;
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
        private const string _userId = "userid";

        private Mock<IMediator> _mediator;
        private Mock<IAssessorSequenceService> _sequenceService;

        private RoatpClarificationController _controller;

        [SetUp]
        public void TestSetup()
        {
            _mediator = new Mock<IMediator>();
            _sequenceService = new Mock<IAssessorSequenceService>();

            _controller = new RoatpClarificationController(_mediator.Object, _sequenceService.Object);
        }

        [Test]
        public async Task GetAssessorOverview_gets_expected_sequences()
        {
            var expectedResult = new List<AssessorSequence>();
            _sequenceService.Setup(x => x.GetSequences(_applicationId)).ReturnsAsync(expectedResult);

            var actualResult = await _controller.GetClarificationOverview(_applicationId);

            _sequenceService.Verify(x => x.GetSequences(_applicationId), Times.Once);
            CollectionAssert.AreEqual(expectedResult, actualResult);
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
    }
}
