using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Submit;
using SFA.DAS.ApplyService.Application.Email.Consts;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Threading;


namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.ApplicationSubmitHandlerTests
{
    public class When_submitting_sequence_1 : ApplicationSubmitHandlerTestsBase
    {
        ///////////////////////////////////////////////////////////
        // TODO: THIS WILL NEED RE-WRITING FOR NEW RoATP PROCESS
        ///////////////////////////////////////////////////////////

        //[Test]
        //public void Then_InitSubmissions_Should_Have_An_Entry()
        //{
        //    var request = new ApplicationSubmitRequest { SequenceId = 1, ApplicationId = Guid.NewGuid(), UserId = Guid.NewGuid(), UserEmail = "Email" };

        //    Handler.Handle(request, new CancellationToken()).Wait();

        //    ApplyRepository.Verify(r => r.SubmitApplicationSequence(It.IsAny<ApplicationSubmitRequest>(),
        //            It.Is<ApplicationData>(appData => appData.InitSubmissionsCount == 1
        //                                    && appData.LatestInitSubmissionDate.HasValue
        //                                    && !string.IsNullOrEmpty(appData.ReferenceNumber))
        //        ));
        //}

        //[Test]
        //public void Then_The_INITIAL_SUBMISSION_Email_Is_Sent()
        //{
        //    var request = new ApplicationSubmitRequest { SequenceId = 1, ApplicationId = Guid.NewGuid(), UserId = Guid.NewGuid(), UserEmail = "Email" };

        //    Handler.Handle(request, new CancellationToken()).Wait();

        //    EmailService.Verify(r => r.SendEmailToContact(EmailTemplateName.APPLY_EPAO_INITIAL_SUBMISSION, It.IsAny<Contact>(), It.IsAny<object>()), Times.Once);
        //}
    }
}
