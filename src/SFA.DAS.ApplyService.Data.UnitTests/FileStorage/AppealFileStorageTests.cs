using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Azure.Storage.Blobs;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Data.FileStorage;
using SFA.DAS.ApplyService.Domain.Models;

namespace SFA.DAS.ApplyService.Data.UnitTests.FileStorage
{
    [TestFixture]
    public class AppealFileStorageTests
    {
        private Mock<BlobServiceClient> _blobServiceClient;
        private Mock<BlobContainerClient> _blobContainerClient;
        private AppealFileStorage _appealFileStorage;
        private Mock<IByteArrayEncryptionService> _byteArrayEncryptionService;
        private byte[] _encryptionServiceResult;
        private string _capturedBlobName;
        private Stream _capturedUploadStream;

        [SetUp]
        public void Setup()
        {
            var autoFixture = new Fixture();
            _encryptionServiceResult = autoFixture.Create<byte[]>();

            _blobServiceClient = new Mock<BlobServiceClient>();
            _blobContainerClient = new Mock<BlobContainerClient>();

            _blobServiceClient.Setup(x => x.GetBlobContainerClient(It.IsAny<string>()))
                .Returns(_blobContainerClient.Object);

            _blobContainerClient.Setup(x =>
                    x.UploadBlobAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                    .Callback<string, Stream, CancellationToken>((blobName, content, token) =>
                        {
                            _capturedUploadStream = content;
                            _capturedBlobName = blobName;
                        });


            _byteArrayEncryptionService = new Mock<IByteArrayEncryptionService>();
            _byteArrayEncryptionService.Setup(x => x.Encrypt(It.IsAny<byte[]>())).Returns(_encryptionServiceResult);

            _appealFileStorage = new AppealFileStorage(_blobServiceClient.Object,
                _byteArrayEncryptionService.Object);
        }

        [Test]
        public async Task Add_Uploads_File_In_Blob_Storage()
        {
            await _appealFileStorage.Add(Guid.NewGuid(), new FileUpload(), new CancellationToken());
            _blobContainerClient.Verify(x => x.UploadBlobAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task Add_Encrypts_File()
        {
            await _appealFileStorage.Add(Guid.NewGuid(), new FileUpload(), new CancellationToken());

            var capturedBytes = ReadFully(_capturedUploadStream);
            Assert.AreEqual(_encryptionServiceResult, capturedBytes);
        }

        [Test]
        public async Task Add_Returns_Reference_Of_Uploaded_File()
        {
            var result = await _appealFileStorage.Add(Guid.NewGuid(), new FileUpload(), new CancellationToken());
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Add_Uploads_File_Within_ApplicationId_Subfolder()
        {
            var applicationId = Guid.NewGuid();
            var result = await _appealFileStorage.Add(applicationId, new FileUpload(), new CancellationToken());
            var expectedBlobName = $"{applicationId}/{result}";
            Assert.AreEqual(expectedBlobName, _capturedBlobName);
        }

        private static byte[] ReadFully(Stream input)
        {
            using MemoryStream ms = new MemoryStream();
            input.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
