using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Configuration;

namespace SFA.DAS.ApplyService.Storage
{
    public class StorageService : IStorageService
    {
        private readonly IConfigurationService _configurationService;

        public StorageService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }
        
        public async Task<string> Store(string applicationId, string pageId, string questionId, string fileName, Stream fileStream)
        {
            var config = await _configurationService.GetConfig();

            var account = CloudStorageAccount.Parse(config.FileStorage.StorageConnectionString);
            var client = account.CreateCloudBlobClient();
            var container = client.GetContainerReference(config.FileStorage.ContainerName);
            await container.CreateIfNotExistsAsync();

            var applicationFolder = container.GetDirectoryReference(applicationId);
            var pageFolder = applicationFolder.GetDirectoryReference(pageId.ToLower());
            var questionFolder = pageFolder.GetDirectoryReference(questionId.ToLower());
            
            var blob = questionFolder.GetBlockBlobReference(fileName);

            await blob.UploadFromStreamAsync(fileStream);

            return fileName;
        }

        public async Task<Tuple<string, Stream>> Retrieve(string applicationId, string pageId, string questionId,
            string filename)
        {
            var config = await _configurationService.GetConfig();

            var account = CloudStorageAccount.Parse(config.FileStorage.StorageConnectionString);
            var client = account.CreateCloudBlobClient();
            var container = client.GetContainerReference(config.FileStorage.ContainerName);
            await container.CreateIfNotExistsAsync();

            var applicationFolder = container.GetDirectoryReference(applicationId);
            var pageFolder = applicationFolder.GetDirectoryReference(pageId.ToLower());
            var questionFolder = pageFolder.GetDirectoryReference(questionId.ToLower());

            var blob = questionFolder.GetBlobReference(filename);
            
            
            var ms = new MemoryStream();

            await blob.DownloadToStreamAsync(ms);
            ms.Position = 0;
            return new Tuple<string, Stream>(filename, ms);
        }
    }
}