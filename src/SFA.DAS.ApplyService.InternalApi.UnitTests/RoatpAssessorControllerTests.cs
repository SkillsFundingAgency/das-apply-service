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
            var expectedResult = new RoatpAssessorSummary(1, 1, 1, 1);
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
    }
}
