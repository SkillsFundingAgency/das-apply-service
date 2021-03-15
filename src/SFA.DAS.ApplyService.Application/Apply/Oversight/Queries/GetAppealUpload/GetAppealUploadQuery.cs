using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using SFA.DAS.ApplyService.Data.FileStorage;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetAppealUpload
{
    public class GetAppealUploadQuery : IRequest<GetAppealUploadQueryResult>
    {
        public Guid ApplicationId { get; set; }
        public Guid OversightReviewId { get; set; }
        public Guid AppealId { get; set; }
        public Guid AppealUploadId { get; set; }
    }

    public class GetAppealUploadQueryResult
    {
        public string Filename { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }

    public class GetAppealUploadQueryValidator : AbstractValidator<GetAppealUploadQuery>
    {
        public GetAppealUploadQueryValidator()
        {
            RuleFor(x => x.ApplicationId).NotEmpty();
            RuleFor(x => x.OversightReviewId).NotEmpty();
            RuleFor(x => x.AppealId).NotEmpty();
            RuleFor(x => x.AppealUploadId).NotEmpty();
        }
    }

    public class GetAppealUploadQueryHandler : IRequestHandler<GetAppealUploadQuery, GetAppealUploadQueryResult>
    {
        private readonly IAppealUploadRepository _appealUploadRepository;
        private readonly IAppealsFileStorage _appealsFileStorage;

        public GetAppealUploadQueryHandler(IAppealUploadRepository appealUploadRepository, IAppealsFileStorage appealsFileStorage)
        {
            _appealUploadRepository = appealUploadRepository;
            _appealsFileStorage = appealsFileStorage;
        }

        public async Task<GetAppealUploadQueryResult> Handle(GetAppealUploadQuery request, CancellationToken cancellationToken)
        {
            var upload = await _appealUploadRepository.GetById(request.AppealUploadId);

            if (upload.ApplicationId != request.ApplicationId || upload.AppealId != request.AppealId)
            {
                throw new InvalidOperationException();
            }

            var content = await _appealsFileStorage.Get(request.ApplicationId, upload.FileStorageReference, cancellationToken);

            return new GetAppealUploadQueryResult
            {
                Filename = upload.Filename,
                ContentType = upload.ContentType,
                Content = content
            };
        }
    }
}
