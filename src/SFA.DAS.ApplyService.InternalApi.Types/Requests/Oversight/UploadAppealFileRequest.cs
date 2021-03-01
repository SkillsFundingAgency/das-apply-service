using System;
using Microsoft.AspNetCore.Http;

namespace SFA.DAS.ApplyService.InternalApi.Types.Requests.Oversight
{
    public class UploadAppealFileRequest
    {
        public Guid ApplicationId { get; set; }
        public IFormFile File { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
