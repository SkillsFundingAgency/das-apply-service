using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Gateway;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.AddSubcontractorDeclarationFileUploadHandler
{
    [TestFixture]
    public class AddSubcontractorDeclarationFileUploadHandlerTests
    {
        private Mock<IApplyRepository> _repository;
        private Apply.Gateway.AddSubcontractorDeclarationFileUploadHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IApplyRepository>();
            _handler = new Apply.Gateway.AddSubcontractorDeclarationFileUploadHandler(_repository.Object, Mock.Of<ILogger<Apply.Gateway.AddSubcontractorDeclarationFileUploadHandler>>());
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

            _repository.Verify(x => x.UpdateGatewayApplyData(applicationId, It.IsAny<ApplyData>(), userId, userName), Times.Once);
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
            _repository.Verify(x => x.UpdateGatewayApplyData(applicationId, It.IsAny<ApplyData>(), userId, userName), Times.Never);
        }
    }
}
