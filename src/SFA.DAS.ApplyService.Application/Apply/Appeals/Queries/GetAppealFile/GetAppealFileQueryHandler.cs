using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Data.FileStorage;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Appeals.Queries.GetAppealFile
{
    public class GetAppealFileQueryHandler : IRequestHandler<GetAppealFileQuery, GetAppealFileQueryResult>
    {
        private readonly IAppealFileRepository _appealFileRepository;
        private readonly IAppealsFileStorage _appealsFileStorage;

        public GetAppealFileQueryHandler(IAppealFileRepository appealFileRepository, IAppealsFileStorage appealsFileStorage)
        {
            _appealFileRepository = appealFileRepository;
            _appealsFileStorage = appealsFileStorage;
        }

        public async Task<GetAppealFileQueryResult> Handle(GetAppealFileQuery request, CancellationToken cancellationToken)
        {
            var upload = await _appealFileRepository.Get(request.FileId);

            if (upload.ApplicationId != request.ApplicationId)
            {
                throw new InvalidOperationException();
            }

            var content = await _appealsFileStorage.Get(request.ApplicationId, upload.FileStorageReference, cancellationToken);

            return new GetAppealFileQueryResult
            {
                Filename = upload.Filename,
                ContentType = upload.ContentType,
                Content = content
            };
        }
    }
}