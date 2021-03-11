using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Types;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight.Commands.CreateAppeal
{
	public class CreateAppealCommandHandler : IRequestHandler<CreateAppealCommand>
    {
        private readonly IOversightReviewRepository _oversightReviewRepository;
        private readonly IAppealRepository _appealRepository;
        private readonly IAppealUploadRepository _appealUploadRepository;
        private readonly IAuditService _auditService;

		public CreateAppealCommandHandler(IOversightReviewRepository oversightReviewRepository, IAppealRepository appealRepository, IAppealUploadRepository appealUploadRepository, IAuditService auditService)
        {
            _oversightReviewRepository = oversightReviewRepository;
            _appealRepository = appealRepository;
            _appealUploadRepository = appealUploadRepository;
            _auditService = auditService;
        }

		public async Task<Unit> Handle(CreateAppealCommand request, CancellationToken cancellationToken)
		{
            var oversightReview = await GetOversightReview(request.OversightReviewId);
            await VerifyAppealDoesNotExist(request.OversightReviewId);

            _auditService.StartTracking(UserAction.CreateAppeal, request.UserId, request.UserName);

            var appeal = new Appeal
            {
                OversightReviewId = request.OversightReviewId,
                Message = request.Message,
                UserId = request.UserId,
                UserName = request.UserName
            };

            _appealRepository.Add(appeal);
            _auditService.AuditInsert(appeal);

            var uploads = await _appealUploadRepository.GetByApplicationId(oversightReview.ApplicationId);

            foreach (var upload in uploads)
            {
                upload.AppealId = appeal.Id;
                _appealUploadRepository.Update(upload);
                _auditService.AuditUpdate(upload);
            }

            _auditService.Save();

			return Unit.Value;
		}

        private async Task<OversightReview> GetOversightReview(Guid oversightReviewId)
        {
            var oversightReview = await _oversightReviewRepository.GetById(oversightReviewId);

            if (oversightReview == null)
            {
                throw new InvalidOperationException($"OversightReview {oversightReviewId} not found");
            }

            if (oversightReview.Status != OversightReviewStatus.Unsuccessful)
            {
                throw new InvalidOperationException($"OversightReview {oversightReviewId} has status {oversightReview.Status} and cannot be appealed");
            }

            return oversightReview;
        }

        private async Task VerifyAppealDoesNotExist(Guid oversightReviewId)
        {
            var appeal = await _appealRepository.GetByOversightReviewId(oversightReviewId);
            if (appeal != null)
            {
                throw new InvalidOperationException($"Unable to create Appeal for OversightReview {oversightReviewId} as Appeal {appeal.Id} already exists");
            }
        }
	}
}
