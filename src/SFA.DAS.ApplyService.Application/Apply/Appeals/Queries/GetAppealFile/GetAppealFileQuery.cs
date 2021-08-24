using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Appeals.Queries.GetAppealFile
{
    public class GetAppealFileQuery : IRequest<GetAppealFileQueryResult>
    {
        public Guid ApplicationId { get; set; }
        public Guid FileId { get; set; }
    }
}
