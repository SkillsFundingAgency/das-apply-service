using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Services.Files
{
    public interface IFileStorageService
    {
        Task<IEnumerable<DownloadFileInfo>> GetApplicationFileList(Guid applicationId, ContainerType containerType, CancellationToken cancellationToken);
        Task<IEnumerable<DownloadFileInfo>> GetFileList(Guid applicationId, int? sequenceNumber, int? sectionNumber, string pageId, ContainerType containerType, CancellationToken cancellationToken);

        Task<bool> UploadApplicationFiles(Guid applicationId, IFormFileCollection files, ContainerType containerType, CancellationToken cancellationToken);
        Task<bool> UploadFiles(Guid applicationId, int? sequenceNumber, int? sectionNumber, string pageId, IFormFileCollection files, ContainerType containerType, CancellationToken cancellationToken);

        Task<bool> DeleteApplicationFile(Guid applicationId, string fileName, ContainerType containerType, CancellationToken cancellationToken);
        Task<bool> DeleteFile(Guid applicationId, int? sequenceNumber, int? sectionNumber, string pageId, string fileName, ContainerType containerType, CancellationToken cancellationToken);

        Task<DownloadFile> DownloadApplicationFile(Guid applicationId, string fileName, ContainerType containerType, CancellationToken cancellationToken);
        Task<DownloadFile> DownloadFile(Guid applicationId, int? sequenceNumber, int? sectionNumber, string pageId, string fileName, ContainerType containerType, CancellationToken cancellationToken);

        Task<DownloadFile> DownloadApplicationFiles(Guid applicationId, ContainerType containerType, CancellationToken cancellationToken);
        Task<DownloadFile> DownloadFiles(Guid applicationId, int? sequenceNumber, int? sectionNumber, string pageId, ContainerType containerType, CancellationToken cancellationToken);

        Task<bool> DeleteApplicationDirectory(Guid applicationId, ContainerType containerType, CancellationToken cancellationToken);
        Task<bool> DeleteDirectory(Guid applicationId, int? sequenceNumber, int? sectionNumber, string pageId, ContainerType containerType, CancellationToken cancellationToken);
    }
}