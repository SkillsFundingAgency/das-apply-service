using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetStagedFiles
{
    public class GetStagedFilesQuery : IRequest<GetStagedFilesResult>
    {
        public Guid ApplicationId { get; set; }
    }
}
