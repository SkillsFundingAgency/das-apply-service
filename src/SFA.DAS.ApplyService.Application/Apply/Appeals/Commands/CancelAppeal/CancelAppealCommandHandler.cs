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
                    _appealFileRepository.Remove(file.Id);
                    _auditService.AuditDelete(file);
                }
            }

            var currentAppeal = await _appealRepository.GetByApplicationId(request.ApplicationId);
            if (currentAppeal != null)
            {
                _appealRepository.Remove(currentAppeal.Id);
                _auditService.AuditDelete(currentAppeal);
            }

            _auditService.Save();

            return Unit.Value;
        }
    }
}
