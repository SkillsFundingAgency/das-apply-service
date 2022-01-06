using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Appeals.Commands.UploadAppealFile
{
    public class UploadAppealFileCommandHandler : IRequestHandler<UploadAppealFileCommand>
    {
        private readonly IAppealFileRepository _appealFileRepository;
        private readonly IAuditService _auditService;

        public UploadAppealFileCommandHandler(IAppealFileRepository appealFileRepository, IAuditService auditService)
        {
            _appealFileRepository = appealFileRepository;
            _auditService = auditService;
        }

        public async Task<Unit> Handle(UploadAppealFileCommand request, CancellationToken cancellationToken)
        {
            _auditService.StartTracking(UserAction.UploadAppealFile, request.UserId, request.UserName);

            var appealFile = await _appealFileRepository.Get(request.ApplicationId, request.AppealFile.FileName);

            if (appealFile is null)
            {
                appealFile = new AppealFile
                {
                    ApplicationId = request.ApplicationId,
                    ContentType = request.AppealFile.ContentType,
                    FileName = request.AppealFile.FileName,
                    Size = request.AppealFile.Data.Length,
                    UserId = request.UserId,
                    UserName = request.UserName
                };

                _auditService.AuditInsert(appealFile);
                _appealFileRepository.Add(appealFile);
            }
            else
            {
                _auditService.AuditUpdate(appealFile);

                appealFile.ContentType = request.AppealFile.ContentType;
                appealFile.Size = request.AppealFile.Data.Length;
                appealFile.UserId = request.UserId;
                appealFile.UserName = request.UserName;
                appealFile.CreatedOn = DateTime.UtcNow;
                
                _appealFileRepository.Update(appealFile);
            }
             
            _auditService.Save();

            return await Task.FromResult(Unit.Value);
        }
    }
}
