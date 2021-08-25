using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Appeals.Commands.DeleteAppealFile;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.Appeals.DeleteAppealFileCommandHandlerTests
{
    [TestFixture]
    public class DeleteAppealFileCommandHandlerTests
    {
        private DeleteAppealFileCommandHandler _handler;
        private Mock<IAppealFileRepository> _appealUploadRepository;
        private Mock<IAuditService> _auditService;

        private DeleteAppealFileCommand _command;
        private readonly Guid _applicationId = Guid.NewGuid();
        private readonly string _fileName = Guid.NewGuid().ToString();
        private readonly Guid _appealUploadId = Guid.NewGuid();

        [SetUp]
        public void Setup()
        {
            _command = new DeleteAppealFileCommand
            {
                ApplicationId = _applicationId,
                FileName = _fileName,
                UserId = "userId",
                UserName = "userName"
            };

            var upload = new AppealFile { Id = _appealUploadId, FileName = _fileName, ApplicationId = _applicationId };

            _appealUploadRepository = new Mock<IAppealFileRepository>();
            _appealUploadRepository.Setup(x => x.Get(_applicationId, _fileName)).ReturnsAsync(upload);
            _appealUploadRepository.Setup(x => x.Get(_appealUploadId)).ReturnsAsync(upload);
            _appealUploadRepository.Setup(x => x.Remove(It.IsAny<Guid>()));

            _auditService = new Mock<IAuditService>();
            _auditService.Setup(x => x.StartTracking(UserAction.RemoveAppealFile, _command.UserId, _command.UserName));
            _auditService.Setup(x => x.AuditDelete(It.IsAny<AppealFile>()));

            _handler = new DeleteAppealFileCommandHandler(_appealUploadRepository.Object, _auditService.Object);
        }

        [Test]
        public async Task Handle_Records_File_Removal()
        {
            await _handler.Handle(_command, CancellationToken.None);

            _appealUploadRepository.Verify(x => x.Remove(It.Is<Guid>(id => id == _appealUploadId)), Times.Once);
        }

        [Test]
        public async Task Handle_Audits_Removed_File()
        {
            await _handler.Handle(_command, CancellationToken.None);

            _auditService.Verify(x => x.AuditDelete(It.Is<AppealFile>(upload => upload.Id == _appealUploadId)));
        }
    }
}
