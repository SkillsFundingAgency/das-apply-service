using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Assessor;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
    public class RoatpModerationControllerTests
    {
        private Mock<ILogger<RoatpAssessorController>> _logger;
        private Mock<IMediator> _mediator;
        private RoatpModerationController _controller;

        [SetUp]
        public void TestSetup()
        {
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<RoatpAssessorController>>();

            _controller = new RoatpModerationController(_logger.Object, _mediator.Object);
        }

        [Test]
        public async Task Get_in_moderation_applications_returns_applications()
        {
            var expectedUser = "sadjkffgdji";
            var expectedResult = new List<RoatpModerationApplicationSummary>();
            _mediator.Setup(x => x.Send(It.IsAny<ApplicationsInModerationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.InModerationApplications();

            Assert.AreSame(expectedResult, actualResult);
        }
    }
}
