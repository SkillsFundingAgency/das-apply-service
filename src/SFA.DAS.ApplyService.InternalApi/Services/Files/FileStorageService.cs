using Microsoft.AspNetCore.Http;
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

        public FileStorageService(ILogger<FileStorageService> logger, IConfigurationService configurationService)
        {
            _logger = logger;

            var config = configurationService.GetConfig().GetAwaiter().GetResult();
            _fileStorageConfig = config.FileStorage;
        }

        public async Task<IEnumerable<DownloadFileInfo>> GetApplicationFileList(Guid applicationId, ContainerType containerType, CancellationToken cancellationToken)
        {
            var fileList = new List<DownloadFileInfo>();

            var container = await BlobContainerHelpers.GetContainer(_fileStorageConfig, containerType);

            if (container != null)
            {
                var applicationDirectory = container.GetDirectory(applicationId);
                var directoryFileList = applicationDirectory.GetFileList();
                fileList.AddRange(directoryFileList);
            }

            return fileList;
        }

        public async Task<IEnumerable<DownloadFileInfo>> GetFileList(Guid applicationId, int? sequenceNumber, int? sectionNumber, string pageId, ContainerType containerType, CancellationToken cancellationToken)
        {
            var fileList = new List<DownloadFileInfo>();

            if (!string.IsNullOrWhiteSpace(pageId))
            {
                var container = await BlobContainerHelpers.GetContainer(_fileStorageConfig, containerType);

                if (container != null)
                {
                    var pageDirectory = container.GetDirectory(applicationId, sequenceNumber, sectionNumber, pageId);
                    var directoryFileList = pageDirectory.GetFileList();
                    fileList.AddRange(directoryFileList);
                }
            }

            return fileList;
        }

        public async Task<bool> UploadApplicationFiles(Guid applicationId, IFormFileCollection files, ContainerType containerType, CancellationToken cancellationToken)
        {
            var success = false;

            if (files != null)
            {
                var container = await BlobContainerHelpers.GetContainer(_fileStorageConfig, containerType);

                if (container != null)
                {
                    var applicationDirectory = container.GetDirectory(applicationId);
                    success = await applicationDirectory.UploadFiles(files, _logger, cancellationToken);
                }
            }

            return success;
        }

        public async Task<bool> UploadFiles(Guid applicationId, int? sequenceNumber, int? sectionNumber, string pageId, IFormFileCollection files, ContainerType containerType, CancellationToken cancellationToken)
        {
            var success = false;

            if (!string.IsNullOrWhiteSpace(pageId) && files != null)
            {
                var container = await BlobContainerHelpers.GetContainer(_fileStorageConfig, containerType);

                if (container != null)
                {
                    var pageDirectory = container.GetDirectory(applicationId, sequenceNumber, sectionNumber, pageId);
                    success = await pageDirectory.UploadFiles(files, _logger, cancellationToken);
                }
            }

            return success;
        }

        public async Task<bool> DeleteApplicationFile(Guid applicationId, string fileName, ContainerType containerType, CancellationToken cancellationToken)
        {
            var success = false;

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var container = await BlobContainerHelpers.GetContainer(_fileStorageConfig, containerType);

                if (container != null)
                {
                    var applicationDirectory = container.GetDirectory(applicationId);
                    success = await applicationDirectory.DeleteFile(fileName, cancellationToken);
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
                    success = await pageDirectory.DeleteFile(fileName, cancellationToken);
                }
            }

            return success;
        }

        public async Task<DownloadFile> DownloadApplicationFile(Guid applicationId, string fileName, ContainerType containerType, CancellationToken cancellationToken)
        {
            var file = default(DownloadFile);

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var container = await BlobContainerHelpers.GetContainer(_fileStorageConfig, containerType);

                if (container != null)
                {
                    var applicationDirectory = container.GetDirectory(applicationId);
                    file = await applicationDirectory.DownloadFile(fileName, cancellationToken);
                }
            }

            return file;
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
                    file = await pageDirectory.DownloadFile(fileName, cancellationToken);
                }
            }

            return file;
        }

        public async Task<DownloadFile> DownloadApplicationFiles(Guid applicationId, ContainerType containerType, CancellationToken cancellationToken)
        {
            var zipFile = default(DownloadFile);

            var container = await BlobContainerHelpers.GetContainer(_fileStorageConfig, containerType);

            if (container != null)
            {
                var applicationDirectory = container.GetDirectory(applicationId);
                var files = await applicationDirectory.DownloadDirectoryFiles(cancellationToken);

                if (files.Any())
                {
                    zipFile = ZipDownloadFiles(files, $"uploads.zip");
                }
            }

            return zipFile;
        }

        public async Task<DownloadFile> DownloadFiles(Guid applicationId, int? sequenceNumber, int? sectionNumber, string pageId, ContainerType containerType, CancellationToken cancellationToken)
        {
            var zipFile = default(DownloadFile);

            if (!string.IsNullOrWhiteSpace(pageId))
            {
                var container = await BlobContainerHelpers.GetContainer(_fileStorageConfig, containerType);

                if (container != null)
                {
                    var pageDirectory = container.GetDirectory(applicationId, sequenceNumber, sectionNumber, pageId);
                    var files = await pageDirectory.DownloadDirectoryFiles(cancellationToken);

                    if (files.Any())
                    {
                        zipFile = ZipDownloadFiles(files, $"{pageId}_uploads.zip");
                    }
                }
            }

            return zipFile;
        }

        private DownloadFile ZipDownloadFiles(IEnumerable<DownloadFile> files, string zipFileName)
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

                return new DownloadFile { FileName = zipFileName, ContentType = "application/zip", Stream = newStream };
            }
        }
    }
}
