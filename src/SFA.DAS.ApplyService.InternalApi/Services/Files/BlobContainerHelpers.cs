using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using SFA.DAS.ApplyService.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Services.Files;

[ExcludeFromCodeCoverage]
public static class BlobContainerHelpers
{
    public static async Task<BlobContainerClient> GetContainer(FileStorageConfig fileStorageConfig, ContainerType containerType)
    {
        BlobContainerClient container = null;

        var containerName = GetContainerName(fileStorageConfig, containerType);

        if (!string.IsNullOrWhiteSpace(containerName))
        {
            var options = new BlobClientOptions(BlobClientOptions.ServiceVersion.V2023_11_03);
            var client = new BlobServiceClient(fileStorageConfig.StorageConnectionString, options);
            container = client.GetBlobContainerClient(containerName.ToLower());
            await container.CreateIfNotExistsAsync();
        }

        return container;
    }

    public static BlobDirectory GetDirectory(this BlobContainerClient container, Guid applicationId)
    {
        return container.GetApplicationDirectory(applicationId);
    }

    public static BlobDirectory GetDirectory(this BlobContainerClient container, Guid applicationId, int? sequenceNumber, int? sectionNumber, string pageId)
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

    private static BlobDirectory GetApplicationDirectory(this BlobContainerClient container, Guid applicationId)
    {
        return new BlobDirectory(container, $"{applicationId}/");
    }

    private static BlobDirectory GetPageDirectory(this BlobContainerClient container, Guid applicationId, string pageId)
    {
        return new BlobDirectory(container, $"{applicationId}/{pageId.ToLowerInvariant()}/");
    }

    private static BlobDirectory GetPageDirectory(this BlobContainerClient container, Guid applicationId, int sequenceNumber, int sectionNumber, string pageId)
    {
        return new BlobDirectory(container, $"{applicationId}/{sequenceNumber}/{sectionNumber}/{pageId.ToLowerInvariant()}/");
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

public sealed class BlobDirectory
{
    public BlobDirectory(BlobContainerClient container, string prefix)
    {
        Container = container;
        Prefix = prefix;
    }

    public BlobContainerClient Container { get; }

    public string Prefix { get; }
}
}
