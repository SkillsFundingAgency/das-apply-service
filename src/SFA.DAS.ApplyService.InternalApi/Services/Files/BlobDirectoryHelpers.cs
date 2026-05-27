using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using static SFA.DAS.ApplyService.InternalApi.Services.Files.BlobContainerHelpers;

namespace SFA.DAS.ApplyService.InternalApi.Services.Files;

[ExcludeFromCodeCoverage]
public static class BlobDirectoryHelpers
{
    public static async Task<IEnumerable<DownloadFileInfo>> GetFileList(this BlobDirectory directory)
    {
        var fileList = new List<DownloadFileInfo>();

        await foreach (var blobItem in directory.Container.GetBlobsAsync(BlobTraits.None, BlobStates.None, directory.Prefix, CancellationToken.None))
        {
            if (!blobItem.Name.StartsWith(directory.Prefix, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var fileInfo = new DownloadFileInfo
            {
                FileName = Path.GetFileName(blobItem.Name),
                ContentType = blobItem.Properties.ContentType,
                Size = blobItem.Properties.ContentLength ?? 0,
                CreatedOn = blobItem.Properties.CreatedOn?.DateTime ?? DateTime.Now
            };

            fileList.Add(fileInfo);
        }

        return fileList;
    }

    public static async Task<bool> UploadFiles(this BlobDirectory directory, Microsoft.AspNetCore.Http.IFormFileCollection files, ILogger<FileStorageService> logger, CancellationToken cancellationToken)
    {
        bool success;

        try
        {
            foreach (var file in files)
            {
                var blobClient = directory.Container.GetBlobClient($"{directory.Prefix}{file.FileName}");
                using var stream = file.OpenReadStream();
                await blobClient.UploadAsync(stream, new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders { ContentType = file.ContentType }
                }, cancellationToken);
            }

            success = true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error uploading files to directory: {DirectoryPrefix}", directory.Prefix);
            success = false;
        }

        return success;
    }

    public static async Task<bool> DeleteFile(this BlobDirectory directory, string fileName, CancellationToken cancellationToken)
    {
        var blob = directory.Container.GetBlobClient($"{directory.Prefix}{fileName}");

        var response = await blob.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        return response.Value;
    }

    public static async Task<DownloadFile> DownloadFile(this BlobDirectory directory, string fileName, CancellationToken cancellationToken)
    {
        var blob = directory.Container.GetBlobClient($"{directory.Prefix}{fileName}");

        return await DownloadFileFromBlob(blob, cancellationToken);
    }

    public static async Task<bool> DeleteDirectory(this BlobDirectory directory, CancellationToken cancellationToken)
    {
        var deleteResults = new List<bool>();

        await foreach (var blobItem in directory.Container.GetBlobsAsync(BlobTraits.None, BlobStates.None, directory.Prefix, cancellationToken))
        {
            var blobClient = directory.Container.GetBlobClient(blobItem.Name);
            var result = await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
            deleteResults.Add(result.Value);
        }

        return deleteResults.All(r => r is true);

    }

    public static async Task<IEnumerable<DownloadFile>> DownloadDirectoryFiles(this BlobDirectory directory, CancellationToken cancellationToken)
    {
        var downloadFiles = new List<DownloadFile>();

        await foreach (var blobItem in directory.Container.GetBlobsAsync(BlobTraits.None, BlobStates.None, directory.Prefix, cancellationToken))
        {
            var blobClient = directory.Container.GetBlobClient(blobItem.Name);
            var downloadFile = await DownloadFileFromBlob(blobClient, cancellationToken);
            downloadFiles.Add(downloadFile);
        }

        return downloadFiles;
    }

    private static async Task<DownloadFile> DownloadFileFromBlob(Azure.Storage.Blobs.BlobClient blob, CancellationToken cancellationToken)
    {
        if (await blob.ExistsAsync(cancellationToken))
        {
            var blobStream = new MemoryStream();

            var download = await blob.DownloadStreamingAsync(cancellationToken: cancellationToken);
            await download.Value.Content.CopyToAsync(blobStream, cancellationToken);
            blobStream.Position = 0;

            return new DownloadFile
            {
                FileName = Path.GetFileName(blob.Name),
                ContentType = download.Value.Details.ContentType,
                Stream = blobStream
            };
        }
        else
        {
            return default;
        }
    }
}
