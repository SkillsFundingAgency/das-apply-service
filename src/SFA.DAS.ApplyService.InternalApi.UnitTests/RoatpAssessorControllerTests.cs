using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Assessor;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.InternalApi.Controllers;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
    public class RoatpAssessorControllerTests
    {
        private Mock<IMediator> _mediator;
        private RoatpAssessorController _controller;

        [SetUp]
        public void TestSetup()
        {
            _mediator = new Mock<IMediator>();
            _controller = new RoatpAssessorController(_mediator.Object);
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
        public async Task SubmitAssessorPageOutcome_calls_mediator()
        {
            var applicationId = Guid.NewGuid();
            var request = new SubmitAssessorPageOutcomeRequest { ApplicationId = applicationId, SequenceNumber = 1, SectionNumber = 2, PageId = "30", AssessorType = 2,
                                                                 UserId = "4fs7f-userId-7gfhh", Status = "Fail", Comment = "Very bad" };

            await _controller.SubmitAssessorPageOutcome(request);

            _mediator.Verify(x => x.Send(It.Is<SubmitAssessorPageOutcomeHandlerRequest>(r => r.ApplicationId == applicationId && r.SequenceNumber == request.SequenceNumber && r.SectionNumber == request.SectionNumber && 
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

            var request = new GetPageReviewOutcomeRequest
            { 
                ApplicationId = expectedApplicationId,
                SequenceNumber = expectedSequenceNumber,
                SectionNumber = expectedSectionNumber,
                PageId = expectedPageId,
                AssessorType = expectedAssessorType,
                UserId = expectedUserId
            };

            var expectedResult = new PageReviewOutcome();
            _mediator.Setup(x => x.Send(It.Is<GetPageReviewOutcomeHandlerRequest>(r => r.ApplicationId == expectedApplicationId && r.SequenceNumber == expectedSequenceNumber &&
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

            var request = new GetAssessorReviewOutcomesPerSectionRequest
            {
                ApplicationId = expectedApplicationId,
                SequenceNumber = expectedSequenceNumber,
                SectionNumber = expectedSectionNumber,
                AssessorType = expectedAssessorType,
                UserId = expectedUserId
            };

            var expectedResult = new List<PageReviewOutcome>();
            _mediator.Setup(x => x.Send(It.Is<GetAssessorReviewOutcomesPerSectionHandlerRequest>(r => r.ApplicationId == expectedApplicationId && r.SequenceNumber == expectedSequenceNumber &&
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

            var request = new GetAllAssessorReviewOutcomesRequest
            {
                ApplicationId = expectedApplicationId,
                AssessorType = expectedAssessorType,
                UserId = expectedUserId
            };

            var expectedResult = new List<PageReviewOutcome>();
            _mediator.Setup(x => x.Send(It.Is<GetAllAssessorReviewOutcomesHandlerRequest>(r => r.ApplicationId == expectedApplicationId && 
                        r.AssessorType == expectedAssessorType && r.UserId == expectedUserId), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.GetAllAssessorReviewOutcomes(request);

            Assert.AreSame(expectedResult, actualResult);
        }
    }
}
