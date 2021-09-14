using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Appeals.Commands;
using SFA.DAS.ApplyService.Application.Apply.Oversight;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Data.UnitOfWork;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Types;
using SFA.DAS.ApplyService.EmailService.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.Appeals
{
    [TestFixture]
    public class RecordAppealOutcomeHandlerTests
    {
        private RecordAppealOutcomeHandler _handler;
        private Mock<IApplyRepository> _applyRepository;
        private Mock<IAppealRepository> _appealRepository;
        private Mock<IApplicationRepository> _applicationRepository;
        private Mock<IApplicationUpdatedEmailService> _applicationUpdatedEmailService;
        private Mock<IAuditService> _auditService;
        private Mock<ILogger<RecordAppealOutcomeHandler>> _logger;
        private Mock<IUnitOfWork> _unitOfWork;        
        private Guid _applicationId;

        [SetUp]
        public void SetUp()
        {
            _applicationId = Guid.NewGuid();

            _applyRepository = new Mock<IApplyRepository>();
            _applyRepository.Setup(x => x.GetApplication(_applicationId)).ReturnsAsync(() => new Domain.Entities.Apply
            { ApplicationId = _applicationId, Status = ApplicationStatus.Submitted });
            _applyRepository.Setup(x => x.GetContactForApplication(_applicationId)).ReturnsAsync(() => new Domain.Entities.Contact
            { FamilyName = "kuruva" });

            _applicationRepository = new Mock<IApplicationRepository>();
            _applicationRepository.Setup(x => x.GetApplication(_applicationId)).ReturnsAsync(() => new Domain.Entities.Apply
            { ApplicationId = _applicationId, Status = ApplicationStatus.InProgress, ApplyData = new ApplyData { ApplyDetails = new ApplyDetails { ApplicationRemovedOn = DateTime.Today } } });
            _applicationRepository.Setup(x => x.Update(It.IsAny<Domain.Entities.Apply>()));

            _applicationUpdatedEmailService = new Mock<IApplicationUpdatedEmailService>();

            _auditService = new Mock<IAuditService>();
            _auditService.Setup(x => x.AuditInsert(It.IsAny<OversightReview>()));

             _logger = new Mock<ILogger<RecordAppealOutcomeHandler>>();
            _unitOfWork = new Mock<IUnitOfWork>();

            _appealRepository = new Mock<IAppealRepository>();
            _appealRepository.Setup(x => x.GetByApplicationId(_applicationId)).ReturnsAsync(() => new Appeal { Status = AppealStatus.InProgress, ApplicationId = _applicationId });

        }


        [TestCase(AppealStatus.Successful, ApplicationStatus.Successful, GatewayReviewStatus.Rejected, 1)]
        [TestCase(AppealStatus.Unsuccessful, ApplicationStatus.Unsuccessful, GatewayReviewStatus.Pass, 1)]
        [TestCase(AppealStatus.Unsuccessful, ApplicationStatus.Rejected, GatewayReviewStatus.Rejected, 0)]
        public async Task Record_appeal_outcome_updates_appeal_status_and_applies_correct_application_status(string appealStatus, string applicationStatus, string gatewayReviewStatus, int noOfCallsToApply)
        {
            var command = new RecordAppealOutcomeCommand
            {
                ApplicationId = Guid.NewGuid(),
                AppealStatus = appealStatus,
                UserName = "test user",
                UserId = "testUser",
                InternalComments = "testInternalComments",
                ExternalComments = "testExternalComments"
            };

            var appealRepository = new Mock<IAppealRepository>();
            appealRepository.Setup(x => x.Add(It.IsAny<Appeal>()));
            appealRepository.Setup(x => x.GetByApplicationId(It.IsAny<Guid>())).ReturnsAsync(() => null);

            var repository = new Mock<IApplicationRepository>();
            repository.Setup(x => x.GetApplication(command.ApplicationId)).ReturnsAsync(() => new Domain.Entities.Apply
                {ApplicationId = command.ApplicationId, Status = ApplicationStatus.Submitted, GatewayReviewStatus = gatewayReviewStatus});
            repository.Setup(x => x.Update(It.IsAny<Domain.Entities.Apply>()));

            var logger = new Mock<ILogger<RecordAppealOutcomeHandler>>();
            var handler = new RecordAppealOutcomeHandler(logger.Object, appealRepository.Object, repository.Object, Mock.Of<IAuditService>(),  Mock.Of<IUnitOfWork>(), Mock.Of<IApplicationUpdatedEmailService>());

            var result = await handler.Handle(command, new CancellationToken());

            result.Should().BeTrue();

            appealRepository.Verify(
                x => x.Add(It.Is<Appeal>(
                    r => r.ApplicationId == command.ApplicationId
                         && r.AppealDeterminedDate.Value.Date == DateTime.UtcNow.Date
                         && r.InternalComments == command.InternalComments
                         && r.ExternalComments == command.ExternalComments
                         && r.Status == command.AppealStatus
                         )),
                Times.Once);

            repository.Verify(x => x.Update(It.Is<Domain.Entities.Apply>(apply =>
                    apply.ApplicationId == command.ApplicationId && apply.ApplicationStatus == applicationStatus)),
                Times.Exactly(noOfCallsToApply));
        }

        [Test]
        public async Task Record_appeal_InProgress_Is_Recorded_In_Relevant_Properties()
        {
            var command = new RecordAppealOutcomeCommand
            {
                ApplicationId = Guid.NewGuid(),
                AppealStatus = AppealStatus.InProgress,
                UserName = "test user",
                UserId = "testUser",
                InternalComments = "testInternalComments",
                ExternalComments = "testExternalComments"
            };
        
            var appealRepository = new Mock<IAppealRepository>();
            appealRepository.Setup(x => x.Add(It.IsAny<Appeal>()));
            appealRepository.Setup(x => x.GetByApplicationId(It.IsAny<Guid>())).ReturnsAsync(() => null);
        
            var repository = new Mock<IApplicationRepository>();
            repository.Setup(x => x.GetApplication(command.ApplicationId)).ReturnsAsync(() => new Domain.Entities.Apply
                { ApplicationId = command.ApplicationId, Status = ApplicationStatus.Submitted });
            repository.Setup(x => x.Update(It.IsAny<Domain.Entities.Apply>()));
        
            var logger = new Mock<ILogger<RecordAppealOutcomeHandler>>();
            var handler = new RecordAppealOutcomeHandler(logger.Object, appealRepository.Object, repository.Object, Mock.Of<IAuditService>(), Mock.Of<IUnitOfWork>(), Mock.Of<IApplicationUpdatedEmailService>());

            var result = await handler.Handle(command, new CancellationToken());
        
            result.Should().BeTrue();
        
            appealRepository.Verify(
                x => x.Add(It.Is<Appeal>(
                    r => r.ApplicationId == command.ApplicationId
                         && r.InProgressDate.Value.Date == DateTime.UtcNow.Date
                         && r.AppealDeterminedDate ==null
                         && r.InProgressUserId == command.UserId
                         && r.InProgressUserName == command.UserName
                         && r.InProgressInternalComments == command.InternalComments
                         && r.InProgressExternalComments == command.ExternalComments
                         && r.InternalComments == null
                         && r.ExternalComments == null
                         && r.Status == AppealStatus.InProgress
                         )),
                Times.Once);
        
            repository.Verify(x => x.Update(It.Is<Domain.Entities.Apply>(apply =>
                    apply.ApplicationId == command.ApplicationId && apply.ApplicationStatus == ApplicationStatus.InProgressAppeal)),
                Times.Once);
        }
        
	    [TestCase(AppealStatus.Successful)]
        [TestCase(AppealStatus.SuccessfulAlreadyActive)]
        [TestCase(AppealStatus.SuccessfulFitnessForFunding)]
        public async Task Record_appeal_For_gateway_fail_Is_Recorded_As_AppealSuccessful(string appealStatus)
        {
            var command = new RecordAppealOutcomeCommand
            {
                ApplicationId = Guid.NewGuid(),
                AppealStatus = appealStatus,
                UserName = "test user",
                UserId = "testUser",
                InternalComments = "testInternalComments",
                ExternalComments = "testExternalComments"
            };

            var appealRepository = new Mock<IAppealRepository>();
            appealRepository.Setup(x => x.Add(It.IsAny<Appeal>()));
            appealRepository.Setup(x => x.GetByApplicationId(It.IsAny<Guid>())).ReturnsAsync(() => null);

            var repository = new Mock<IApplicationRepository>();
            repository.Setup(x => x.GetApplication(command.ApplicationId)).ReturnsAsync(() => new Domain.Entities.Apply
            { ApplicationId = command.ApplicationId, Status = ApplicationStatus.Submitted, GatewayReviewStatus = GatewayReviewStatus.Fail});
            repository.Setup(x => x.Update(It.IsAny<Domain.Entities.Apply>()));

            var logger = new Mock<ILogger<RecordAppealOutcomeHandler>>();
            var handler = new RecordAppealOutcomeHandler(logger.Object, appealRepository.Object, repository.Object, Mock.Of<IAuditService>(), Mock.Of<IUnitOfWork>(), Mock.Of<IApplicationUpdatedEmailService>());

            var result = await handler.Handle(command, new CancellationToken());

            result.Should().BeTrue();

            appealRepository.Verify(
                x => x.Add(It.Is<Appeal>(
                    r => r.ApplicationId == command.ApplicationId
                         && r.AppealDeterminedDate.Value.Date == DateTime.UtcNow.Date
                         && r.InternalComments == command.InternalComments
                         && r.ExternalComments == command.ExternalComments
                         && r.Status == command.AppealStatus
                         )),
                Times.Once);

            repository.Verify(x => x.Update(It.Is<Domain.Entities.Apply>(apply =>
                    apply.ApplicationId == command.ApplicationId && apply.ApplicationStatus == ApplicationStatus.AppealSuccessful)),
                Times.Once);
        }

 	    [TestCase(AppealStatus.Successful)]
        [TestCase(AppealStatus.SuccessfulAlreadyActive)]
        [TestCase(AppealStatus.SuccessfulFitnessForFunding)]
        public async Task Record_appeal_For_application_removed_Is_Recorded_As_AppealSuccessful(string appealStatus)
        {
            var command = new RecordAppealOutcomeCommand
            {
                ApplicationId = Guid.NewGuid(),
                AppealStatus = appealStatus,
                UserName = "test user",
                UserId = "testUser",
                InternalComments = "testInternalComments",
                ExternalComments = "testExternalComments"
            };

            var appealRepository = new Mock<IAppealRepository>();
            appealRepository.Setup(x => x.Add(It.IsAny<Appeal>()));
            appealRepository.Setup(x => x.GetByApplicationId(It.IsAny<Guid>())).ReturnsAsync(() => null);

            var repository = new Mock<IApplicationRepository>();
            repository.Setup(x => x.GetApplication(command.ApplicationId)).ReturnsAsync(() => new Domain.Entities.Apply
            { ApplicationId = command.ApplicationId, Status = ApplicationStatus.Submitted, ApplyData = new ApplyData{ApplyDetails = new ApplyDetails { ApplicationRemovedOn = DateTime.Today} }});
            repository.Setup(x => x.Update(It.IsAny<Domain.Entities.Apply>()));

            var logger = new Mock<ILogger<RecordAppealOutcomeHandler>>();
            var handler = new RecordAppealOutcomeHandler(logger.Object, appealRepository.Object, repository.Object, Mock.Of<IAuditService>(), Mock.Of<IUnitOfWork>(), Mock.Of<IApplicationUpdatedEmailService>());

            var result = await handler.Handle(command, new CancellationToken());

            result.Should().BeTrue();

            appealRepository.Verify(
                x => x.Add(It.Is<Appeal>(
                    r => r.ApplicationId == command.ApplicationId
                         && r.AppealDeterminedDate.Value.Date == DateTime.UtcNow.Date
                         && r.InternalComments == command.InternalComments
                         && r.ExternalComments == command.ExternalComments
                         && r.Status == command.AppealStatus
                         )),
                Times.Once);

            repository.Verify(x => x.Update(It.Is<Domain.Entities.Apply>(apply =>
                    apply.ApplicationId == command.ApplicationId && apply.ApplicationStatus == ApplicationStatus.AppealSuccessful)),
                Times.Once);
        } 



        [TestCase(AppealStatus.InProgress, false)]
        [TestCase(AppealStatus.Successful, true)]
        [TestCase(AppealStatus.SuccessfulAlreadyActive, true)]
        [TestCase(AppealStatus.SuccessfulFitnessForFunding, true)]
        [TestCase(AppealStatus.Unsuccessful, true)]
        public void Record_appeal_Throws_Exception_If_Already_Recorded_Unless_InProgress(string originalStatus, bool expectThrows)
        {
            var applicationId = Guid.NewGuid();
        
            var command = new RecordAppealOutcomeCommand
            {
                ApplicationId = applicationId,
                AppealStatus = AppealStatus.Successful,
                UserName = "test user",
                UserId = "testUser",
                InternalComments = "testInternalComments",
                ExternalComments = "testExternalComments"
            };
        
            var existingAppeal = new Appeal
            {
                Status = originalStatus
            };
        
            var appealRepository = new Mock<IAppealRepository>();
            appealRepository.Setup(x => x.Update(It.IsAny<Appeal>()));
            appealRepository.Setup(x => x.GetByApplicationId(It.IsAny<Guid>())).ReturnsAsync(() => existingAppeal);
        
            var repository = new Mock<IApplicationRepository>();
            repository.Setup(x => x.GetApplication(command.ApplicationId)).ReturnsAsync(() => new Domain.Entities.Apply
                { ApplicationId = command.ApplicationId, Status = ApplicationStatus.Submitted });
        
            var logger = new Mock<ILogger<RecordAppealOutcomeHandler>>();
            var handler = new RecordAppealOutcomeHandler(logger.Object, appealRepository.Object, repository.Object, Mock.Of<IAuditService>(),  Mock.Of<IUnitOfWork>(), Mock.Of<IApplicationUpdatedEmailService>());

            if (expectThrows)
            {
                Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(command, new CancellationToken()));
            }
            else
            {
                Assert.DoesNotThrowAsync(async () => await handler.Handle(command, new CancellationToken()));
            }
        }

        [Test]
        public async Task Handle_User_Is_Notified_Of_Update_To__Applicant()
        {
            var request = new RecordAppealOutcomeCommand
            {
                ApplicationId = _applicationId,
                UserId = "test user id",
                UserName = "test user name",
                AppealStatus = AppealStatus.Successful,
                ExternalComments = "External Commnets",
                InternalComments = "Internal Comments",
            };                      
            
            _handler = new RecordAppealOutcomeHandler(_logger.Object, _appealRepository.Object, _applicationRepository.Object, _auditService.Object, _unitOfWork.Object, _applicationUpdatedEmailService.Object);

            await  _handler.Handle(request,CancellationToken.None); 

            _applicationUpdatedEmailService.Verify(x => x.SendEmail(request.ApplicationId), Times.Once);
        }
    }   
}
