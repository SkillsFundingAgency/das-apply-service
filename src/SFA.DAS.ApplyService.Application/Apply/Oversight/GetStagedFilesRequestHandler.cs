using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class GetStagedFilesRequestHandler : IRequestHandler<GetStagedFilesRequest, AppealFiles>
    {
        private readonly IAppealsQueries _appealsQueries;

        public GetStagedFilesRequestHandler(IAppealsQueries appealsQueries)
        {
            _appealsQueries = appealsQueries;
        }

        public async Task<AppealFiles> Handle(GetStagedFilesRequest request, CancellationToken cancellationToken)
        {
            return await _appealsQueries.GetStagedAppealFiles(request.ApplicationId);
        }
    }
}
