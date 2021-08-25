using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Appeals.Commands.DeleteAppealFile
{
    public class DeleteAppealFileCommand : IRequest
    {
        public Guid ApplicationId { get; set; }
        public string FileName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
