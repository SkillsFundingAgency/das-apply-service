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
            var sections = await _applyRepository.GetSections(request.ApplicationId);

            foreach (var section in sections)
            {
                if (section.QnAData.HasFeedback)
                {
                    foreach (var feedback in section.QnAData.Feedback)
                    {
                        feedback.IsNew = false;
                        feedback.IsCompleted = true;
                    }
                }

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
            
           
            var contactsToNotify = await _applyRepository.GetNotifyContactsForApplication(request.ApplicationId);

            await _applyRepository.UpdateSections(sections);

            var referenceNumber = await UpdateApplication(request);

            await NotifyContacts(contactsToNotify, referenceNumber);

            return Unit.Value;
        }

        private async Task<string> UpdateApplication(ApplicationSubmitRequest request)
        {
            var application = await _applyRepository.GetApplication(request.ApplicationId);
            var referenceNumber = application.ApplicationData?.ReferenceNumber;
            
            if (!string.IsNullOrEmpty(referenceNumber))
            {
                application.ApplicationData?.InitSubmissions.Add(new InitSubmission
                {
                    SubmittedAt = DateTime.UtcNow,
                    SubmittedBy = request.UserEmail
                });
                await _applyRepository.SubmitApplicationSequence(request, application.ApplicationData);
            }
            else
            {
                referenceNumber = await CreateReferenceNumber(request.ApplicationId);
                await _applyRepository.SubmitApplicationSequence(request, new ApplicationData
                {
                    InitSubmissions = new List<InitSubmission>
                    {
                        new InitSubmission
                        {
                            SubmittedAt = DateTime.UtcNow,
                            SubmittedBy = request.UserEmail
                        }
                    },
                    ReferenceNumber = referenceNumber
                });
            }

            return referenceNumber;
        }

        private async Task NotifyContacts(IEnumerable<Contact> contactsToNotify, string applicationReference)
        {
            foreach (var contact in contactsToNotify)
            {
                // TODO: Think about a better way to send this as it will send a copy to the EPAO team for each contact
                await _emailServiceObject.SendEmail(EmailTemplateName.APPLY_EPAO_INITIAL_SUBMISSION, contact.Email,
                    new { contactname = $"{contact.GivenNames} {contact.FamilyName}", reference = applicationReference });
            }
        }

        private async Task<string> CreateReferenceNumber(Guid applicationId)
        {
            var referenceNumber = string.Empty;
            var seq = await _applyRepository.GetNextAppReferenceSequence();
            if (seq <= 0) return referenceNumber;
            var refFormat = await _applyRepository.GetWorkflowReferenceFormat(applicationId);
            if (string.IsNullOrEmpty(refFormat)) return referenceNumber;
            referenceNumber = string.Format($"{refFormat}{seq:D6}");

            return referenceNumber;
        }
    }
}