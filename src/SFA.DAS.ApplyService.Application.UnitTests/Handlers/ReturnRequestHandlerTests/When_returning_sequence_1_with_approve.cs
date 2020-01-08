using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Review.Return;
using SFA.DAS.ApplyService.Application.Email.Consts;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Threading;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.ReturnRequestHandlerTests
{
    public class When_returning_sequence_1_with_approve : ReturnRequestHandlerTestsBase
    {
        ///////////////////////////////////////////////////////////
        // TODO: THIS WILL NEED RE-WRITING FOR NEW RoATP PROCESS
        ///////////////////////////////////////////////////////////
        
        //[Test]
        //public void Then_The_Sequence_Is_Closed_And_Status_Is_Approved()
        //{
        //    var request = new ReturnRequest(Guid.NewGuid(), 1, "Approve");

        //    Handler.Handle(request, new CancellationToken()).Wait();

        //    ApplyRepository.Verify(r => r.UpdateSequenceStatus(request.ApplicationId, request.SequenceId, ApplicationSequenceStatus.Approved, ApplicationStatus.InProgress), Times.Once);
        //    ApplyRepository.Verify(r => r.CloseSequence(request.ApplicationId, request.SequenceId), Times.Once);
        //}

        //[Test]
        //public void Then_The_Next_Sequence_Is_Opened()
        //{
        //    var request = new ReturnRequest(Guid.NewGuid(), 1, "Approve");

        //    Handler.Handle(request, new CancellationToken()).Wait();

        //    ApplyRepository.Verify(r => r.OpenSequence(request.ApplicationId, request.SequenceId + 1), Times.Once);
        //}

        //[Test]
        //public void Then_The_APPLY_EPAO_UPDATE_Email_Is_Sent()
        //{
        //    var request = new ReturnRequest(Guid.NewGuid(), 1, "Approve");

        //    Handler.Handle(request, new CancellationToken()).Wait();

        //    EmailService.Verify(r => r.SendEmailToContact(EmailTemplateName.APPLY_EPAO_UPDATE, It.IsAny<Contact>(), It.IsAny<object>()), Times.Once);
        //}
    }
}
