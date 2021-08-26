using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Types;

namespace SFA.DAS.ApplyService.Application.Apply.Appeals.Commands.MakeAppeal
{
    public class MakeAppealCommandHandler : IRequestHandler<MakeAppealCommand>
    {
        private readonly IOversightReviewRepository _oversightReviewRepository;
        private readonly IAppealRepository _appealRepository;
        private readonly IAuditService _auditService;

		public MakeAppealCommandHandler(IOversightReviewRepository oversightReviewRepository, IAppealRepository appealRepository, IAuditService auditService)
        {
            _oversightReviewRepository = oversightReviewRepository;
            _appealRepository = appealRepository;
            _auditService = auditService;
        }

		public async Task<Unit> Handle(MakeAppealCommand request, CancellationToken cancellationToken)
		{
            var oversightReview = await _oversightReviewRepository.GetByApplicationId(request.ApplicationId);
            VerifyOversightReviewIsUnsuccessfulOrRemoved(oversightReview);

            var currentAppeal = await _appealRepository.GetByApplicationId(request.ApplicationId);
            VerifyAppealNotSubmitted(currentAppeal);

            _auditService.StartTracking(UserAction.MakeAppeal, request.UserId, request.UserName);

            if(currentAppeal is null)
            {
                currentAppeal = new Appeal 
                { 
                    ApplicationId = request.ApplicationId ,
                    Status = AppealStatus.Submitted,
                    HowFailedOnPolicyOrProcesses = request.HowFailedOnPolicyOrProcesses,
                    HowFailedOnEvidenceSubmitted = request.HowFailedOnEvidenceSubmitted,
                    UserId = request.UserId,
                    UserName = request.UserName,
                };

                _appealRepository.Add(currentAppeal);
                _auditService.AuditInsert(currentAppeal);
            }
            else
            {
                currentAppeal.Status = AppealStatus.Submitted;
                currentAppeal.HowFailedOnPolicyOrProcesses = request.HowFailedOnPolicyOrProcesses;
                currentAppeal.HowFailedOnEvidenceSubmitted = request.HowFailedOnEvidenceSubmitted;
                currentAppeal.UserId = request.UserId;
                currentAppeal.UserName = request.UserName;

                _appealRepository.Update(currentAppeal);
                _auditService.AuditUpdate(currentAppeal);
            }




            //var uploads = await _appealUploadRepository.GetByApplicationId(oversightReview.ApplicationId);

            //foreach (var upload in uploads)
            //{
            //    _auditService.AuditUpdate(upload);
            //    upload.AppealId = appeal.Id;
            //    _appealUploadRepository.Update(upload);
            //}

            _auditService.Save();

			return Unit.Value;
		}

        private void VerifyOversightReviewIsUnsuccessfulOrRemoved(OversightReview oversightReview)
        {
            var allowedStatuses = new[] { OversightReviewStatus.Unsuccessful, OversightReviewStatus.Removed };

            if (oversightReview is null)
            {
                throw new InvalidOperationException($"OversightReview for Application {oversightReview.ApplicationId} not found");
            }
            else if (!allowedStatuses.Contains(oversightReview.Status))
            {
                throw new InvalidOperationException($"OversightReview for Application {oversightReview.ApplicationId} has status {oversightReview.Status} and cannot be appealed");
            }
        }

        private void VerifyAppealNotSubmitted(Appeal appeal)
        {
            var allowedStatuses = new[] { AppealStatus.None };

            if (appeal != null && !allowedStatuses.Contains(appeal.Status))
            {
                throw new InvalidOperationException($"Unable to create Appeal for Application {appeal.ApplicationId} as Appeal {appeal.Id} already submitted");
            }
        }
	}
}
