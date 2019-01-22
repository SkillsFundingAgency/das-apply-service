using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Email.Consts;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Application.Users;
using SFA.DAS.ApplyService.Domain.Entities;

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
            await UpdateApplicationSections(request);
            await SubmitApplicationSequence(request);

            var updatedApplication = await _applyRepository.GetApplication(request.ApplicationId);

            var contact = await _contactRepository.GetContact(request.UserEmail);
            var reference = updatedApplication.ApplicationData.ReferenceNumber;
            var standard = updatedApplication.ApplicationData.StandardName;

            await NotifyContact(contact, request.SequenceId, reference, standard);

            return Unit.Value;
        }

        private async Task UpdateApplicationSections(ApplicationSubmitRequest request)
        {
            var sections = await _applyRepository.GetSections(request.ApplicationId);

            foreach (var section in sections)
            {
                foreach (var page in section.QnAData.Pages)
                {
                    if (page.HasFeedback)
                    {
                        foreach (var feedback in page.Feedback)
                        {
                            feedback.IsNew = false;
                        }
                    }
                }
            }

            await _applyRepository.UpdateSections(sections);
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
                    SubmittedBy = request.UserEmail
                };

                application.ApplicationData.InitSubmissions.Add(submission);
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
                    SubmittedBy = request.UserEmail
                };

                application.ApplicationData.StandardSubmissions.Add(submission);
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