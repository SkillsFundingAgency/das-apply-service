using System;
using MediatR;
using SFA.DAS.ApplyService.Domain.Models;

namespace SFA.DAS.ApplyService.Application.Appeals.Commands.UploadAppealFile
{
    public class UploadAppealFileCommand : IRequest
    {
        public Guid ApplicationId { get; set; }
        public FileUpload AppealFile { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
