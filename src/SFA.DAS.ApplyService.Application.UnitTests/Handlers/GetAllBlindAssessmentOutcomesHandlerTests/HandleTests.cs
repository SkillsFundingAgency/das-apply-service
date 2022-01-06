using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Moderator;
using SFA.DAS.ApplyService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.GetAllBlindAssessmentOutcomesHandlerTests
{
    [TestFixture]
    public class HandleTests
    {
        [Test]
        public async Task Handle_ValidRequest_ReturnsAllBlindAssessmentOutcomes()
        {
            //Arrange
            var request = new GetAllBlindAssessmentOutcomesRequest(Guid.NewGuid());
            var repositoryMock = new Mock<IModeratorRepository>();
            var subject = new GetAllBlindAssessmentOutcomesHandler(repositoryMock.Object, Mock.Of<ILogger<GetAllBlindAssessmentOutcomesHandler>>());

            //Act
            var result = await subject.Handle(request, new CancellationToken());

            //Assert
            repositoryMock.Verify(r => r.GetAllBlindAssessmentOutcome(request.ApplicationId), Times.Once);
        }
    }
}
