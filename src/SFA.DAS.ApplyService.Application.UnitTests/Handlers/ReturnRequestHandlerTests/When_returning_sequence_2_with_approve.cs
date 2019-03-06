using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Review.Return;
using SFA.DAS.ApplyService.Application.Email.Consts;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Threading;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.ReturnRequestHandlerTests
{
    public class When_returning_sequence_2_with_approve : ReturnRequestHandlerTestsBase
    {
        [Test]
        public void Then_The_Sequence_Is_Closed_And_Status_Is_Approved()
        {
            var request = new ReturnRequest(Guid.NewGuid(), 2, "Approve");

            Handler.Handle(request, new CancellationToken()).Wait();

            ApplyRepository.Verify(r => r.UpdateSequenceStatus(request.ApplicationId, request.SequenceId, ApplicationSequenceStatus.Approved, ApplicationStatus.InProgress), Times.Once);
            ApplyRepository.Verify(r => r.CloseSequence(request.ApplicationId, request.SequenceId), Times.Once);
        }

        [Test]
        public void Then_Application_Is_Approved()
        {
            var request = new ReturnRequest(Guid.NewGuid(), 2, "Approve");

            Handler.Handle(request, new CancellationToken()).Wait();

            ApplyRepository.Verify(r => r.OpenSequence(request.ApplicationId, It.IsAny<int>()), Times.Never);
            ApplyRepository.Verify(r => r.UpdateApplicationStatus(request.ApplicationId, ApplicationStatus.Approved), Times.Once);
        }

        [Test]
        public void Then_The_APPLY_EPAO_RESPONSE_Email_Is_Sent()
        {
            var request = new ReturnRequest(Guid.NewGuid(), 2, "Approve");

            Handler.Handle(request, new CancellationToken()).Wait();

            EmailService.Verify(r => r.SendEmailToContact(EmailTemplateName.APPLY_EPAO_RESPONSE, It.IsAny<Contact>(), It.IsAny<object>()), Times.Once);
        }
    }
}
