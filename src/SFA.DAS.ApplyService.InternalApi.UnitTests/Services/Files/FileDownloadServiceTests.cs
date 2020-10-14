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
    public class FileDownloadServiceTests
    {
        private const string _fileStorageConnectionString = "UseDevelopmentStorage=true;DevelopmentStorageProxyUri=http://127.0.0.1";
        private const string _fileStorageContainerName = "filedownloadservice-unit-tests";

        private readonly string _nameOfFileThatExists = $"{Guid.NewGuid()}.txt";
        private readonly string _nameOfFileThatExists2 = $"{Guid.NewGuid()}.txt";

        private readonly Guid _applicationId = Guid.NewGuid();
        private readonly int _sequenceNumber = 1;
        private readonly int _sectionNumber = 1;
        private readonly string _pageId = $"{Guid.NewGuid()}";
        private readonly ContainerType _containerType = ContainerType.Moderation;

        private FileUploadService _fileUploadService;
        private FileDownloadService _fileDownloadService;

        [SetUp]
        public async Task Setup()
        {
            var config = new ApplyConfig { FileStorage = new FileStorageConfig { StorageConnectionString = _fileStorageConnectionString, ModerationContainerName = _fileStorageContainerName } };
            var _configurationService = new Mock<IConfigurationService>();
            _configurationService.Setup(x => x.GetConfig()).ReturnsAsync(config);

            var _fileEncryptionService = new Mock<IFileEncryptionService>();
            _fileEncryptionService.Setup(x => x.Decrypt(It.IsAny<Stream>())).Returns<Stream>(x => x); // Don't Decrypt stream
            _fileEncryptionService.Setup(x => x.Encrypt(It.IsAny<Stream>())).Returns<Stream>(x => x); // Don't Encrypt stream

            var fileUploadLogger = Mock.Of<ILogger<FileUploadService>>();
            _fileUploadService = new FileUploadService(fileUploadLogger, _configurationService.Object, _fileEncryptionService.Object);

            var filesThatWillExist = new FormFileCollection { GenerateFile(_nameOfFileThatExists), GenerateFile(_nameOfFileThatExists2) };
            await _fileUploadService.UploadFiles(_applicationId, _sequenceNumber, _sectionNumber, _pageId, filesThatWillExist, _containerType, new CancellationToken());

            _fileDownloadService = new FileDownloadService(_configurationService.Object, _fileEncryptionService.Object);
        }

        [Test]
#if (!DEBUG)
        [Ignore("Must be tested on local DEV machine as it uses local Azure storage")]
#endif
        public async Task DownloadFile_when_file_exists_Then_it_returns_the_file()
        {
            var result = await _fileDownloadService.DownloadFile(_applicationId, _sequenceNumber, _sectionNumber, _pageId, _nameOfFileThatExists, _containerType, new CancellationToken());

            Assert.IsNotNull(result);
            StringAssert.AreEqualIgnoringCase(_nameOfFileThatExists, result.FileName);
        }

        [Test]
#if (!DEBUG)
        [Ignore("Must be tested on local DEV machine as it uses local Azure storage")]
#endif
        public async Task DownloadFile_when_page_does_not_exists_Then_it_returns_null()
        {
            var nameOfPageThatDoesNotExist = $"{Guid.NewGuid()}";
            var result = await _fileDownloadService.DownloadFile(_applicationId, _sequenceNumber, _sectionNumber, nameOfPageThatDoesNotExist, _nameOfFileThatExists, _containerType, new CancellationToken());

            Assert.IsNull(result);
        }

        [Test]
#if (!DEBUG)
        [Ignore("Must be tested on local DEV machine as it uses local Azure storage")]
#endif
        public async Task DownloadFile_when_file_does_not_exists_Then_it_returns_null()
        {
            var nameOfFileThatDoesNotExist = $"{Guid.NewGuid()}.txt";
            var result = await _fileDownloadService.DownloadFile(_applicationId, _sequenceNumber, _sectionNumber, _pageId, nameOfFileThatDoesNotExist, _containerType, new CancellationToken());

            Assert.IsNull(result);
        }

        [Test]
#if (!DEBUG)
        [Ignore("Must be tested on local DEV machine as it uses local Azure storage")]
#endif
        public async Task DownloadPageFiles_when_page_exists_Then_it_returns_the_files_as_a_zip()
        {
            var result = await _fileDownloadService.DownloadPageFiles(_applicationId, _sequenceNumber, _sectionNumber, _pageId, _containerType, new CancellationToken());

            Assert.IsNotNull(result);
            StringAssert.EndsWith(".zip", result.FileName);
            StringAssert.AreEqualIgnoringCase("application/zip", result.ContentType);
        }

        [Test]
#if (!DEBUG)
        [Ignore("Must be tested on local DEV machine as it uses local Azure storage")]
#endif
        public async Task DownloadPageFiles_when_page_does_not_exist_Then_it_returns_null()
        {
            var nameOfPageThatDoesNotExist = $"{Guid.NewGuid()}";
            var result = await _fileDownloadService.DownloadPageFiles(_applicationId, _sequenceNumber, _sectionNumber, nameOfPageThatDoesNotExist, _containerType, new CancellationToken());

            Assert.IsNull(result);
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
