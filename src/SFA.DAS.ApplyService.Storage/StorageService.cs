using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Storage
{
    public class StorageService : IStorageService
    {
        private readonly ILogger<StorageService> _logger;
        private readonly IConfigurationService _configurationService;

        public StorageService(ILogger<StorageService> logger, IConfigurationService configurationService)
        {
            _logger = logger;
            _configurationService = configurationService;
        }

        public async Task<string> Store(string applicationId, int sequenceId, int sectionId, string pageId, string questionId, string filename, Stream fileStream, string fileContentType)
        {
            try
            {
                var container = await GetContainer();
                var questionFolder = GetDirectory(applicationId, sequenceId, sectionId, pageId, questionId, container);

                var blob = questionFolder.GetBlockBlobReference(filename);
                blob.Properties.ContentType = fileContentType;
                await blob.UploadFromStreamAsync(fileStream);

                if(blob?.Uri?.IsAbsoluteUri is true)
                {
                    return Path.GetFileName(blob.Uri.LocalPath);
                }

                return filename;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to store file within the blob");
                throw;
            }
        }

        public async Task<Tuple<string, Stream, string>> Retrieve(string applicationId, int sequenceId, int sectionId, string pageId, string questionId, string filename)
        {
            try
            {
                var container = await GetContainer();
                var questionFolder = GetDirectory(applicationId, sequenceId, sectionId, pageId, questionId, container);

                var ms = new MemoryStream();
                var blob = questionFolder.GetBlobReference(filename);
                await blob.DownloadToStreamAsync(ms, null, new BlobRequestOptions() { DisableContentMD5Validation = true }, null);
                ms.Position = 0;

                return new Tuple<string, Stream, string>(filename, ms, blob.Properties.ContentType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to retrieve file within the blob");
                throw;
            }
        }

        public async Task Delete(Guid applicationId, int sequenceId, int sectionId, string pageId, string questionId, string filename)
        {
            var container = await GetContainer();
            var questionFolder = GetDirectory(applicationId.ToString(), sequenceId, sectionId, pageId, questionId, container);

            var blob = questionFolder.GetBlobReference(filename);
            await blob.DeleteIfExistsAsync();
        }

        private async Task<CloudBlobContainer> GetContainer()
        {
            var config = await _configurationService.GetConfig();

            var account = CloudStorageAccount.Parse(config.FileStorage.StorageConnectionString);
            var client = account.CreateCloudBlobClient();

            var container = client.GetContainerReference(config.FileStorage.ContainerName);
            await container.CreateIfNotExistsAsync();

            return container;
        }

        private static CloudBlobDirectory GetDirectory(string applicationId, int sequenceId, int sectionId, string pageId, string questionId, CloudBlobContainer container)
        {
            var applicationFolder = container.GetDirectoryReference(applicationId);
            var sequenceFolder = applicationFolder.GetDirectoryReference(sequenceId.ToString());
            var sectionFolder = sequenceFolder.GetDirectoryReference(sectionId.ToString());
            var pageFolder = sectionFolder.GetDirectoryReference(pageId.ToLower());
            var questionFolder = pageFolder.GetDirectoryReference(questionId.ToLower());
            return questionFolder;
        }
    }
}