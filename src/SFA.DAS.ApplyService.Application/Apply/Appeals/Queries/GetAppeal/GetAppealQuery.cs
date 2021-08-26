using System;
using MediatR;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Application.Apply.Appeals.Queries.GetAppeal
{
    public class GetAppealQuery : IRequest<Appeal>
    {
        public Guid ApplicationId { get; set; }
    }
}
