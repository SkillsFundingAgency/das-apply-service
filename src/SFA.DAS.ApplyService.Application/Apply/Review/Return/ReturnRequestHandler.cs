using MediatR;
using SFA.DAS.ApplyService.Application.Email.Consts;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Application.Users;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Review.Return
{
    public class ReturnRequestHandler : IRequestHandler<ReturnRequest>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IEmailService _emailServiceObject;
        private readonly IContactRepository _contactRepository;

        public ReturnRequestHandler(IApplyRepository applyRepository, IContactRepository contactRepository, IEmailService emailServiceObject)
        {
            _applyRepository = applyRepository;
            _emailServiceObject = emailServiceObject;
            _contactRepository = contactRepository;
        }

        public async Task<Unit> Handle(ReturnRequest request, CancellationToken cancellationToken)
        {
            // There will probably be some sort of decision of notifications here based on what status the Application
            // is being set to.  But for now I'm just setting it and forgetting.
            var sequence = await _applyRepository.GetActiveSequence(request.ApplicationId);

            if (request.RequestReturnType == "ReturnWithFeedback")
            {
                await _applyRepository.UpdateSequenceStatus(request.ApplicationId, request.SequenceId,
                    ApplicationSequenceStatus.FeedbackAdded, ApplicationStatus.FeedbackAdded);
            }
            else if (request.RequestReturnType == "Approve" || request.RequestReturnType == "ApproveWithFeedback")
            {
                await _applyRepository.UpdateSequenceStatus(request.ApplicationId, request.SequenceId,
                    ApplicationSequenceStatus.Approved, ApplicationStatus.InProgress);

                await _applyRepository.CloseSequence(request.ApplicationId, request.SequenceId);

                var nextSequence =
                    (await _applyRepository.GetSequences(request.ApplicationId))
                    .FirstOrDefault(seq => (int)seq.SequenceId == request.SequenceId + 1);

                if (nextSequence != null)
                {
                    await _applyRepository.OpenSequence(request.ApplicationId, (int)nextSequence.SequenceId);
                }
                else
                {
                    // this is the last sequence, so approve the whole application
                    await _applyRepository.UpdateApplicationStatus(request.ApplicationId, ApplicationStatus.Approved);
                }
            }
            else
            {
                await _applyRepository.UpdateSequenceStatus(request.ApplicationId, request.SequenceId,
                    ApplicationSequenceStatus.Rejected, ApplicationStatus.Rejected);
            }

            await NotifyContact(request.ApplicationId, request.SequenceId);

            return Unit.Value;
        }

        private async Task NotifyContact(Guid applicationId, int sequenceId)
        {
            var application = await _applyRepository.GetApplication(applicationId);
            var standard = application.ApplicationData?.StandardName ?? string.Empty;
            var reference = application.ApplicationData?.ReferenceNumber ?? string.Empty;

            if (sequenceId == 1)
            {
                var lastInitSubmission = application.ApplicationData?.InitSubmissions.OrderByDescending(sub => sub.SubmittedAt).FirstOrDefault();
                var contactToNotify = await _contactRepository.GetContact(lastInitSubmission?.SubmittedBy);

                await _emailServiceObject.SendEmailToContact(EmailTemplateName.APPLY_EPAO_UPDATE, contactToNotify, new { reference });
            }
            else if (sequenceId == 2)
            {
                var lastStandardSubmission = application.ApplicationData?.StandardSubmissions.OrderByDescending(sub => sub.SubmittedAt).FirstOrDefault();
                var contactToNotify = await _contactRepository.GetContact(lastStandardSubmission?.SubmittedBy);

                await _emailServiceObject.SendEmailToContact(EmailTemplateName.APPLY_EPAO_RESPONSE, contactToNotify, new { reference, standard });
            }
        }
    }
}