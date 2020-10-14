using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.InternalApi.Services.Files;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests.Services.Files
{
    public class FileUploadServiceTests
    {
        private const string _fileStorageConnectionString = "UseDevelopmentStorage=true;DevelopmentStorageProxyUri=http://127.0.0.1";
        private const string _fileStorageContainerName = "fileuploadservice-unit-tests";

        private readonly string _nameOfFileToUpload = $"{Guid.NewGuid()}.txt";

        private readonly Guid _applicationId = Guid.NewGuid();
        private readonly int _sequenceNumber = 1;
        private readonly int _sectionNumber = 1;
        private readonly string _pageId = $"{Guid.NewGuid()}";
        private readonly ContainerType _containerType = ContainerType.Moderation;

        private FileUploadService _fileUploadService;

        [SetUp]
        public void Setup()
        {
            var config = new ApplyConfig { FileStorage = new FileStorageConfig { StorageConnectionString = _fileStorageConnectionString, ModerationContainerName = _fileStorageContainerName } };
            var _configurationService = new Mock<IConfigurationService>();
            _configurationService.Setup(x => x.GetConfig()).ReturnsAsync(config);

            var _fileEncryptionService = new Mock<IFileEncryptionService>();
            _fileEncryptionService.Setup(x => x.Decrypt(It.IsAny<Stream>())).Returns<Stream>(x => x); // Don't Decrypt stream
            _fileEncryptionService.Setup(x => x.Encrypt(It.IsAny<Stream>())).Returns<Stream>(x => x); // Don't Encrypt stream

            var fileUploadLogger = Mock.Of<ILogger<FileUploadService>>();
            _fileUploadService = new FileUploadService(fileUploadLogger, _configurationService.Object, _fileEncryptionService.Object);
        }

        [Test]
#if (!DEBUG)
        [Ignore("Must be tested on local DEV machine as it uses local Azure storage")]
#endif
        public async Task UploadFiles_when_all_files_successfully_upload_returns_true()
        {
            var filesToUpload = new FormFileCollection { GenerateFile(_nameOfFileToUpload) };
            var result =  await _fileUploadService.UploadFiles(_applicationId, _sequenceNumber, _sectionNumber, _pageId, filesToUpload, _containerType, new CancellationToken());

            Assert.IsTrue(result);
        }

        [Test]
#if (!DEBUG)
        [Ignore("Must be tested on local DEV machine as it uses local Azure storage")]
#endif
        public async Task UploadFiles_when_no_files_specified_returns_false()
        {
            var filesToUpload = default(FormFileCollection);
            var result = await _fileUploadService.UploadFiles(_applicationId, _sequenceNumber, _sectionNumber, _pageId, filesToUpload, _containerType, new CancellationToken());

            Assert.False(result);
        }

        [Test]
#if (!DEBUG)
        [Ignore("Must be tested on local DEV machine as it uses local Azure storage")]
#endif
        public async Task UploadFiles_when_unknown_ContainerType_specificed_returns_false()
        {
            var filesToUpload = new FormFileCollection { GenerateFile(_nameOfFileToUpload) };
            var result = await _fileUploadService.UploadFiles(_applicationId, _sequenceNumber, _sectionNumber, _pageId, filesToUpload, ContainerType.Unknown, new CancellationToken());

            Assert.False(result);
        }

        private static FormFile GenerateFile(string fileName)
        {
            var content = "This is a dummy file";
            return new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(content)), 0, content.Length, fileName, fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/octet-stream"
            };
        }
    }
}
