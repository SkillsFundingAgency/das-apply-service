using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetAppeal
{
    public class GetAppealQueryHandler : IRequestHandler<GetAppealQuery, GetAppealQueryResult>
    {
        private readonly IAppealsQueries _appealsQueries;

        public GetAppealQueryHandler(IAppealsQueries appealsQueries)
        {
            _appealsQueries = appealsQueries;
        }

        public async Task<GetAppealQueryResult> Handle(GetAppealQuery request, CancellationToken cancellationToken)
        {
            var result = await _appealsQueries.GetAppeal(request.ApplicationId, request.OversightReviewId);

            return result == null ? null : new GetAppealQueryResult
            {
                Message = result.Message,
                CreatedOn = result.CreatedOn,
                UserId = result.UserId,
                UserName = result.UserName,
                Uploads = result.Uploads.Select(upload => new GetAppealQueryResult.AppealUpload
                    {
                        Id = upload.Id,
                        Filename = upload.Filename,
                        ContentType = upload.ContentType
                    }).ToList()
            };
        }
    }
}