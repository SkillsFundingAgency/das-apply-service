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
        
        public async Task<string> Store(string applicationId, int sequenceId, int sectionId, string pageId, string questionId, string fileName, Stream fileStream, string fileContentType)
        {
            var container = await GetContainer();

            var questionFolder = GetDirectory(applicationId, sequenceId, sectionId, pageId, questionId, container);
            
            var blob = questionFolder.GetBlockBlobReference(fileName);
            blob.Properties.ContentType = fileContentType;

            try
            {
                await blob.UploadFromStreamAsync(fileStream);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            

            return fileName;
        }

        public async Task<Tuple<string, Stream, string>> Retrieve(string applicationId, int sequenceId, int sectionId, string pageId, string questionId,
            string filename)
        {
            var container = await GetContainer();

            var questionFolder = GetDirectory(applicationId, sequenceId, sectionId, pageId, questionId, container);

            var blob = questionFolder.GetBlobReference(filename);
            
            var ms = new MemoryStream();

            await blob.DownloadToStreamAsync(ms,null,new BlobRequestOptions(){DisableContentMD5Validation = true},null);
            ms.Position = 0;
            return new Tuple<string, Stream, string>(filename, ms, blob.Properties.ContentType);
        }

        public async Task Delete(Guid applicationId, int sequenceId, int sectionId, string pageId, string questionId, string filename)
        {
            var container = await GetContainer();
            var questionFolder = GetDirectory(applicationId.ToString(), sequenceId, sectionId, pageId, questionId, container);
            var blob = questionFolder.GetBlobReference(filename);
            await blob.DeleteAsync();   
        }

        public async Task<bool> Exists(string applicationId, int sequenceId, int sectionId, string pageId, string questionId, string filename)
        {
            var container = await GetContainer();
            var questionFolder = GetDirectory(applicationId.ToString(), sequenceId, sectionId, pageId, questionId, container);
            var blob = questionFolder.GetBlobReference(filename);
            return await blob.ExistsAsync();
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