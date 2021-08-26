using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Application.Appeals.Queries.GetAppealFile
{
    public class GetAppealFileQueryHandler : IRequestHandler<GetAppealFileQuery, AppealFile>
    {
        private readonly IAppealsQueries _appealsQueries;

        public GetAppealFileQueryHandler(IAppealsQueries appealsQueries)
        {
            _appealsQueries = appealsQueries;
        }

        public async Task<AppealFile> Handle(GetAppealFileQuery request, CancellationToken cancellationToken)
        {
            return await _appealsQueries.GetAppealFile(request.ApplicationId, request.FileName);
        }
    }
}