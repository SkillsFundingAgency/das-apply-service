using MediatR;
using SFA.DAS.ApplyService.Application.Email.Consts;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Application.Users;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Submit
{
    public class ApplicationSubmitHandler : IRequestHandler<ApplicationSubmitRequest>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IEmailService _emailServiceObject;

        public ApplicationSubmitHandler(IApplyRepository applyRepository, IEmailService emailServiceObject, IContactRepository contactRepository)
        {
            _applyRepository = applyRepository;
            _emailServiceObject = emailServiceObject;
            _contactRepository = contactRepository;
        }
        
        public async Task<Unit> Handle(ApplicationSubmitRequest request, CancellationToken cancellationToken)
        {
            await SubmitApplicationSequence(request);

            var updatedApplication = await _applyRepository.GetApplication(request.ApplicationId);

            var contact = await _contactRepository.GetContact(request.UserEmail);
            var reference = updatedApplication.ApplicationData.ReferenceNumber;
            var standard = updatedApplication.ApplicationData.StandardName;

            await NotifyContact(contact, request.SequenceId, reference, standard);

            return Unit.Value;
        }

        private async Task SubmitApplicationSequence(ApplicationSubmitRequest request)
        {
            var application = await _applyRepository.GetApplication(request.ApplicationId);

            if(application.ApplicationData == null)
            {
                // Note: If this has happened then are bigger issues elsewhere. Perhaps raise an exception rather than masking it??
                application.ApplicationData = new ApplicationData();
            }

            if(string.IsNullOrWhiteSpace(application.ApplicationData.ReferenceNumber))
            {
                application.ApplicationData.ReferenceNumber = await CreateReferenceNumber(request.ApplicationId);
            }

            if(request.SequenceId == 1)
            {
                if (application.ApplicationData.InitSubmissions == null)
                {
                    application.ApplicationData.InitSubmissions = new List<InitSubmission>();
                }

                var submission = new InitSubmission
                {
                    SubmittedAt = DateTime.UtcNow,
                    SubmittedBy = request.UserId,
                    SubmittedByEmail = request.UserEmail
                };

                application.ApplicationData.InitSubmissions.Add(submission);
                application.ApplicationData.InitSubmissionsCount = application.ApplicationData.InitSubmissions.Count;
                application.ApplicationData.LatestInitSubmissionDate = submission.SubmittedAt;
            }
            else if(request.SequenceId == 2)
            {
                if (application.ApplicationData.StandardSubmissions == null)
                {
                    application.ApplicationData.StandardSubmissions = new List<StandardSubmission>();
                }

                var submission = new StandardSubmission
                {
                    SubmittedAt = DateTime.UtcNow,
                    SubmittedBy = request.UserId,
                    SubmittedByEmail = request.UserEmail
                };

                application.ApplicationData.StandardSubmissions.Add(submission);
                application.ApplicationData.StandardSubmissionsCount = application.ApplicationData.StandardSubmissions.Count;
                application.ApplicationData.LatestStandardSubmissionDate = submission.SubmittedAt;
            }

            await _applyRepository.SubmitApplicationSequence(request, application.ApplicationData);
        }

        private async Task<string> CreateReferenceNumber(Guid applicationId)
        {
            var referenceNumber = string.Empty;

            var seq = await _applyRepository.GetNextAppReferenceSequence();
            var refFormat = await _applyRepository.GetWorkflowReferenceFormat(applicationId);

            if (seq > 0 && !string.IsNullOrEmpty(refFormat))
            {
                referenceNumber = string.Format($"{refFormat}{seq:D6}");
            }

            return referenceNumber;
        }

        private async Task NotifyContact(Contact contact, int sequenceId, string reference, string standard)
        {
            if (sequenceId == 1)
            {
                await _emailServiceObject.SendEmailToContact(EmailTemplateName.APPLY_EPAO_INITIAL_SUBMISSION, contact, new { reference });
            }
            else if (sequenceId == 2)
            {
                await _emailServiceObject.SendEmailToContact(EmailTemplateName.APPLY_EPAO_STANDARD_SUBMISSION, contact, new { reference, standard });
            }
        }
    }
}