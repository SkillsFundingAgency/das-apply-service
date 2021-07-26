using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Models;

namespace SFA.DAS.ApplyService.Data.FileStorage
{
    public class AppealsFileStorage : FileStorage, IAppealsFileStorage
    {
        public AppealsFileStorage(BlobServiceClient blobServiceClient,
            IConfigurationService configurationService)
            : base(blobServiceClient)
        {
            var config = configurationService.GetConfig().GetAwaiter().GetResult();

            if (string.IsNullOrWhiteSpace(config.FileStorage.AppealsContainerName))
            {
                throw new InvalidOperationException("AppealsFileStorage error - FileStore.AppealsContainerName config is missing");
            }

            ContainerName = config.FileStorage.AppealsContainerName;
        }

        protected override string ContainerName { get; }

        public async Task<Guid> Add(Guid applicationId, FileUpload file, CancellationToken cancellationToken)
        {
            return await AddFileToContainer($"{applicationId}", file, cancellationToken);
        }

        public async Task Remove(Guid applicationId, Guid reference, CancellationToken cancellationToken)
        {
            await RemoveFileFromContainer($"{applicationId}", reference);
        }

        public async Task<byte[]> Get(Guid applicationId, Guid reference, CancellationToken cancellationToken)
        {
            return await GetFileFromContainer($"{applicationId}", reference);
        }
    }
}

