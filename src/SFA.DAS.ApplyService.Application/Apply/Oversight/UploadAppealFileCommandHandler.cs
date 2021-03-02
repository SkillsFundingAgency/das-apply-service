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
        private readonly IAppealsFileStorage _appealsFileStorage;
        private readonly IAuditService _auditService;

        public UploadAppealFileCommandHandler(
            IAppealUploadRepository appealUploadRepository,
            IAppealsFileStorage appealsFileStorage,
            IAuditService auditService
            )
        {
            _appealUploadRepository = appealUploadRepository;
            _appealsFileStorage = appealsFileStorage;
            _auditService = auditService;
        }

        public async Task<Unit> Handle(UploadAppealFileCommand request, CancellationToken cancellationToken)
        {
            _auditService.StartTracking(UserAction.UploadAppealFile, request.UserId, request.UserName);

            var fileStorageReference = await _appealsFileStorage.Add(request.ApplicationId, request.File, cancellationToken);

            var appealUpload = new AppealUpload
            {
                ApplicationId = request.ApplicationId,
                FileStorageReference = fileStorageReference,
                ContentType = request.File.ContentType,
                Filename = request.File.Filename,
                Size = request.File.Data.Length,
                UserId = request.UserId,
                UserName = request.UserName
            };

            _appealUploadRepository.Add(appealUpload);

            _auditService.AuditInsert(appealUpload);
            _auditService.Save();

            return Unit.Value;
        }
    }
}
