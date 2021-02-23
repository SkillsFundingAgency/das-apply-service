using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using SFA.DAS.ApplyService.Domain.Models;

namespace SFA.DAS.ApplyService.Data.FileStorage
{
    public abstract class FileStorage
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IByteArrayEncryptionService _byteArrayEncryptionService;

        protected FileStorage(BlobServiceClient blobServiceClient, IByteArrayEncryptionService byteArrayEncryptionService)
        {
            _blobServiceClient = blobServiceClient;
            _byteArrayEncryptionService = byteArrayEncryptionService;
        }

        protected abstract string ContainerName { get; }

        protected async Task<BlobContainerClient> GetClient()
        {
            var result = _blobServiceClient.GetBlobContainerClient(ContainerName);
            await result.CreateIfNotExistsAsync();
            return result;
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
