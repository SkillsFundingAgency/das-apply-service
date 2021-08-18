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
        private readonly IAppealRepository _appealRepository;
        private readonly IAuditService _auditService;

		public MakeAppealCommandHandler(IAppealRepository appealRepository, IAuditService auditService)
        {
            _appealRepository = appealRepository;
            _auditService = auditService;
        }

		public async Task<Unit> Handle(MakeAppealCommand request, CancellationToken cancellationToken)
		{
            var currentAppeal = await _appealRepository.GetByApplicationId(request.ApplicationId);
            VerifyAppealNotSubmitted(currentAppeal);

            _auditService.StartTracking(UserAction.CreateAppeal, request.UserId, request.UserName);

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
