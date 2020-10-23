using Microsoft.Azure.Storage.Blob;
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
    public interface IFileListingService
    {
        Task<IEnumerable<string>> GetFileList(Guid applicationId, int? sequenceNumber, int? sectionNumber, string pageId, ContainerType containerType, CancellationToken cancellationToken);
    }

    public class FileListingService : IFileListingService
    {
        private readonly FileStorageConfig _fileStorageConfig;

        public FileListingService(IConfigurationService configurationService)
        {
            var config = configurationService.GetConfig().GetAwaiter().GetResult();
            _fileStorageConfig = config.FileStorage;
        }

        public async Task<IEnumerable<string>> GetFileList(Guid applicationId, int? sequenceNumber, int? sectionNumber, string pageId, ContainerType containerType, CancellationToken cancellationToken)
        {
            var fileList = default(List<string>);

            if (!string.IsNullOrWhiteSpace(pageId))
            {
                var container = await BlobContainerHelpers.GetContainer(_fileStorageConfig, containerType);

                if (container != null)
                {
                    var pageDirectory = container.GetDirectory(applicationId, sequenceNumber, sectionNumber, pageId);
                    var fileBlobs = pageDirectory.ListBlobs(useFlatBlobListing: true).ToList();

                    fileList = new List<string>(fileBlobs.Count);
                    foreach (var blob in fileBlobs.OfType<CloudBlob>())
                    {
                        string fileName = Path.GetFileName(blob.Name);
                        fileList.Add(fileName);
                    }
                }
            }

            return fileList;
        }
    }
}
