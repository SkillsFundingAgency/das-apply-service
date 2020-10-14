using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.InternalApi.Services.Files;
using System;
using System.IO;
using System.Text;


namespace SFA.DAS.ApplyService.InternalApi.UnitTests.Services.Files
{
    public class FileEncryptionServiceTests
    {
        private readonly string _fileEncryptionKey = $"{Guid.NewGuid()}";

        private FileEncryptionService _fileEncryptionService;

        [SetUp]
        public void Setup()
        {
            var config = new ApplyConfig { FileStorage = new FileStorageConfig { FileEncryptionKey = _fileEncryptionKey } };
            var _configurationService = new Mock<IConfigurationService>();
            _configurationService.Setup(x => x.GetConfig()).ReturnsAsync(config);

            _fileEncryptionService = new FileEncryptionService(_configurationService.Object);
        }

        [Test]
        public void Encrypt_scrambles_the_input_stream()
        {
            var content = "This is some content I want encrypting";

            using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                var result = _fileEncryptionService.Encrypt(inputStream);

                using (var reader = new StreamReader(result))
                {
                    var encryptedContent = reader.ReadToEnd();
                    StringAssert.AreNotEqualIgnoringCase(encryptedContent, content);
                }
            }
        }

        [Test]
        public void Encrypt_and_Decrypt_are_symetrical()
        {
            var orginialContent = "This is some content I want encrypting";

            using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(orginialContent)))
            {
                var encryptResult = _fileEncryptionService.Encrypt(inputStream);
                var decryptResult = _fileEncryptionService.Decrypt(encryptResult);

                using (var reader = new StreamReader(decryptResult))
                {
                    var content = reader.ReadToEnd();
                    StringAssert.AreEqualIgnoringCase(content, orginialContent);
                }
            }
        }
    }
}
