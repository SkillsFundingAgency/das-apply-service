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
        private readonly IAppealsFileStorage _appealsFileStorage;
        private readonly IAuditService _auditService;

        public RemoveAppealFileCommandHandler(IAppealUploadRepository appealUploadRepository,
            IAppealsFileStorage appealsFileStorage,
            IAuditService auditService)
        {
            _appealUploadRepository = appealUploadRepository;
            _appealsFileStorage = appealsFileStorage;
            _auditService = auditService;
        }

        public async Task<Unit> Handle(RemoveAppealFileCommand request, CancellationToken cancellationToken)
        {
            var upload = await _appealUploadRepository.GetById(request.FileId);

            if (upload.ApplicationId != request.ApplicationId)
            {
                throw new InvalidOperationException($"Appeal upload {request.FileId} does not belong to Application {request.ApplicationId}");
            }

            await _appealsFileStorage.Remove(request.ApplicationId, upload.FileStorageReference, cancellationToken);

            _auditService.StartTracking(UserAction.RemoveAppealFile, request.UserId, request.UserName);

            _auditService.AuditDelete(upload);
            _appealUploadRepository.Remove(upload.Id);

            _auditService.Save();

            return Unit.Value;
        }
    }
}
