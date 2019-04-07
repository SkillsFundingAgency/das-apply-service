using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Review.Return;
using SFA.DAS.ApplyService.Application.Email.Consts;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Threading;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.ReturnRequestHandlerTests
{
    public class When_returning_sequence_2_with_rejected : ReturnRequestHandlerTestsBase
    {
        [Test]
        public void Then_The_Sequence_Remains_Open_And_Status_Is_Rejected()
        {
            var request = new ReturnRequest(Guid.NewGuid(), 2, "Rejected");

            Handler.Handle(request, new CancellationToken()).Wait();

            ApplyRepository.Verify(r => r.UpdateSequenceStatus(request.ApplicationId, request.SequenceId, ApplicationSequenceStatus.Rejected, ApplicationStatus.Rejected), Times.Once);
            ApplyRepository.Verify(r => r.CloseSequence(request.ApplicationId, request.SequenceId), Times.Never);
        }

        [Test]
        public void Then_The_APPLY_EPAO_RESPONSE_Email_Is_Sent()
        {
            var request = new ReturnRequest(Guid.NewGuid(), 2, "Rejected");

            Handler.Handle(request, new CancellationToken()).Wait();

            EmailService.Verify(r => r.SendEmailToContact(EmailTemplateName.APPLY_EPAO_RESPONSE, It.IsAny<Contact>(), It.IsAny<object>()), Times.Once);
        }
    }
}
