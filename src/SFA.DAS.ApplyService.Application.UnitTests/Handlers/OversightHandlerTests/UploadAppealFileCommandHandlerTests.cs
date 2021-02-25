using System;
using System.IO;
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
using SFA.DAS.ApplyService.Domain.Models;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.OversightHandlerTests
{
    [TestFixture]
    public class UploadAppealFileCommandHandlerTests
    {
        private UploadAppealFileCommandHandler _handler;
        private Mock<IAppealUploadRepository> _appealUploadRepository;
        private Mock<IAppealsFileStorage> _appealFileStorage;
        private Mock<IAuditService> _auditService;

        private UploadAppealFileCommand _command;
        private readonly Guid _applicationId = Guid.NewGuid();
        private readonly Guid _fileStorageFileId = Guid.NewGuid();

        [SetUp]
        public void Setup()
        {
            _command = new UploadAppealFileCommand
            {
                ApplicationId = _applicationId,
                File = GenerateTestFileUpload(),
                UserId = "userId",
                UserName = "userName"
            };

            _appealUploadRepository = new Mock<IAppealUploadRepository>();
            _appealUploadRepository.Setup(x => x.Add(It.IsAny<AppealUpload>()));

            _appealFileStorage = new Mock<IAppealsFileStorage>();
            _appealFileStorage.Setup(x => x.Add(_applicationId, It.IsAny<FileUpload>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _fileStorageFileId);

            _auditService = new Mock<IAuditService>();
            _auditService.Setup(x => x.StartTracking(UserAction.UploadAppealFile, _command.UserId, _command.UserName));
            _auditService.Setup(x => x.AuditInsert(It.IsAny<AppealUpload>()));

            _handler = new UploadAppealFileCommandHandler(_appealUploadRepository.Object, _appealFileStorage.Object, _auditService.Object);
        }

        [Test]
        public async Task Handle_Adds_File_To_Appeals_File_Storage()
        {
            await _handler.Handle(_command, CancellationToken.None);
            _appealFileStorage.Verify(x => x.Add(_applicationId,
                It.Is<FileUpload>(file => file == _command.File),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task Handle_Records_Uploaded_File()
        {
            await _handler.Handle(_command, CancellationToken.None);

            _appealUploadRepository.Verify(x => x.Add(It.Is<AppealUpload>(upload =>
                upload.ApplicationId == _command.ApplicationId
                && upload.Filename == _command.File.Filename
                && upload.ContentType == _command.File.ContentType
                && upload.FileStorageReference == _fileStorageFileId
                && upload.UserId == _command.UserId
                && upload.UserName == _command.UserName
                && upload.Size == _command.File.Data.Length
                )));
        }

        [Test]
        public async Task Handle_Audits_Added_File()
        {
            await _handler.Handle(_command, CancellationToken.None);

            _auditService.Verify(x => x.AuditInsert(It.Is<AppealUpload>(upload =>
                upload.ApplicationId == _command.ApplicationId
                && upload.Filename == _command.File.Filename
                && upload.ContentType == _command.File.ContentType
                && upload.FileStorageReference == _fileStorageFileId
                && upload.UserId == _command.UserId
                && upload.UserName == _command.UserName
                && upload.Size == _command.File.Data.Length
                )));
        }

        private FileUpload GenerateTestFileUpload()
        {
            var result = new FileUpload();

            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write("This is test file content!");
            writer.Flush();
            ms.Position = 0;

            result.Filename = "test.pdf";
            result.Data = ms.ToArray();
            result.ContentType = "application/pdf";

            return result;
        }
    }
}
