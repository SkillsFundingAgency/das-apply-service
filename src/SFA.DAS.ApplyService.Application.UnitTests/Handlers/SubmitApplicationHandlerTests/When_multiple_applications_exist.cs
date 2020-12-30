using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Submit;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.SubmitApplicationHandlerTests
{
    public class When_multiple_applications_exist : SubmitApplicationHandlerTestsBase
    {        
        private readonly Guid differentAppGuid = Guid.NewGuid();
        private readonly Guid sameAppGuid = Guid.NewGuid();

        [SetUp]
        public void AdditionalSetup()
        {
            ApplyRepository.Setup(r => r.CanSubmitApplication(It.Is<Guid>(id => id == differentAppGuid))).ReturnsAsync(false);
            ApplyRepository.Setup(r => r.CanSubmitApplication(It.Is<Guid>(id => id == sameAppGuid))).ReturnsAsync(true);
        }

        [Test]
        public void Then_prevent_submission_if_another_user_has_already_submitted()
        {
            var request = new SubmitApplicationRequest { ApplicationId = differentAppGuid, SubmittingContactId = differentAppGuid };

            Handler.Handle(request, new CancellationToken()).Wait();

            ApplyRepository.Verify(r => r.SubmitApplication(It.IsAny<Guid>(), It.IsAny<ApplyData>(), It.IsAny<FinancialData>(), It.IsAny<Guid>()), Times.Never);
        }

        [Test]
        public void Then_allow_submission_if_user_is_the_one_whom_already_submitted()
        {
            var request = new SubmitApplicationRequest 
            { 
                ApplicationId = sameAppGuid, SubmittingContactId = sameAppGuid,
                ApplyData = new ApplyData
                {
                    ApplyDetails = new ApplyDetails(),
                    Sequences = new List<ApplySequence>()
                }
            };

            Handler.Handle(request, new CancellationToken()).Wait();

            ApplyRepository.Verify(r => r.SubmitApplication(It.IsAny<Guid>(), It.IsAny<ApplyData>(), It.IsAny<FinancialData>(), It.IsAny<Guid>()), Times.Once);
        }
    }
}
