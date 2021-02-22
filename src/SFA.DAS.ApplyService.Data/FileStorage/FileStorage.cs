using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Models;

namespace SFA.DAS.ApplyService.Data.FileStorage
{
    public abstract class FileStorage
    {
        private readonly IByteArrayEncryptionService _byteArrayEncryptionService;
        protected FileStorageConfig _config;

        protected FileStorage(IConfigurationService configurationService, IByteArrayEncryptionService byteArrayEncryptionService)
        {
            _byteArrayEncryptionService = byteArrayEncryptionService;
            var config = configurationService.GetConfig().GetAwaiter().GetResult();
            _config = config.FileStorage;
        }

        protected abstract string ContainerName { get; }

        protected async Task<BlobContainerClient> GetClient()
        {
            var blobServiceClient = new BlobServiceClient(_config.StorageConnectionString);
            var container = blobServiceClient.GetBlobContainerClient(ContainerName);
            await container.CreateIfNotExistsAsync();
            return container;
        }

        protected async Task<Guid> AddFileToContainer(string path, FileUpload file, CancellationToken cancellationToken)
        {
            var client = await GetClient();

            var reference = Guid.NewGuid();

            path = RemoveTrailingDelimiter(path);

            var blobName = string.IsNullOrWhiteSpace(path) ? reference.ToString() : $"{path}/{reference}";

            var encryptedBytes = _byteArrayEncryptionService.Encrypt(file.Data);

            await client.UploadBlobAsync(blobName, new MemoryStream(encryptedBytes), cancellationToken);

            return reference;
        }

        protected async Task RemoveFileFromContainer(string path, Guid reference)
        {
            var client = await GetClient();

            path = RemoveTrailingDelimiter(path);

            var blobName = string.IsNullOrWhiteSpace(path) ? reference.ToString() : $"{path}/{reference}";

            await client.DeleteBlobIfExistsAsync(blobName);
        }

        private string RemoveTrailingDelimiter(string path)
        {
            return path.EndsWith("/") ? path.Substring(0, path.Length - 1) : path;
        }
    }
}
