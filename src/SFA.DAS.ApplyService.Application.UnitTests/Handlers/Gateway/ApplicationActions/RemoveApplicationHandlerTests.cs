using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Gateway.ApplicationActions;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.Gateway.ApplicationActions
{
    public class RemoveApplicationHandlerTests
    {
        private RemoveApplicationHandler _handler;
        private Mock<IGatewayRepository> _repository;

        private Guid _applicationId;
        private const string _comments = "comments";
        private const string _externalComments = "external comments";
        private const string _userId = "userId";
        private const string _userName = "_userName";

        [SetUp]
        public void SetUp()
        {
            _applicationId = Guid.NewGuid();

            _repository = new Mock<IGatewayRepository>();
            var logger = Mock.Of<ILogger<RemoveApplicationHandler>>();
            
            _handler = new RemoveApplicationHandler(_repository.Object, logger);
        }

        [Test]
        public async Task Handler_removes_application()
        {
            await _handler.Handle(new RemoveApplicationRequest(_applicationId, _comments, _externalComments, _userId, _userName), CancellationToken.None);
            _repository.Verify(x => x.RemoveApplication(_applicationId, _comments, _externalComments, _userId, _userName), Times.Once);
        }
    }
}
