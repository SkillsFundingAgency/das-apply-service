using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Appeals.Queries.GetAppealFile;
using SFA.DAS.ApplyService.Data.FileStorage;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.Appeals.GetAppealFileQueryHandlerTests
{
    [TestFixture]
    public class GetAppealFileQueryHandlerTests
    {
        private GetAppealFileQueryHandler _handler;
        private Mock<IAppealFileRepository> _appealFileRepository;
        private Mock<IAppealsFileStorage> _appealFileStorage;
        private readonly Fixture _autoFixture = new Fixture();
        private GetAppealFileQuery _request;
        private AppealFile _appealFile;

        private readonly Guid _applicationId = Guid.NewGuid();
        private readonly Guid _appealFileId = Guid.NewGuid();

        private readonly Guid _fileStorageReference = Guid.NewGuid();
        private string _fileName;
        private string _contentType;
        private byte[] _content;

        [SetUp]
        public void Setup()
        {
            _fileName = _autoFixture.Create<string>();
            _contentType = _autoFixture.Create<string>();
            _content = _autoFixture.Create<byte[]>();

            _request = new GetAppealFileQuery
            {
                ApplicationId = _applicationId,
                FileId = _appealFileId
            };

            _appealFileRepository = new Mock<IAppealFileRepository>();
            _appealFileStorage = new Mock<IAppealsFileStorage>();

            _appealFile = new AppealFile
            {
                Filename = _fileName,
                ContentType = _contentType,
                FileStorageReference = _fileStorageReference,
                ApplicationId = _applicationId,
                Id = _appealFileId,
                CreatedOn = _autoFixture.Create<DateTime>(),
                Size = _autoFixture.Create<int>(),
                UserId = _autoFixture.Create<string>(),
                UserName = _autoFixture.Create<string>()
            };

            _appealFileRepository.Setup(x => x.Get(_appealFileId)).ReturnsAsync(_appealFile);

            _appealFileStorage.Setup(x => x.Get(_applicationId, _fileStorageReference, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_content);

            _handler = new GetAppealFileQueryHandler(_appealFileRepository.Object, _appealFileStorage.Object);
        }

        [Test]
        public async Task Handle_Returns_Requested_Upload()
        {
            var result = await _handler.Handle(_request, CancellationToken.None);

            Assert.AreEqual(_fileName, result.Filename);
            Assert.AreEqual(_contentType, result.ContentType);
            Assert.AreEqual(_content, result.Content);
        }

        [Test]
        public void Handle_Throws_If_Upload_Does_Not_Belong_To_Application()
        {
            _appealFile.ApplicationId = Guid.NewGuid();
            Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(_request, CancellationToken.None));
        }
    }
}
