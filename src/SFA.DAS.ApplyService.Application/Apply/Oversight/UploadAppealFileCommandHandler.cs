using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Data.FileStorage;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class UploadAppealFileCommandHandler : IRequestHandler<UploadAppealFileCommand>
    {
        private readonly IAppealUploadRepository _appealUploadRepository;
        private readonly IAppealFileStorage _appealFileStorage;
        private readonly IAuditService _auditService;

        public UploadAppealFileCommandHandler(
            IAppealUploadRepository appealUploadRepository,
            IAppealFileStorage appealFileStorage,
            IAuditService auditService
            )
        {
            _appealUploadRepository = appealUploadRepository;
            _appealFileStorage = appealFileStorage;
            _auditService = auditService;
        }

        public async Task<Unit> Handle(UploadAppealFileCommand request, CancellationToken cancellationToken)
        {
            _auditService.StartTracking(UserAction.UploadAppealFile, request.UserId, request.UserName);

            var fileId = await _appealFileStorage.Add(request.ApplicationId, request.File, cancellationToken);

            var appealUpload = new AppealUpload
            {
                ApplicationId = request.ApplicationId,
                FileId = fileId,
                ContentType = request.File.ContentType,
                Filename = request.File.Filename,
                Size = request.File.Data.Length,
                UserId = request.UserId,
                UserName = request.UserName
            };

            _appealUploadRepository.Add(appealUpload);

            _auditService.AuditInsert(appealUpload);

            return Unit.Value;
        }
    }
}
