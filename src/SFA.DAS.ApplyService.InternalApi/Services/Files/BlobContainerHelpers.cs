using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SFA.DAS.ApplyService.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Services.Files;

[ExcludeFromCodeCoverage]
public static class BlobContainerHelpers
{
    public static async Task<CloudBlobContainer> GetContainer(FileStorageConfig fileStorageConfig, ContainerType containerType)
    {
        var container = default(CloudBlobContainer);

        var containerName = GetContainerName(fileStorageConfig, containerType);

        if (!string.IsNullOrWhiteSpace(containerName))
        {
            var account = CloudStorageAccount.Parse(fileStorageConfig.StorageConnectionString);
            var client = account.CreateCloudBlobClient();
            container = client.GetContainerReference(containerName.ToLower());
            await container.CreateIfNotExistsAsync();
        }

        return container;
    }

    public static CloudBlobDirectory GetDirectory(this CloudBlobContainer container, Guid applicationId)
    {
        return container.GetApplicationDirectory(applicationId);
    }

    public static CloudBlobDirectory GetDirectory(this CloudBlobContainer container, Guid applicationId, int? sequenceNumber, int? sectionNumber, string pageId)
    {
        if (string.IsNullOrWhiteSpace(pageId))
        {
            throw new ArgumentNullException(nameof(pageId));
        }
        else if (sequenceNumber is null || sectionNumber is null)
        {
            return container.GetPageDirectory(applicationId, pageId);

        }
        else
        {
            return container.GetPageDirectory(applicationId, sequenceNumber.Value, sectionNumber.Value, pageId);
        }
    }

    private static CloudBlobDirectory GetApplicationDirectory(this CloudBlobContainer container, Guid applicationId)
    {
        return container.GetDirectoryReference(applicationId.ToString()); ;
    }

    private static CloudBlobDirectory GetPageDirectory(this CloudBlobContainer container, Guid applicationId, string pageId)
    {
        var applicationFolder = container.GetApplicationDirectory(applicationId);
        var pageFolder = applicationFolder.GetDirectoryReference(pageId.ToLower());
        return pageFolder;
    }

    private static CloudBlobDirectory GetPageDirectory(this CloudBlobContainer container, Guid applicationId, int sequenceNumber, int sectionNumber, string pageId)
    {
        var applicationFolder = container.GetApplicationDirectory(applicationId);
        var sequenceFolder = applicationFolder.GetDirectoryReference(sequenceNumber.ToString());
        var sectionFolder = sequenceFolder.GetDirectoryReference(sectionNumber.ToString());
        var pageFolder = sectionFolder.GetDirectoryReference(pageId.ToLower());
        return pageFolder;
    }

    private static string GetContainerName(FileStorageConfig fileStorageConfig, ContainerType containerType)
    {
        switch (containerType)
        {
            case ContainerType.Gateway:
                return fileStorageConfig.GatewayContainerName;
            case ContainerType.Financial:
                return fileStorageConfig.FinancialContainerName;
            case ContainerType.Assessor:
                return fileStorageConfig.AssessorContainerName;
            case ContainerType.Appeals:
                return fileStorageConfig.AppealsContainerName;
            default:
                return null;
        }
    }
}
