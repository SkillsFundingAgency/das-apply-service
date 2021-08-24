using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Application.Apply.Appeals.Queries.GetAppealFileList
{
    public class GetAppealFileListQuery : IRequest<List<AppealFile>>
    {
        public Guid ApplicationId { get; set; }
    }
}
