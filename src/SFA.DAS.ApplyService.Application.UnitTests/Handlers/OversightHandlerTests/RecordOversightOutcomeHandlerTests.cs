
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
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Types;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.OversightHandlerTests
{
    [TestFixture]
    public class RecordOversightOutcomeHandlerTests
    {
        [TestCase(OversightReviewStatus.Successful, ApplicationStatus.Approved)]
        [TestCase(OversightReviewStatus.Unsuccessful, ApplicationStatus.Rejected)]
        public async Task Record_oversight_outcome_updates_oversight_status_and_applies_correct_application_status(OversightReviewStatus oversightReviewStatus, string applicationStatus)
        {
            var command = new RecordOversightOutcomeCommand
            {
                ApplicationId = Guid.NewGuid(),
                OversightStatus = oversightReviewStatus,
                UserName = "test user",
                UserId = "testUser",
                InternalComments = "testInternalComments",
                ExternalComments = "testExternalComments"
            };

            var oversightReviewRepository = new Mock<IOversightReviewRepository>();
            oversightReviewRepository.Setup(x => x.Add(It.IsAny<OversightReview>())).Returns(() => Task.CompletedTask);
            oversightReviewRepository.Setup(x => x.GetByApplicationId(It.IsAny<Guid>())).ReturnsAsync(() => null);

            var repository = new Mock<IApplyRepository>();
            repository.Setup(x => x.GetApplication(command.ApplicationId)).ReturnsAsync(() => new Domain.Entities.Apply
                {ApplicationId = command.ApplicationId, Status = ApplicationStatus.Submitted});
            repository.Setup(x => x.UpdateApplication(It.IsAny<Domain.Entities.Apply>())).Returns(Task.CompletedTask);

            var logger = new Mock<ILogger<RecordOversightOutcomeHandler>>();
            var handler = new RecordOversightOutcomeHandler(logger.Object, oversightReviewRepository.Object, repository.Object, Mock.Of<IAuditService>());

            var result = await handler.Handle(command, new CancellationToken());

            result.Should().BeTrue();

            oversightReviewRepository.Verify(
                x => x.Add(It.Is<OversightReview>(
                    r => r.ApplicationId == command.ApplicationId
                         && r.ApplicationDeterminedDate.Value.Date == DateTime.UtcNow.Date
                         && r.InternalComments == command.InternalComments
                         && r.ExternalComments == command.ExternalComments
                         && r.Status == command.OversightStatus
                         )),
                Times.Once);

            repository.Verify(x => x.UpdateApplication(It.Is<Domain.Entities.Apply>(apply =>
                    apply.ApplicationId == command.ApplicationId && apply.ApplicationStatus == applicationStatus)),
                Times.Once);
        }

        [Test]
        public async Task Record_oversight_InProgress_Is_Recorded_In_Relevant_Properties()
        {
            var command = new RecordOversightOutcomeCommand
            {
                ApplicationId = Guid.NewGuid(),
                OversightStatus = OversightReviewStatus.InProgress,
                UserName = "test user",
                UserId = "testUser",
                InternalComments = "testInternalComments",
                ExternalComments = "testExternalComments"
            };

            var oversightReviewRepository = new Mock<IOversightReviewRepository>();
            oversightReviewRepository.Setup(x => x.Add(It.IsAny<OversightReview>())).Returns(() => Task.CompletedTask);
            oversightReviewRepository.Setup(x => x.GetByApplicationId(It.IsAny<Guid>())).ReturnsAsync(() => null);

            var repository = new Mock<IApplyRepository>();
            repository.Setup(x => x.GetApplication(command.ApplicationId)).ReturnsAsync(() => new Domain.Entities.Apply
                { ApplicationId = command.ApplicationId, Status = ApplicationStatus.Submitted });

            var logger = new Mock<ILogger<RecordOversightOutcomeHandler>>();
            var handler = new RecordOversightOutcomeHandler(logger.Object, oversightReviewRepository.Object, repository.Object, Mock.Of<IAuditService>());

            var result = await handler.Handle(command, new CancellationToken());

            result.Should().BeTrue();

            oversightReviewRepository.Verify(
                x => x.Add(It.Is<OversightReview>(
                    r => r.ApplicationId == command.ApplicationId
                         && r.InProgressDate.Value.Date == DateTime.UtcNow.Date
                         && r.ApplicationDeterminedDate.Value.Date == DateTime.UtcNow.Date
                         && r.InProgressUserId == command.UserId
                         && r.InProgressUserName == command.UserName
                         && r.InProgressInternalComments == command.InternalComments
                         && r.InProgressExternalComments == command.ExternalComments
                         && r.InternalComments == null
                         && r.ExternalComments == null
                         && r.Status == OversightReviewStatus.InProgress
                         )),
                Times.Once);
        }

        [TestCase(OversightReviewStatus.InProgress, false)]
        [TestCase(OversightReviewStatus.Successful, true)]
        [TestCase(OversightReviewStatus.SuccessfulAlreadyActive, true)]
        [TestCase(OversightReviewStatus.SuccessfulFitnessForFunding, true)]
        [TestCase(OversightReviewStatus.Unsuccessful, true)]
        public void Record_oversight_Throws_Exception_If_Already_Recorded_Unless_InProgress(OversightReviewStatus originalStatus, bool expectThrows)
        {
            var applicationId = Guid.NewGuid();

            var command = new RecordOversightOutcomeCommand
            {
                ApplicationId = applicationId,
                OversightStatus = OversightReviewStatus.Successful,
                UserName = "test user",
                UserId = "testUser",
                InternalComments = "testInternalComments",
                ExternalComments = "testExternalComments"
            };

            var existingOversightReview = new OversightReview
            {
                Status = originalStatus
            };

            var oversightReviewRepository = new Mock<IOversightReviewRepository>();
            oversightReviewRepository.Setup(x => x.Update(It.IsAny<OversightReview>())).Returns(() => Task.CompletedTask);
            oversightReviewRepository.Setup(x => x.GetByApplicationId(It.IsAny<Guid>())).ReturnsAsync(() => existingOversightReview);

            var repository = new Mock<IApplyRepository>();
            repository.Setup(x => x.GetApplication(command.ApplicationId)).ReturnsAsync(() => new Domain.Entities.Apply
                { ApplicationId = command.ApplicationId, Status = ApplicationStatus.Submitted });

            var logger = new Mock<ILogger<RecordOversightOutcomeHandler>>();
            var handler = new RecordOversightOutcomeHandler(logger.Object, oversightReviewRepository.Object, repository.Object, Mock.Of<IAuditService>());

            if (expectThrows)
            {
                Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(command, new CancellationToken()));
            }
            else
            {
                Assert.DoesNotThrowAsync(async () => await handler.Handle(command, new CancellationToken()));
            }
        }

    }   
}
