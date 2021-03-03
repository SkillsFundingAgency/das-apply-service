using System.Text;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Data.FileStorage;

namespace SFA.DAS.ApplyService.Data.UnitTests.FileStorage
{
    [TestFixture]
    public class ByteArrayEncryptionServiceTests
    {
        private ByteArrayEncryptionService _byteArrayEncryptionService;
        private Mock<IConfigurationService> _configurationService;
        private FileStorageConfig _config;
        private string _originalContent;

        [SetUp]
        public void SetUp()
        {
            var autoFixture = new Fixture();
            _originalContent = autoFixture.Create<string>();

            _config = new FileStorageConfig
            {
                FileEncryptionKey = autoFixture.Create<string>()
            };

            _configurationService = new Mock<IConfigurationService>();
            _configurationService.Setup(x => x.GetConfig()).ReturnsAsync(() => new ApplyConfig {FileStorage = _config});

            _byteArrayEncryptionService = new ByteArrayEncryptionService(_configurationService.Object);
        }

        [Test]
        public void Encrypt_modifies_the_original_content()
        {
            var bytes = Encoding.UTF8.GetBytes(_originalContent);
            var encryptedBytes = _byteArrayEncryptionService.Encrypt(bytes);
            var result = Encoding.UTF8.GetString(encryptedBytes);
            StringAssert.AreNotEqualIgnoringCase(result, _originalContent);
        }

        [Test]
        public void Decrypt_encrypted_string_returns_original_value()
        {
            var bytes = Encoding.UTF8.GetBytes(_originalContent);
            var interim = _byteArrayEncryptionService.Encrypt(bytes);
            var decryptedBytes= _byteArrayEncryptionService.Decrypt(interim);
            var result = Encoding.UTF8.GetString(decryptedBytes);
            StringAssert.AreEqualIgnoringCase(_originalContent, result);
        }
    }
}
