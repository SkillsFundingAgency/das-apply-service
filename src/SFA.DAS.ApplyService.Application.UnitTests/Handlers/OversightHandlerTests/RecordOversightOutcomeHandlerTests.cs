
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Oversight;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.OversightHandlerTests
{
    [TestFixture]
    public class RecordOversightOutcomeHandlerTests
    {
        [TestCase(OversightReviewStatus.Successful, ApplicationReviewStatus.Approved)]
        [TestCase(OversightReviewStatus.Unsuccessful, ApplicationReviewStatus.Declined)]
        public async Task Record_oversight_outcome_updates_oversight_status_and_applies_correct_application_status(string oversightReviewStatus, string applicationStatus)
        {
            var command = new RecordOversightOutcomeCommand
            {
                ApplicationId = Guid.NewGuid(),
                OversightStatus = oversightReviewStatus,
                UserName = "test user",
                UserId = "testUser"
            };

            var repository = new Mock<IApplyRepository>();
            repository.Setup(x => x.UpdateOversightReviewStatus(command.ApplicationId, command.OversightStatus, 
                                                                command.UserId, command.UserName))
                                                                .ReturnsAsync(true);

            repository.Setup(x => x.UpdateApplicationStatus(command.ApplicationId, applicationStatus)).Returns(Task.CompletedTask);

            var logger = new Mock<ILogger<RecordOversightOutcomeHandler>>();
            var handler = new RecordOversightOutcomeHandler(repository.Object, logger.Object);

            var result = await handler.Handle(command, new CancellationToken());

            result.Should().BeTrue();

            repository.Verify(x => x.UpdateOversightReviewStatus(command.ApplicationId, command.OversightStatus,
                                                                command.UserId, command.UserName), Times.Once);
            repository.Verify(x => x.UpdateApplicationStatus(command.ApplicationId, applicationStatus), Times.Once);
        }

        [Test]
        public async Task Application_status_is_not_updated_if_fails_to_update_oversight_status()
        {
            var command = new RecordOversightOutcomeCommand
            {
                ApplicationId = Guid.NewGuid(),
                OversightStatus = OversightReviewStatus.Successful,
                UserName = "test user",
                UserId = "testUser"
            };

            var repository = new Mock<IApplyRepository>();
            repository.Setup(x => x.UpdateOversightReviewStatus(command.ApplicationId, command.OversightStatus,
                                                                command.UserId, command.UserName))
                                                                .ReturnsAsync(false);

            repository.Setup(x => x.UpdateApplicationStatus(command.ApplicationId, ApplicationReviewStatus.Approved)).Returns(Task.CompletedTask);

            var logger = new Mock<ILogger<RecordOversightOutcomeHandler>>();
            var handler = new RecordOversightOutcomeHandler(repository.Object, logger.Object);

            var result = await handler.Handle(command, new CancellationToken());

            result.Should().BeFalse();

            repository.Verify(x => x.UpdateOversightReviewStatus(command.ApplicationId, command.OversightStatus,
                                                                command.UserId, command.UserName), Times.Once);
            repository.Verify(x => x.UpdateApplicationStatus(command.ApplicationId, ApplicationReviewStatus.Approved), Times.Never);
        }
    }   
}
