using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Oversight;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Data.FileStorage;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.OversightHandlerTests
{
    [TestFixture]
    public class RemoveAppealFileCommandHandlerTests
    {
        private RemoveAppealFileCommandHandler _handler;
        private Mock<IAppealUploadRepository> _appealUploadRepository;
        private Mock<IAppealFileStorage> _appealFileStorage;
        private Mock<IAuditService> _auditService;

        private RemoveAppealFileCommand _command;
        private readonly Guid _applicationId = Guid.NewGuid();
        private readonly Guid _fileStorageFileId = Guid.NewGuid();
        private readonly Guid _appealUploadId = Guid.NewGuid();

        [SetUp]
        public void Setup()
        {
            _command = new RemoveAppealFileCommand
            {
                ApplicationId = _applicationId,
                AppealUploadId = _appealUploadId,
                UserId = "userId",
                UserName = "userName"
            };

            var upload = new AppealUpload { Id = _appealUploadId, FileId = _fileStorageFileId, ApplicationId = _applicationId };

            _appealUploadRepository = new Mock<IAppealUploadRepository>();
            _appealUploadRepository.Setup(x => x.GetById(_appealUploadId)).ReturnsAsync(upload);
            _appealUploadRepository.Setup(x => x.Remove(It.IsAny<Guid>()));

            _appealFileStorage = new Mock<IAppealFileStorage>();
            _appealFileStorage.Setup(x => x.Remove(_applicationId, It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _auditService = new Mock<IAuditService>();
            _auditService.Setup(x => x.StartTracking(UserAction.RemoveAppealFile, _command.UserId, _command.UserName));
            _auditService.Setup(x => x.AuditDelete(It.IsAny<AppealUpload>()));

            _handler = new RemoveAppealFileCommandHandler(_appealUploadRepository.Object, _appealFileStorage.Object, _auditService.Object);
        }

        [Test]
        public async Task Handle_Removes_File_From_Appeals_File_Storage()
        {
            await _handler.Handle(_command, CancellationToken.None);

            _appealFileStorage.Verify(x => x.Remove(_applicationId,
                It.Is<Guid>(id => id == _fileStorageFileId),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task Handle_Records_File_Removal()
        {
            await _handler.Handle(_command, CancellationToken.None);

            _appealUploadRepository.Verify(x => x.Remove(It.Is<Guid>(id => id == _command.AppealUploadId)), Times.Once);
        }

        [Test]
        public async Task Handle_Audits_Removed_File()
        {
            await _handler.Handle(_command, CancellationToken.None);

            _auditService.Verify(x => x.AuditDelete(It.Is< AppealUpload>(upload => upload.Id == _command.AppealUploadId)));
        }
    }
}
