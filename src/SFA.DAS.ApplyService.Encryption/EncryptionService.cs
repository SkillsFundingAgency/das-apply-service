﻿using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Configuration;

namespace SFA.DAS.ApplyService.Encryption
{
    public class EncryptionService : IEncryptionService
    {
        private readonly IKeyProvider _keyProvider;

        public EncryptionService(IKeyProvider keyProvider)
        {
            _keyProvider = keyProvider;
        }
        
        public async Task<Stream> Encrypt(Stream fileStream)
        {   
            var key = await _keyProvider.GetKey();

            var memoryStream = new MemoryStream(); 
            fileStream.CopyTo(memoryStream);

            var originalBytes = memoryStream.ToArray();

            var encryptedBytes = AES_Encrypt(originalBytes, Encoding.ASCII.GetBytes(key));
            
            return new MemoryStream(encryptedBytes);
        }

        public async Task<Stream> Decrypt(Stream encryptedFileStream)
        {
            var key = await _keyProvider.GetKey();
            
            var memoryStream = new MemoryStream(); 
            encryptedFileStream.CopyTo(memoryStream);

            var encryptedBytes = memoryStream.ToArray();
            
            var originalBytes = AES_Decrypt(encryptedBytes, Encoding.ASCII.GetBytes(key));
            
            return new MemoryStream(originalBytes);
        }


        private byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes;
            using (var ms = new MemoryStream())
            {
                using (var aes = Aes.Create())
                {
                    ConfigureAes(passwordBytes, aes);
                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }
            return encryptedBytes;
        }

        private byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes;
            using (var ms = new MemoryStream())
            {
                using (var aes = Aes.Create())
                {
                    ConfigureAes(passwordBytes, aes);
                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }
            return decryptedBytes;
        }

        private static void ConfigureAes(byte[] passwordBytes, Aes aes)
        {
            aes.KeySize = 256;
            aes.BlockSize = 128;
            var key = new Rfc2898DeriveBytes(passwordBytes, new byte[] {1, 2, 3, 4, 5, 6, 7, 8}, 1000);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Mode = CipherMode.CBC;
        }
    }
    
    public class PlaceholderKeyProvider : IKeyProvider
    {
        private readonly IConfigurationService _configurationService;

        public PlaceholderKeyProvider(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }
        
        public async Task<string> GetKey()
        {
            var config = await _configurationService.GetConfig();
            var key = config.FileStorage.FileEncryptionKey;

            return key;
        }
    }
}