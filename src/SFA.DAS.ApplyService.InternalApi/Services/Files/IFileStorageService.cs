using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Services.Files
{
    public interface IFileStorageService
    {
        Task<IEnumerable<string>> GetPageFileList(Guid applicationId, int? sequenceNumber, int? sectionNumber, string pageId, ContainerType containerType, CancellationToken cancellationToken);
        Task<bool> UploadFiles(Guid applicationId, int? sequenceNumber, int? sectionNumber, string pageId, IFormFileCollection files, ContainerType containerType, CancellationToken cancellationToken);
        Task<bool> DeleteFile(Guid applicationId, int? sequenceNumber, int? sectionNumber, string pageId, string fileName, ContainerType containerType, CancellationToken cancellationToken);
        Task<DownloadFile> DownloadFile(Guid applicationId, int? sequenceNumber, int? sectionNumber, string pageId, string fileName, ContainerType containerType, CancellationToken cancellationToken);
        Task<DownloadFile> DownloadPageFiles(Guid applicationId, int? sequenceNumber, int? sectionNumber, string pageId, ContainerType containerType, CancellationToken cancellationToken);
    }
}