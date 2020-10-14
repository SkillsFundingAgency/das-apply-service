using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Services.Files
{
    public interface IFileUploadService
    {
        Task<bool> UploadFiles(Guid applicationId, int? sequenceNumber, int? sectionNumber, string pageId, IFormFileCollection files, ContainerType containerType, CancellationToken cancellationToken);
    }

    public class FileUploadService : IFileUploadService
    {
        private readonly ILogger<FileUploadService> _logger;
        private readonly FileStorageConfig _fileStorageConfig;
        private readonly IFileEncryptionService _fileEncryptionService;

        public FileUploadService(ILogger<FileUploadService> logger, IConfigurationService configurationService, IFileEncryptionService encryptionService)
        {
            _logger = logger;

            var config = configurationService.GetConfig().GetAwaiter().GetResult();
            _fileStorageConfig = config.FileStorage;
            _fileEncryptionService = encryptionService;
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

                        _logger.LogInformation($"All files uploaded to directory: {questionDirectory.Uri}");
                        success = true;
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError($"Error uploading files to directory: {questionDirectory.Uri} || Message: {ex.Message} || Stack trace: {ex.StackTrace}");
                        success = false;
                    }
                }
            }

            return success;
        }
    }
}
