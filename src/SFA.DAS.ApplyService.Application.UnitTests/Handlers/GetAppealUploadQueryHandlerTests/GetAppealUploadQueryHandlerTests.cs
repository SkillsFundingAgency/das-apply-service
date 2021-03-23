using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetAppealUpload;
using SFA.DAS.ApplyService.Data.FileStorage;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.GetAppealUploadQueryHandlerTests
{
    [TestFixture]
    public class GetAppealUploadQueryHandlerTests
    {
        private GetAppealUploadQueryHandler _handler;
        private Mock<IAppealUploadRepository> _appealUploadRepository;
        private Mock<IAppealsFileStorage> _appealFileStorage;
        private readonly Fixture _autoFixture = new Fixture();
        private GetAppealUploadQuery _request;
        private AppealUpload _appealUpload;

        private readonly Guid _applicationId = Guid.NewGuid();
        private readonly Guid _oversightReviewId = Guid.NewGuid();
        private readonly Guid _appealId = Guid.NewGuid();
        private readonly Guid _appealUploadId = Guid.NewGuid();

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

            _request = new GetAppealUploadQuery
            {
                ApplicationId = _applicationId,
                AppealId = _appealId,
                AppealUploadId = _appealUploadId
            };

            _appealUploadRepository = new Mock<IAppealUploadRepository>();
            _appealFileStorage = new Mock<IAppealsFileStorage>();

            _appealUpload = new AppealUpload
            {
                Filename = _fileName,
                ContentType = _contentType,
                FileStorageReference = _fileStorageReference,
                AppealId = _appealId,
                ApplicationId = _applicationId,
                Id = _appealUploadId,
                CreatedOn = _autoFixture.Create<DateTime>(),
                Size = _autoFixture.Create<int>(),
                UserId = _autoFixture.Create<string>(),
                UserName = _autoFixture.Create<string>()
            };

            _appealUploadRepository.Setup(x => x.GetById(_appealUploadId)).ReturnsAsync(_appealUpload);

            _appealFileStorage.Setup(x => x.Get(_applicationId, _fileStorageReference, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_content);

            _handler = new GetAppealUploadQueryHandler(_appealUploadRepository.Object, _appealFileStorage.Object);
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
            _appealUpload.ApplicationId = Guid.NewGuid();
            Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(_request, CancellationToken.None));
        }

        [Test]
        public void Handle_Throws_If_Upload_Does_Not_Belong_To_Appeal()
        {
            _appealUpload.AppealId = Guid.NewGuid();
            Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(_request, CancellationToken.None));
        }
    }
}