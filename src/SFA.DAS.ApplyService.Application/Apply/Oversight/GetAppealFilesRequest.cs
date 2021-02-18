using System;
using MediatR;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class GetAppealFilesRequest : IRequest<AppealFiles>
    {
        public Guid ApplicationId { get; set; }
        public Guid? AppealId { get; set; }
    }
}
