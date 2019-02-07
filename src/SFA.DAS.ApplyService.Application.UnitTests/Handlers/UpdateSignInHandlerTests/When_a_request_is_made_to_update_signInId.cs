using System;
using System.Threading;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Users;
using SFA.DAS.ApplyService.Application.Users.UpdateSignInId;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.UpdateSignInHandlerTests
{
    [TestFixture]
    public class When_a_request_is_made_to_update_signInId
    {
        [Test]
        public void Then_the_request_is_passed_to_the_repository()
        {
            var contactRepository = new Mock<IContactRepository>();

            var handler = new UpdateSignInRequestHandler(contactRepository.Object);

            var signInId = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            
            handler.Handle(new UpdateSignInIdRequest(signInId, contactId), new CancellationToken()).Wait();
            
            contactRepository.Verify(r => r.UpdateSignInId(contactId,signInId));
        }
    }
}