using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Oversight.Commands.CreateAppeal;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Types;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.OversightHandlerTests
{
    [TestFixture]
    public class CreateAppealCommandHandlerTests
    {
        private CreateAppealCommandHandler _handler;
        private Mock<IAppealRepository> _appealRepository;
        private Mock<IAppealUploadRepository> _appealUploadRepository;
        private Mock<IAuditService> _auditService;
        private Mock<IOversightReviewRepository> _oversightReviewRepository;

        private CreateAppealCommand _command;
        private readonly Guid _oversightReviewId = Guid.NewGuid();
        private readonly Guid _applicationId = Guid.NewGuid();
        private OversightReview _oversightReview;
        private List<AppealUpload> _appealUploads;
        private Guid _appealId;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            _appealId = Guid.NewGuid();

            _oversightReview = new OversightReview
            {
                Id = _oversightReviewId,
                ApplicationId = _applicationId,
                Status = OversightReviewStatus.Unsuccessful
            };

            _appealUploads = new List<AppealUpload>
            {
                new AppealUpload
                {
                    Id = Guid.NewGuid(),
                    ApplicationId = _applicationId
                },
                new AppealUpload
                {
                    Id = Guid.NewGuid(),
                    ApplicationId = _applicationId
                }
            };

            _command = new CreateAppealCommand
            {
                ApplicationId = _applicationId,
                OversightReviewId = _oversightReviewId,
                Message = _fixture.Create<string>(),
                UserId = _fixture.Create<string>(),
                UserName = _fixture.Create<string>()
            };

            _oversightReviewRepository = new Mock<IOversightReviewRepository>();
            _oversightReviewRepository.Setup(x => x.GetById(_oversightReviewId))
                .ReturnsAsync(() => _oversightReview);

            _appealRepository = new Mock<IAppealRepository>();
            _appealRepository.Setup(x => x.Add(It.IsAny<Appeal>())).Callback<Appeal>(appeal => _appealId = appeal.Id);

            _appealUploadRepository = new Mock<IAppealUploadRepository>();
            _appealUploadRepository.Setup(x => x.GetByApplicationId(_applicationId)).ReturnsAsync(_appealUploads);

            _auditService = new Mock<IAuditService>();
            _auditService.Setup(x => x.StartTracking(UserAction.UploadAppealFile, _command.UserId, _command.UserName));
            _auditService.Setup(x => x.AuditInsert(It.IsAny<AppealUpload>()));

            _handler = new CreateAppealCommandHandler(_oversightReviewRepository.Object, _appealRepository.Object, _appealUploadRepository.Object, _auditService.Object);
        }

        [Test]
        public async Task Handle_Adds_Appeal()
        {
            await _handler.Handle(_command, CancellationToken.None);

            _appealRepository.Verify(
                x => x.Add(It.Is<Appeal>(appeal =>
                    appeal.OversightReviewId == _oversightReviewId &&
                    appeal.Message == _command.Message &&
                    appeal.UserId == _command.UserId &&
                    appeal.UserName == _command.UserName &&
                    appeal.CreatedOn.Date == DateTime.UtcNow.Date)));
        }

        [Test]
        public async Task Handle_Audits_New_Appeal()
        {
            await _handler.Handle(_command, CancellationToken.None);

            _auditService.Verify(x=> x.StartTracking(UserAction.CreateAppeal, _command.UserId, _command.UserName), Times.Once);

            _auditService.Verify(x => x.AuditInsert(It.Is<Appeal>(appeal =>
                appeal.OversightReviewId == _oversightReviewId &&
                appeal.Message == _command.Message &&
                appeal.UserId == _command.UserId &&
                appeal.UserName == _command.UserName &&
                appeal.CreatedOn.Date == DateTime.UtcNow.Date)));
        }

        [Test]
        public async Task Handle_Updates_Staged_Files_To_The_New_Appeal()
        {
            await _handler.Handle(_command, CancellationToken.None);

            foreach (var upload in _appealUploads)
            {
                _appealUploadRepository.Verify(x =>
                    x.Update(It.Is<AppealUpload>(u =>
                        u.Id == upload.Id && u.AppealId == _appealId)));
            }
        }

        [Test]
        public async Task Handle_Audits_Updates_To_Staged_Files()
        {
            await _handler.Handle(_command, CancellationToken.None);

            foreach (var upload in _appealUploads)
            {
                _auditService.Verify(x => x.AuditUpdate(It.Is<AppealUpload>(u =>
                        u.Id == upload.Id && u.AppealId == _appealId)));
            }
        }

        [TestCase(OversightReviewStatus.InProgress)]
        [TestCase(OversightReviewStatus.Rejected)]
        [TestCase(OversightReviewStatus.Removed)]
        [TestCase(OversightReviewStatus.Successful)]
        [TestCase(OversightReviewStatus.SuccessfulAlreadyActive)]
        [TestCase(OversightReviewStatus.SuccessfulFitnessForFunding)]
        [TestCase(OversightReviewStatus.Withdrawn)]
        public void Handle_Throws_If_Oversight_Review_Is_Not_Unsuccessful(OversightReviewStatus status)
        {
            _oversightReview.Status = status;
            Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(_command, CancellationToken.None));
        }

        [Test]
        public void Handle_Throws_If_Oversight_Review_Has_Already_Been_Appealed()
        {
            _appealRepository.Setup(x => x.GetByOversightReviewId(_oversightReviewId))
                .ReturnsAsync(() => new Appeal());

            Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(_command, CancellationToken.None));
        }

        [Test]
        public void Handle_Throws_If_Oversight_Review_Does_Not_Exist()
        {
            _oversightReviewRepository.Setup(x => x.GetById(_oversightReviewId))
                .ReturnsAsync(() => null);

            Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(_command, CancellationToken.None));
        }

        [Test]
        public void Handle_Throws_If_Oversight_Review_Does_Not_Belong_To_Application()
        {
            _oversightReview.ApplicationId = Guid.NewGuid();
            Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(_command, CancellationToken.None));
        }
    }
}
