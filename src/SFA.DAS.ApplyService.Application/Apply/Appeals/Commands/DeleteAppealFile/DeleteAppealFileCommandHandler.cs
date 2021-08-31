using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Appeals.Commands.DeleteAppealFile
{
    public class DeleteAppealFileCommandHandler : IRequestHandler<DeleteAppealFileCommand>
    {
        private readonly IAppealFileRepository _appealFileRepository;
        private readonly IAuditService _auditService;

        public DeleteAppealFileCommandHandler(IAppealFileRepository appealFileRepository, IAuditService auditService)
        {
            _appealFileRepository = appealFileRepository;
            _auditService = auditService;
        }

        public async Task<Unit> Handle(DeleteAppealFileCommand request, CancellationToken cancellationToken)
        {
            var upload = await _appealFileRepository.Get(request.ApplicationId, request.FileName);

            if (upload is null)
            {
                throw new InvalidOperationException($"Appeal file {request.FileName} does not belong to Application {request.ApplicationId}");
            }

            _auditService.StartTracking(UserAction.RemoveAppealFile, request.UserId, request.UserName);

            _appealFileRepository.Remove(upload.Id);
            _auditService.AuditDelete(upload);

            _auditService.Save();

            return Unit.Value;
        }
    }
}
