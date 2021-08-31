using System;
using Microsoft.AspNetCore.Http;

namespace SFA.DAS.ApplyService.InternalApi.Types.Requests.Appeals
{
    public class UploadAppealFileRequest
    {
        public Guid ApplicationId { get; set; }
        public IFormFile AppealFile { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
