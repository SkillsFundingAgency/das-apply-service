using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Submit;
using SFA.DAS.ApplyService.Application.Email.Consts;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Threading;


namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.ApplicationSubmitHandlerTests
{
    public class When_submitting_sequence_2 : ApplicationSubmitHandlerTestsBase
    {
        [Test]
        public void Then_InitSubmissions_Should_Have_An_Entry()
        {
            var request = new ApplicationSubmitRequest { SequenceId = 2, ApplicationId = Guid.NewGuid(), UserId = Guid.NewGuid(), UserEmail = "Email" };

            Handler.Handle(request, new CancellationToken()).Wait();

            ApplyRepository.Verify(r => r.SubmitApplicationSequence(It.IsAny<ApplicationSubmitRequest>(),
                    It.Is<ApplicationData>(appData => appData.StandardSubmissionsCount == 1
                                            && appData.LatestStandardSubmissionDate.HasValue
                                            && !string.IsNullOrEmpty(appData.ReferenceNumber))
                ));
        }

        [Test]
        public void Then_The_STANDARD_SUBMISSION_Email_Is_Sent()
        {
            var request = new ApplicationSubmitRequest { SequenceId = 2, ApplicationId = Guid.NewGuid(), UserId = Guid.NewGuid(), UserEmail = "Email" };

            Handler.Handle(request, new CancellationToken()).Wait();

            EmailService.Verify(r => r.SendEmailToContact(EmailTemplateName.APPLY_EPAO_STANDARD_SUBMISSION, It.IsAny<Contact>(), It.IsAny<object>()), Times.Once);
        }
    }
}
