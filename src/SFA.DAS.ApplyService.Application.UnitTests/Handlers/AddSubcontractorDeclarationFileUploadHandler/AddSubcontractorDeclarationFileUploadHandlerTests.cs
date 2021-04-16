using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Gateway;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.AddSubcontractorDeclarationFileUploadHandler
{
    [TestFixture]
    public class AddSubcontractorDeclarationFileUploadHandlerTests
    {
        private Mock<IApplyRepository> _repository;
        private Mock<IGatewayRepository> _gatewayRepository;
        private Apply.Gateway.AddSubcontractorDeclarationFileUploadHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IApplyRepository>();
            _gatewayRepository = new Mock<IGatewayRepository>();
            _handler = new Apply.Gateway.AddSubcontractorDeclarationFileUploadHandler(_repository.Object, _gatewayRepository.Object, Mock.Of<ILogger<Apply.Gateway.AddSubcontractorDeclarationFileUploadHandler>>());
        }

        [Test]
        public async Task AddSubcontractorDeclarationFile_is_updated()
        {
            var applicationId = Guid.NewGuid();
            var clarificationFile = "file.pdf";
            var userId = "user id";
            var userName = "user name";

            var application = new Domain.Entities.Apply {ApplyData = new ApplyData {GatewayReviewDetails = new ApplyGatewayDetails
            {
                GatewaySubcontractorDeclarationClarificationUpload = null
            }}};


            _repository.Setup(x => x.GetApplication(applicationId)).ReturnsAsync(application);

            await _handler.Handle(new AddSubcontractorDeclarationFileUploadRequest(applicationId, clarificationFile, userId, userName), new CancellationToken());

            _gatewayRepository.Verify(x => x.UpdateGatewayApplyData(applicationId, It.IsAny<ApplyData>(), userId, userName), Times.Once);
        }


        [Test]
        public async Task AddSubcontractorDeclarationFile_is_empty_so_is_not_updated()
        {
            var applicationId = Guid.NewGuid();
            var clarificationFile = (string)null;
            var userId = "user id";
            var userName = "user name";

            var application = new Domain.Entities.Apply
            {
                ApplyData = new ApplyData
                {
                    GatewayReviewDetails = new ApplyGatewayDetails
                    {
                        GatewaySubcontractorDeclarationClarificationUpload = null
                    }
                }
            };

            _repository.Setup(x => x.GetApplication(applicationId)).ReturnsAsync(application);

            await _handler.Handle(new AddSubcontractorDeclarationFileUploadRequest(applicationId, clarificationFile, userId, userName), new CancellationToken());

            _repository.Verify(x => x.GetApplication(applicationId), Times.Never);
            _gatewayRepository.Verify(x => x.UpdateGatewayApplyData(applicationId, It.IsAny<ApplyData>(), userId, userName), Times.Never);
        }
    }
}
