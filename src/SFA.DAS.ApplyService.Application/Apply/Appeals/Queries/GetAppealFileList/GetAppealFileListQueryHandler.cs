using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Application.Apply.Appeals.Queries.GetAppealFileList
{
    public class GetAppealFileListQueryHandler : IRequestHandler<GetAppealFileListQuery, List<AppealFile>>
    {
        private readonly IAppealsQueries _appealsQueries;

        public GetAppealFileListQueryHandler(IAppealsQueries appealsQueries)
        {
            _appealsQueries = appealsQueries;
        }

        public async Task<List<AppealFile>> Handle(GetAppealFileListQuery request, CancellationToken cancellationToken)
        {
            return await _appealsQueries.GetAppealFilesForApplication(request.ApplicationId);
        }
    }
}
