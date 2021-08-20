using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Application.Apply.Appeals.Queries.GetAppeal
{
    public class GetAppealQueryHandler : IRequestHandler<GetAppealQuery, Appeal>
    {
        private readonly IAppealsQueries _appealsQueries;

        public GetAppealQueryHandler(IAppealsQueries appealsQueries)
        {
            _appealsQueries = appealsQueries;
        }

        public async Task<Appeal> Handle(GetAppealQuery request, CancellationToken cancellationToken)
        {
            return await _appealsQueries.GetAppeal(request.ApplicationId);
        }
    }
}