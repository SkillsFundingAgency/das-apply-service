using System.IO;
using System.Security.Cryptography;
using System.Text;
using SFA.DAS.ApplyService.Configuration;

namespace SFA.DAS.ApplyService.Data.FileStorage
{
    public class ByteArrayEncryptionService : IByteArrayEncryptionService
    {
        private readonly FileStorageConfig _fileStorageConfig;

        public ByteArrayEncryptionService(IConfigurationService configurationService)
        {
            var config = configurationService.GetConfig().GetAwaiter().GetResult();
            _fileStorageConfig = config.FileStorage;
        }

        public byte[] Encrypt(byte[] bytes)
        {
            var key = _fileStorageConfig.FileEncryptionKey;
            return AES_Encrypt(bytes, Encoding.ASCII.GetBytes(key));
        }

        public byte[] Decrypt(byte[] encryptedBytes)
        {
            var key = _fileStorageConfig.FileEncryptionKey;
            return AES_Decrypt(encryptedBytes, Encoding.ASCII.GetBytes(key));
        }


        private byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes;
            using (var memoryStream = new MemoryStream())
            {
                using (var aes = Aes.Create())
                {
                    ConfigureAes(passwordBytes, aes);
                    using (var cs = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = memoryStream.ToArray();
                }
            }
            return encryptedBytes;
        }

        private byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes;
            using (var memoryStream = new MemoryStream())
            {
                using (var aes = Aes.Create())
                {
                    ConfigureAes(passwordBytes, aes);
                    using (var cs = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = memoryStream.ToArray();
                }
            }
            return decryptedBytes;
        }

        private static void ConfigureAes(byte[] passwordBytes, Aes aes)
        {
            using (var key = new Rfc2898DeriveBytes(passwordBytes, new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }, 1000))
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = key.GetBytes(aes.BlockSize / 8);
                aes.Mode = CipherMode.CBC;
            }
        }
    }
}
