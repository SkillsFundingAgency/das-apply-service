using System;
using MediatR;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Application.Appeals.Queries.GetAppealFile
{
    public class GetAppealFileQuery : IRequest<AppealFile>
    {
        public Guid ApplicationId { get; set; }
        public string FileName { get; set; }
    }
}
