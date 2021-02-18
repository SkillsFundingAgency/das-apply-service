using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class GetAppealFilesRequestHandler : IRequestHandler<GetAppealFilesRequest, AppealFiles>
    {
        private readonly IAppealsQueries _appealsQueries;

        public GetAppealFilesRequestHandler(IAppealsQueries appealsQueries)
        {
            _appealsQueries = appealsQueries;
        }

        public async Task<AppealFiles> Handle(GetAppealFilesRequest request, CancellationToken cancellationToken)
        {
            if (request.AppealId.HasValue)
            {
                throw new NotImplementedException();
            }

            return await _appealsQueries.GetStagedAppealFiles(request.ApplicationId);
        }
    }
}
