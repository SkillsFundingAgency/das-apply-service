using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SFA.DAS.ApplyService.InternalApi.Types.Responses.Appeals;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public interface IAppealsApiClient
    {
        Task<GetAppealResponse> GetAppeal(Guid applicationId);
        Task<bool> MakeAppeal(Guid applicationId, string howFailedOnPolicyOrProcesses, string howFailedOnEvidenceSubmitted, string signinId, string userName);
        Task<bool> CancelAppeal(Guid applicationId, string signinId, string userName);

        Task<GetAppealFileListResponse> GetAppealFileList(Guid applicationId);
        Task<HttpResponseMessage> DownloadFile(Guid applicationId, string fileName);
        Task<bool> UploadFile(Guid applicationId, IFormFile appealFileToUpload, string signinId, string userName);
        Task<bool> DeleteFile(Guid applicationId, string fileName, string signinId, string userName);
    }
}