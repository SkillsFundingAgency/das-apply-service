using System;
using MediatR;
using SFA.DAS.ApplyService.Domain.Models;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight.Commands.UploadAppealFile
{
    public class UploadAppealFileCommand : IRequest
    {
        public Guid ApplicationId { get; set; }
        public FileUpload File { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
