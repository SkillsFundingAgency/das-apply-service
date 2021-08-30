using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Data.FileStorage;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Appeals.Commands.DeleteAppealFile
{
    public class DeleteAppealFileCommandHandler : IRequestHandler<DeleteAppealFileCommand>
    {
        private readonly IAppealFileRepository _appealFileRepository;
        private readonly IAppealsFileStorage _appealsFileStorage;
        private readonly IAuditService _auditService;

        public DeleteAppealFileCommandHandler(IAppealFileRepository appealFileRepository,
            IAppealsFileStorage appealsFileStorage,
            IAuditService auditService)
        {
            _appealFileRepository = appealFileRepository;
            _appealsFileStorage = appealsFileStorage;
            _auditService = auditService;
        }

        public async Task<Unit> Handle(DeleteAppealFileCommand request, CancellationToken cancellationToken)
        {
            var upload = await _appealFileRepository.Get(request.FileId);

            if (upload.ApplicationId != request.ApplicationId)
            {
                throw new InvalidOperationException($"Appeal file {request.FileId} does not belong to Application {request.ApplicationId}");
            }

            await _appealsFileStorage.Remove(request.ApplicationId, upload.FileStorageReference, cancellationToken);

            _auditService.StartTracking(UserAction.RemoveAppealFile, request.UserId, request.UserName);

            _auditService.AuditDelete(upload);
            _appealFileRepository.Remove(upload.Id);

            _auditService.Save();

            return Unit.Value;
        }
    }
}
