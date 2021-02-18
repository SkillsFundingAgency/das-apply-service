using System;
using MediatR;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class GetStagedFilesRequest : IRequest<AppealFiles>
    {
        public Guid ApplicationId { get; set; }
    }
}
