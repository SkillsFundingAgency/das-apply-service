﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Data.FileStorage;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Appeals.Commands.UploadAppealFile
{
    public class UploadAppealFileCommandHandler : IRequestHandler<UploadAppealFileCommand>
    {
        private readonly IAppealFileRepository _appealFileRepository;
        private readonly IAppealsFileStorage _appealsFileStorage;
        private readonly IAuditService _auditService;//

        public UploadAppealFileCommandHandler(
            IAppealFileRepository appealFileRepository,
            IAppealsFileStorage appealsFileStorage,
            IAuditService auditService
            )
        {
            _appealFileRepository = appealFileRepository;
            _appealsFileStorage = appealsFileStorage;
            _auditService = auditService;
        }

        public async Task<Unit> Handle(UploadAppealFileCommand request, CancellationToken cancellationToken)
        {
            _auditService.StartTracking(UserAction.UploadAppealFile, request.UserId, request.UserName);

            var fileStorageReference = await _appealsFileStorage.Add(request.ApplicationId, request.AppealFile, cancellationToken);

            var appealFile = new AppealFile
            {
                ApplicationId = request.ApplicationId,
                FileStorageReference = fileStorageReference,
                ContentType = request.AppealFile.ContentType,
                Filename = request.AppealFile.Filename,
                Size = request.AppealFile.Data.Length,
                UserId = request.UserId,
                UserName = request.UserName
            };

            _appealFileRepository.Add(appealFile);

            _auditService.AuditInsert(appealFile);
            _auditService.Save();

            return Unit.Value;
        }
    }
}
