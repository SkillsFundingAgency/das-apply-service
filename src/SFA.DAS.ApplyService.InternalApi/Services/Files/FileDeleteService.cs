using SFA.DAS.ApplyService.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Services.Files
{
    public interface IFileDeleteService
    {
        Task<bool> DeleteFile(Guid applicationId, int? sequenceNumber, int? sectionNumber, string pageId, string fileName, ContainerType containerType, CancellationToken cancellationToken);
    }

    public class FileDeleteService : IFileDeleteService
    {
        private readonly FileStorageConfig _fileStorageConfig;

        public FileDeleteService(IConfigurationService configurationService)
        {
            var config = configurationService.GetConfig().GetAwaiter().GetResult();
            _fileStorageConfig = config.FileStorage;
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
    }
}
