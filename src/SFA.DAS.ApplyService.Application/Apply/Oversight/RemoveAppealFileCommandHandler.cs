using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Data.FileStorage;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class RemoveAppealFileCommandHandler : IRequestHandler<RemoveAppealFileCommand>
    {
        private readonly IAppealUploadRepository _appealUploadRepository;
        private readonly IAppealFileStorage _appealFileStorage;
        private readonly IAuditService _auditService;

        public RemoveAppealFileCommandHandler(IAppealUploadRepository appealUploadRepository,
            IAppealFileStorage appealFileStorage,
            IAuditService auditService)
        {
            _appealUploadRepository = appealUploadRepository;
            _appealFileStorage = appealFileStorage;
            _auditService = auditService;
        }

        public async Task<Unit> Handle(RemoveAppealFileCommand request, CancellationToken cancellationToken)
        {
            var upload = await _appealUploadRepository.GetById(request.AppealUploadId);

            if (upload.ApplicationId != request.ApplicationId)
            {
                throw new InvalidOperationException($"Appeal upload {request.AppealUploadId} does not belong to Application {request.ApplicationId}");
            }

            await _appealFileStorage.Remove(request.ApplicationId, upload.FileId, cancellationToken);

            _auditService.StartTracking(UserAction.RemoveAppealFile, request.UserId, request.UserName);

            _auditService.AuditDelete(upload);
            _appealUploadRepository.Remove(upload.Id);

            _auditService.Save();

            return Unit.Value;
        }
    }
}
