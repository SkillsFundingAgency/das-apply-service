using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetAppealUpload
{
    public class GetAppealUploadQuery : IRequest<GetAppealUploadQueryResult>
    {
        public Guid ApplicationId { get; set; }
        public Guid AppealId { get; set; }
        public Guid AppealUploadId { get; set; }
    }
}
