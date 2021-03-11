using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetStagedFiles
{
    public class GetStagedFilesQueryHandler : IRequestHandler<GetStagedFilesQuery, GetStagedFilesResult>
    {
        private readonly IAppealsQueries _appealsQueries;

        public GetStagedFilesQueryHandler(IAppealsQueries appealsQueries)
        {
            _appealsQueries = appealsQueries;
        }

        public async Task<GetStagedFilesResult> Handle(GetStagedFilesQuery request, CancellationToken cancellationToken)
        {
            var queryResult = await _appealsQueries.GetStagedAppealFiles(request.ApplicationId);

            return new GetStagedFilesResult
            {
                Files = queryResult.Files.Select(file => new GetStagedFilesResult.AppealFile
                    {
                        Id = file.Id,
                        Filename = file.Filename
                    }).ToList()
            };
        }
    }
}
