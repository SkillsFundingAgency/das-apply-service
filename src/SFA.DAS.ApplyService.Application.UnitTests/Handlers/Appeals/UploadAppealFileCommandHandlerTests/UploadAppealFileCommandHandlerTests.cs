using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Appeals.Commands.UploadAppealFile;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Domain.Models;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.Appeals.UploadAppealFileCommandHandlerTests
{
    [TestFixture]
    public class UploadAppealFileCommandHandlerTests
    {
        private UploadAppealFileCommandHandler _handler;
        private Mock<IAppealFileRepository> _appealUploadRepository;
        private Mock<IAuditService> _auditService;

        private UploadAppealFileCommand _command;
        private readonly Guid _applicationId = Guid.NewGuid();

        [SetUp]
        public void Setup()
        {
            _command = new UploadAppealFileCommand
            {
                ApplicationId = _applicationId,
                AppealFile = new FileUpload { FileName = "test.pdf", Data = new byte[] { 0x25, 0x50, 0x44, 0x46 }, ContentType = "application/pdf" },
                UserId = "userId",
                UserName = "userName"
            };

            _appealUploadRepository = new Mock<IAppealFileRepository>();
            _appealUploadRepository.Setup(x => x.Add(It.IsAny<AppealFile>()));


            _auditService = new Mock<IAuditService>();
            _auditService.Setup(x => x.StartTracking(UserAction.UploadAppealFile, _command.UserId, _command.UserName));
            _auditService.Setup(x => x.AuditInsert(It.IsAny<AppealFile>()));

            _handler = new UploadAppealFileCommandHandler(_appealUploadRepository.Object, _auditService.Object);
        }

        [Test]
        public async Task Handle_Records_Uploaded_File()
        {
            await _handler.Handle(_command, CancellationToken.None);

            _appealUploadRepository.Verify(x => x.Add(It.Is<AppealFile>(upload =>
                upload.ApplicationId == _command.ApplicationId
                && upload.FileName == _command.AppealFile.FileName
                && upload.ContentType == _command.AppealFile.ContentType
                && upload.UserId == _command.UserId
                && upload.UserName == _command.UserName
                && upload.Size == _command.AppealFile.Data.Length
                )));

            _auditService.Verify(x => x.Save());
        }

        [Test]
        public async Task Handle_Audits_Added_File()
        {
            await _handler.Handle(_command, CancellationToken.None);

            _auditService.Verify(x => x.StartTracking(UserAction.UploadAppealFile, _command.UserId, _command.UserName), Times.Once);

            _auditService.Verify(x => x.AuditInsert(It.Is<AppealFile>(upload =>
                upload.ApplicationId == _command.ApplicationId
                && upload.FileName == _command.AppealFile.FileName
                && upload.ContentType == _command.AppealFile.ContentType
                && upload.UserId == _command.UserId
                && upload.UserName == _command.UserName
                && upload.Size == _command.AppealFile.Data.Length
                )));

            _auditService.Verify(x => x.Save());
        }
    }
}
