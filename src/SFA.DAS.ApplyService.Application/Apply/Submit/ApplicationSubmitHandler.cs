using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Email.Consts;
using SFA.DAS.ApplyService.Application.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Submit
{
    public class ApplicationSubmitHandler : IRequestHandler<ApplicationSubmitRequest>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IEmailService _emailServiceObject;

        public ApplicationSubmitHandler(IApplyRepository applyRepository, IEmailService emailServiceObject)
        {
            _applyRepository = applyRepository;
            _emailServiceObject = emailServiceObject;
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

            await _applyRepository.UpdateSections(sections);
            
            await _applyRepository.SubmitApplicationSequence(request);

            await NotifyContacts(request.ApplicationId, "unknown reference"); // TODO: Get application reference

            return Unit.Value;
        }

        private async Task NotifyContacts(Guid applicationId, string applicationReference)
        {
            var contactsToNotify = await _applyRepository.GetNotifyContactsForApplication(applicationId);

            foreach (var contact in contactsToNotify)
            {
                // TODO: Think about a better way to send this as it will send a copy to the EPAO team for each contact
                await _emailServiceObject.SendEmail(EmailTemplateName.APPLY_EPAO_INITIAL_SUBMISSION, contact.Email,
                    new { contactname = $"{contact.GivenNames} {contact.FamilyName}", reference = applicationReference });
            }
        }
    }
}