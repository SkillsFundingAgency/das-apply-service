using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Services.Files
{
    public class FileStorageService : IFileStorageService
    {
        private readonly ILogger<FileStorageService> _logger;
        private readonly FileStorageConfig _fileStorageConfig;
        private readonly IFileEncryptionService _fileEncryptionService;

        public FileStorageService(ILogger<FileStorageService> logger, IConfigurationService configurationService, IFileEncryptionService encryptionService)
        {
            _logger = logger;

            var config = configurationService.GetConfig().GetAwaiter().GetResult();
            _fileStorageConfig = config.FileStorage;
            _fileEncryptionService = encryptionService;
        }

        public async Task<IEnumerable<string>> GetPageFileList(Guid applicationId, int? sequenceNumber, int? sectionNumber, string pageId, ContainerType containerType, CancellationToken cancellationToken)
        {
            var fileList = new List<string>();

            if (!string.IsNullOrWhiteSpace(pageId))
            {
                var container = await BlobContainerHelpers.GetContainer(_fileStorageConfig, containerType);

                if (container != null)
                {
                    var pageDirectory = container.GetDirectory(applicationId, sequenceNumber, sectionNumber, pageId);
                    var fileBlobs = pageDirectory.ListBlobs(useFlatBlobListing: true).ToList();

                    foreach (var blob in fileBlobs.OfType<CloudBlob>())
                    {
                        string fileName = Path.GetFileName(blob.Name);
                        fileList.Add(fileName);
                    }
                }
            }

            return fileList;
        }

        public async Task<bool> UploadFiles(Guid applicationId, int? sequenceNumber, int? sectionNumber, string pageId, IFormFileCollection files, ContainerType containerType, CancellationToken cancellationToken)
        {
            var success = false;

            if (!string.IsNullOrWhiteSpace(pageId) && files != null)
            {
                var container = await BlobContainerHelpers.GetContainer(_fileStorageConfig, containerType);

                if (container != null)
                {
                    var questionDirectory = container.GetDirectory(applicationId, sequenceNumber, sectionNumber, pageId);

                    try
                    {
                        foreach (var file in files)
                        {
                            var blob = questionDirectory.GetBlockBlobReference(file.FileName);
                            blob.Properties.ContentType = file.ContentType;

                            var encryptedFileStream = _fileEncryptionService.Encrypt(file.OpenReadStream());

                            await blob.UploadFromStreamAsync(encryptedFileStream, cancellationToken);
                        }

                        success = true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error uploading files to directory: {questionDirectory.Uri} || Message: {ex.Message} || Stack trace: {ex.StackTrace}");
                        success = false;
                    }
                }
            }

            return success;
        }

        public async Task<bool> DeleteFile(Guid applicationId, int? sequenceNumber, int? sectionNumber, string pageId, string fileName, ContainerType containerType, CancellationToken cancellationToken)
        {
            var success = false;

            if (!string.IsNullOrWhiteSpace(pageId) && !string.IsNullOrWhiteSpace(fileName))
            {
                var container = await BlobContainerHelpers.GetContainer(_fileStorageConfig, containerType);

                if (container != null)
                {
                    var pageDirectory = container.GetDirectory(applicationId, sequenceNumber, sectionNumber, pageId);
                    var pageFileBlob = pageDirectory.GetBlobReference(fileName);

                    success = await pageFileBlob.DeleteIfExistsAsync(cancellationToken);
                }
            }

            return success;
        }

        public async Task<DownloadFile> DownloadFile(Guid applicationId, int? sequenceNumber, int? sectionNumber, string pageId, string fileName, ContainerType containerType, CancellationToken cancellationToken)
        {
            var file = default(DownloadFile);

            if (!string.IsNullOrWhiteSpace(pageId) && !string.IsNullOrWhiteSpace(fileName))
            {
                var container = await BlobContainerHelpers.GetContainer(_fileStorageConfig, containerType);

                if (container != null)
                {
                    var pageDirectory = container.GetDirectory(applicationId, sequenceNumber, sectionNumber, pageId);
                    var pageFileBlob = pageDirectory.GetBlobReference(fileName);

                    if (pageFileBlob.Exists())
                    {
                        file = await DownloadFileFromBlob(pageFileBlob, cancellationToken);
                    }
                }
            }

            return file;
        }

        public async Task<DownloadFile> DownloadPageFiles(Guid applicationId, int? sequenceNumber, int? sectionNumber, string pageId, ContainerType containerType, CancellationToken cancellationToken)
        {
            var zipFile = default(DownloadFile);

            if (!string.IsNullOrWhiteSpace(pageId))
            {
                var container = await BlobContainerHelpers.GetContainer(_fileStorageConfig, containerType);

                if (container != null)
                {
                    var pageDirectory = container.GetDirectory(applicationId, sequenceNumber, sectionNumber, pageId);
                    var files = await DownloadFilesFromDirectory(pageDirectory, cancellationToken);

                    if (files.Any())
                    {
                        zipFile = ZipDownloadFiles(files, $"{pageId}_uploads.zip");
                    }
                }
            }

            return zipFile;
        }

        private async Task<DownloadFile> DownloadFileFromBlob(CloudBlob blob, CancellationToken cancellationToken)
        {
            var blobStream = new MemoryStream();

            await blob.DownloadToStreamAsync(blobStream, null, new BlobRequestOptions() { DisableContentMD5Validation = true }, null, cancellationToken);
            blobStream.Position = 0;

            var decryptedStream = _fileEncryptionService.Decrypt(blobStream);

            return new DownloadFile { FileName = Path.GetFileName(blob.Name), ContentType = blob.Properties.ContentType, Stream = decryptedStream };
        }

        private async Task<List<DownloadFile>> DownloadFilesFromDirectory(CloudBlobDirectory directory, CancellationToken cancellationToken)
        {
            var questionFileBlobs = directory.ListBlobs(useFlatBlobListing: true).ToList();

            var downloadFiles = new List<DownloadFile>(questionFileBlobs.Count);

            foreach (var blob in questionFileBlobs.OfType<CloudBlob>())
            {
                var downloadFile = await DownloadFileFromBlob(blob, cancellationToken);
                downloadFiles.Add(downloadFile);
            }

            return downloadFiles;
        }

        private DownloadFile ZipDownloadFiles(List<DownloadFile> files, string zipFileName)
        {
            using (var zipStream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in files)
                    {
                        var zipEntry = zipArchive.CreateEntry(file.FileName);
                        using (var entryStream = zipEntry.Open())
                        {
                            file.Stream.CopyTo(entryStream);
                        }
                    }
                }

                zipStream.Position = 0;
                var newStream = new MemoryStream();
                zipStream.CopyTo(newStream);
                newStream.Position = 0;

                return new DownloadFile { FileName = zipFileName ?? "uploads.zip", ContentType = "application/zip", Stream = newStream };
            }
        }
    }
}
