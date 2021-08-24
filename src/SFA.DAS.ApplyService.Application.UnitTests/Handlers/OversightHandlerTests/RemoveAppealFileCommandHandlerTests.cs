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
    [Ignore("placed on ignore as new appeal work to be done that will make use of this")]
    public class RemoveAppealFileCommandHandlerTests
    {
        private DeleteAppealFileCommandHandler _handler;
        private Mock<IAppealFileRepository> _appealUploadRepository;
        private Mock<IAppealsFileStorage> _appealFileStorage;
        private Mock<IAuditService> _auditService;

        private DeleteAppealFileCommand _command;
        private readonly Guid _applicationId = Guid.NewGuid();
        private readonly Guid _fileStorageFileId = Guid.NewGuid();
        private readonly Guid _appealUploadId = Guid.NewGuid();

        [SetUp]
        public void Setup()
        {
            _command = new DeleteAppealFileCommand
            {
                ApplicationId = _applicationId,
                FileId = _appealUploadId,
                UserId = "userId",
                UserName = "userName"
            };

            var upload = new AppealFile { Id = _appealUploadId, FileStorageReference = _fileStorageFileId, ApplicationId = _applicationId };

            _appealUploadRepository = new Mock<IAppealFileRepository>();
            _appealUploadRepository.Setup(x => x.Get(_appealUploadId)).ReturnsAsync(upload);
            _appealUploadRepository.Setup(x => x.Remove(It.IsAny<Guid>()));

            _appealFileStorage = new Mock<IAppealsFileStorage>();
            _appealFileStorage.Setup(x => x.Remove(_applicationId, It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _auditService = new Mock<IAuditService>();
            _auditService.Setup(x => x.StartTracking(UserAction.RemoveAppealFile, _command.UserId, _command.UserName));
            _auditService.Setup(x => x.AuditDelete(It.IsAny<AppealFile>()));

            _handler = new DeleteAppealFileCommandHandler(_appealUploadRepository.Object, _appealFileStorage.Object, _auditService.Object);
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

            _appealUploadRepository.Verify(x => x.Remove(It.Is<Guid>(id => id == _command.FileId)), Times.Once);
        }

        [Test]
        public async Task Handle_Audits_Removed_File()
        {
            await _handler.Handle(_command, CancellationToken.None);

            _auditService.Verify(x => x.AuditDelete(It.Is< AppealFile>(upload => upload.Id == _command.FileId)));
        }
    }
}
