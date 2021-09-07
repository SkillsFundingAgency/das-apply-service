using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Appeals.Commands.CancelAppeal
{
    public class CancelAppealCommandHandler : IRequestHandler<CancelAppealCommand>
    {
        private readonly IAppealRepository _appealRepository;
        private readonly IAppealFileRepository _appealFileRepository;
        private readonly IAuditService _auditService;

        public CancelAppealCommandHandler(IAppealRepository appealRepository, IAppealFileRepository appealFileRepository, IAuditService auditService)
        {
            _appealRepository = appealRepository;
            _appealFileRepository = appealFileRepository;
            _auditService = auditService;
        }

        public async Task<Unit> Handle(CancelAppealCommand request, CancellationToken cancellationToken)
        {
            _auditService.StartTracking(UserAction.CancelAppeal, request.UserId, request.UserName);

            var appealFiles = await _appealFileRepository.GetAllForApplication(request.ApplicationId);
            if(appealFiles != null)
            {
                foreach(var file in appealFiles)
                {
                    _auditService.AuditDelete(file);
                    _appealFileRepository.Remove(file.Id);
                }
            }

            var currentAppeal = await _appealRepository.GetByApplicationId(request.ApplicationId);
            if (currentAppeal != null)
            {
                _auditService.AuditDelete(currentAppeal);
                _appealRepository.Remove(currentAppeal.Id);
            }

            _auditService.Save();

            return Unit.Value;
        }
    }
}
