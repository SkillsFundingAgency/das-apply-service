using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Services.Files;

[ExcludeFromCodeCoverage]
public static class BlobDirectoryHelpers
{
    public static async Task<IEnumerable<DownloadFileInfo>> GetFileList(this CloudBlobDirectory directory)
    {
        var fileList = new List<DownloadFileInfo>();

        var fileBlobs = await directory.ListBlobsSegmentedAsync(new BlobContinuationToken());

        foreach (var blob in fileBlobs.Results.OfType<CloudBlob>())
        {
            var fileInfo = new DownloadFileInfo
            {
                FileName = Path.GetFileName(blob.Name),
                ContentType = blob.Properties.ContentType,
                Size = blob.Properties.Length,
                CreatedOn = blob.Properties.Created?.DateTime ?? System.DateTime.Now
            };

            fileList.Add(fileInfo);
        }

        return fileList;
    }

    public static async Task<bool> UploadFiles(this CloudBlobDirectory directory, Microsoft.AspNetCore.Http.IFormFileCollection files, ILogger<FileStorageService> logger, CancellationToken cancellationToken)
    {
        bool success;

        try
        {
            foreach (var file in files)
            {
                var blob = directory.GetBlockBlobReference(file.FileName);
                blob.Properties.ContentType = file.ContentType;

                await blob.UploadFromStreamAsync(file.OpenReadStream());
            }

            success = true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error uploading files to directory: {directory.Uri} || Message: {ex.Message} || Stack trace: {ex.StackTrace}");
            success = false;
        }

        return success;
    }

    public static async Task<bool> DeleteFile(this CloudBlobDirectory directory, string fileName, CancellationToken cancellationToken)
    {
        var blob = directory.GetBlobReference(fileName);

        return await blob.DeleteIfExistsAsync();
    }

    public static async Task<DownloadFile> DownloadFile(this CloudBlobDirectory directory, string fileName, CancellationToken cancellationToken)
    {
        var blob = directory.GetBlobReference(fileName);

        return await DownloadFileFromBlob(blob, cancellationToken);
    }

    public static async Task<bool> DeleteDirectory(this CloudBlobDirectory directory, CancellationToken cancellationToken)
    {
        var fileBlobs = await directory.ListBlobsSegmentedAsync(new BlobContinuationToken());

        var deleteResults = new List<bool>(fileBlobs.Results.Count());

        foreach (var blob in fileBlobs.Results.OfType<CloudBlob>())
        {
            var result = await blob.DeleteIfExistsAsync();
            deleteResults.Add(result);
        }

        return deleteResults.All(r => r is true);

    }

    public static async Task<IEnumerable<DownloadFile>> DownloadDirectoryFiles(this CloudBlobDirectory directory, CancellationToken cancellationToken)
    {
        var fileBlobs = await directory.ListBlobsSegmentedAsync(new BlobContinuationToken());

        var downloadFiles = new List<DownloadFile>(fileBlobs.Results.Count());

        foreach (var blob in fileBlobs.Results.OfType<CloudBlob>())
        {
            var downloadFile = await DownloadFileFromBlob(blob, cancellationToken);
            downloadFiles.Add(downloadFile);
        }

        return downloadFiles;
    }

    private static async Task<DownloadFile> DownloadFileFromBlob(CloudBlob blob, CancellationToken cancellationToken)
    {
        if (blob.ExistsAsync().Result)
        {
            var blobStream = new MemoryStream();

            await blob.DownloadToStreamAsync(blobStream, null, new BlobRequestOptions() { DisableContentMD5Validation = true }, null, cancellationToken);
            blobStream.Position = 0;

            return new DownloadFile { FileName = Path.GetFileName(blob.Name), ContentType = blob.Properties.ContentType, Stream = blobStream };
        }
        else
        {
            return default(DownloadFile);
        }
    }
}
