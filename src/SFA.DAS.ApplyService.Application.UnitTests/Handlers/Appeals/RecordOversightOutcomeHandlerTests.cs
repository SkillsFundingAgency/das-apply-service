using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Oversight;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Data.UnitOfWork;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.EmailService.Interfaces;
using SFA.DAS.ApplyService.Types;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.Appeals
{
    //MFCMFC
    [TestFixture]
    public class RecordAppealOutcomeHandlerTests
    {
        [TestCase(OversightReviewStatus.Successful, ApplicationStatus.Successful, GatewayReviewStatus.Rejected)]  
        [TestCase(OversightReviewStatus.Unsuccessful, ApplicationStatus.Unsuccessful, GatewayReviewStatus.Pass)]
        [TestCase(OversightReviewStatus.Unsuccessful, ApplicationStatus.Rejected, GatewayReviewStatus.Rejected)]
        public async Task Record_oversight_outcome_updates_oversight_status_and_applies_correct_application_status(OversightReviewStatus oversightReviewStatus, string applicationStatus, string gatewayReviewStatus)
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
            oversightReviewRepository.Setup(x => x.Add(It.IsAny<OversightReview>()));
            oversightReviewRepository.Setup(x => x.GetByApplicationId(It.IsAny<Guid>())).ReturnsAsync(() => null);

            var repository = new Mock<IApplicationRepository>();
            repository.Setup(x => x.GetApplication(command.ApplicationId)).ReturnsAsync(() => new Domain.Entities.Apply
                {ApplicationId = command.ApplicationId, Status = ApplicationStatus.Submitted, GatewayReviewStatus = gatewayReviewStatus});
            repository.Setup(x => x.Update(It.IsAny<Domain.Entities.Apply>()));

            var logger = new Mock<ILogger<RecordOversightOutcomeHandler>>();
            var handler = new RecordOversightOutcomeHandler(logger.Object, oversightReviewRepository.Object, repository.Object, Mock.Of<IAuditService>(), Mock.Of<IApplicationUpdatedEmailService>(), Mock.Of<IUnitOfWork>());

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

            repository.Verify(x => x.Update(It.Is<Domain.Entities.Apply>(apply =>
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
            oversightReviewRepository.Setup(x => x.Add(It.IsAny<OversightReview>()));
            oversightReviewRepository.Setup(x => x.GetByApplicationId(It.IsAny<Guid>())).ReturnsAsync(() => null);

            var repository = new Mock<IApplicationRepository>();
            repository.Setup(x => x.GetApplication(command.ApplicationId)).ReturnsAsync(() => new Domain.Entities.Apply
                { ApplicationId = command.ApplicationId, Status = ApplicationStatus.Submitted });
            repository.Setup(x => x.Update(It.IsAny<Domain.Entities.Apply>()));

            var logger = new Mock<ILogger<RecordOversightOutcomeHandler>>();
            var handler = new RecordOversightOutcomeHandler(logger.Object, oversightReviewRepository.Object, repository.Object, Mock.Of<IAuditService>(), Mock.Of<IApplicationUpdatedEmailService>(), Mock.Of<IUnitOfWork>());

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

            repository.Verify(x => x.Update(It.Is<Domain.Entities.Apply>(apply =>
                    apply.ApplicationId == command.ApplicationId && apply.ApplicationStatus == ApplicationStatus.InProgressOutcome)),
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
            oversightReviewRepository.Setup(x => x.Update(It.IsAny<OversightReview>()));
            oversightReviewRepository.Setup(x => x.GetByApplicationId(It.IsAny<Guid>())).ReturnsAsync(() => existingOversightReview);

            var repository = new Mock<IApplicationRepository>();
            repository.Setup(x => x.GetApplication(command.ApplicationId)).ReturnsAsync(() => new Domain.Entities.Apply
                { ApplicationId = command.ApplicationId, Status = ApplicationStatus.Submitted });

            var logger = new Mock<ILogger<RecordOversightOutcomeHandler>>();
            var handler = new RecordOversightOutcomeHandler(logger.Object, oversightReviewRepository.Object, repository.Object, Mock.Of<IAuditService>(), Mock.Of<IApplicationUpdatedEmailService>(), Mock.Of<IUnitOfWork>());

            if (expectThrows)
            {
                Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(command, new CancellationToken()));
            }
            else
            {
                Assert.DoesNotThrowAsync(async () => await handler.Handle(command, new CancellationToken()));
            }
        }

        [TestCase(OversightReviewStatus.SuccessfulAlreadyActive)]
        [TestCase(OversightReviewStatus.Successful)]
        [TestCase(OversightReviewStatus.SuccessfulFitnessForFunding)]
        [TestCase(OversightReviewStatus.Unsuccessful)]

        public async Task Handler_sends_application_updated_email_for_all_status_changes(OversightReviewStatus newOversightReviewStatus)
        {
            var applicationId = Guid.NewGuid();

            var command = new RecordOversightOutcomeCommand
            {
                ApplicationId = applicationId,
                OversightStatus = newOversightReviewStatus,
                UserName = "test user",
                UserId = "testUser",
                InternalComments = "testInternalComments",
                ExternalComments = "testExternalComments"
            };

            var existingOversightReview = new OversightReview
            {
                Status = OversightReviewStatus.InProgress
            };

            var oversightReviewRepository = new Mock<IOversightReviewRepository>();
            oversightReviewRepository.Setup(x => x.Update(It.IsAny<OversightReview>()));
            oversightReviewRepository.Setup(x => x.GetByApplicationId(It.IsAny<Guid>())).ReturnsAsync(() => existingOversightReview);

            var repository = new Mock<IApplicationRepository>();
            repository.Setup(x => x.GetApplication(command.ApplicationId)).ReturnsAsync(() => new Domain.Entities.Apply
            { ApplicationId = command.ApplicationId, Status = ApplicationStatus.Submitted });

            var applicationUpdatedEmailService = new Mock<IApplicationUpdatedEmailService>();
            var logger = new Mock<ILogger<RecordOversightOutcomeHandler>>();
            var handler = new RecordOversightOutcomeHandler(logger.Object, oversightReviewRepository.Object, repository.Object, Mock.Of<IAuditService>(), applicationUpdatedEmailService.Object, Mock.Of<IUnitOfWork>());
            await handler.Handle(command, new CancellationToken());

            applicationUpdatedEmailService.Verify(x => x.SendEmail(It.Is<Guid>(id => id == command.ApplicationId)), Times.Once);
        }
    }   
}
